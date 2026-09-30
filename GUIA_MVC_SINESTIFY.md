# Guia de Implementacao MVC e Camadas - Sinestify

## Objetivo

Este documento orienta a evolucao do Sinestify para a arquitetura combinada no `Texto.txt`: WebAPI com Controllers MVC, ConsoleApp, Service, Business, Data e Model. Ele parte do codigo que ja existe no Sinestify; nao e uma avaliacao do MeuDiarioSENAC.

## Estado atual do Sinestify

- A solucao `Sinestify.sln` contem os projetos `Sinestify.ConsoleApp` e `Sinestify.Data`.
- `Sinestify.Data` contem `ContextoSinestify`, as entidades `Musica`, `Genero` e `Emocao`, o repositorio `RepositorioMusicas` e as migrations do EF Core.
- `Sinestify.ConsoleApp` contem `Programa`, `Menu` e `Tocador`. O `Programa` configura injecao de dependencias, mas o `Menu` ainda chama diretamente `RepositorioMusicas`.
- O banco usa EF Core com Pomelo para MySQL. O contexto ainda tem uma connection string padrao com credenciais no codigo; isso deve ser removido.
- Ainda nao existem projetos separados `Sinestify.Model`, `Sinestify.Business`, `Sinestify.Service` ou `Sinestify.WebApi`.
- `Texto.txt` serve como referencia para a divisao do trabalho, nao como codigo executavel.

## Arquitetura de destino

```text
Cliente HTTP -> WebAPI (Controllers) --┐
                                       +-> Service -> Business -> Model
ConsoleApp ---------------------------┘           -> Data -> Model -> MySQL
```

Direcao recomendada das referencias entre projetos:

- `Sinestify.Model`: nao referencia outros projetos da aplicacao.
- `Sinestify.Business` -> `Sinestify.Model`.
- `Sinestify.Data` -> `Sinestify.Model`.
- `Sinestify.Service` -> `Sinestify.Business`, `Sinestify.Data` e `Sinestify.Model`.
- `Sinestify.WebApi` -> `Sinestify.Service`.
- `Sinestify.ConsoleApp` -> `Sinestify.Service`.

Business nao deve depender de Service: Service coordena as regras de Business e as operacoes de Data. Assim evitamos dependencia circular. Controllers e ConsoleApp nao acessam DbContext ou repositorios diretamente.

MVC aqui significa Controllers como fronteira HTTP da WebAPI. Como a aplicacao e uma API, nao e necessario criar Views Razor; requests e responses sao representados por DTOs.

## Estrutura desejada

```text
Sinestify.sln
Sinestify.Model/
  Musica.cs
  Genero.cs
  Emocao.cs
Sinestify.Business/
  MusicaBusiness.cs
  GeneroBusiness.cs
  EmocaoBusiness.cs
Sinestify.Data/
  ContextoSinestify.cs
  RepositorioMusicas.cs
  Migrations/
Sinestify.Service/
  MusicaService.cs
  GeneroService.cs
  EmocaoService.cs
Sinestify.WebApi/
  Controllers/
  Dtos/
  Program.cs
Sinestify.ConsoleApp/
  Menu.cs
  Tocador.cs
  Program.cs
```

Manter os nomes de dominio em portugues, como no codigo atual. Usar nomes de arquivo e identificadores sem acentos, por exemplo `Emocao.cs` e `Emocao`, para evitar inconsistencias de ferramentas e sistemas de arquivos.

## Instrucoes por integrante

### Otto - WebAPI/API

**Responsabilidade:** criar a camada HTTP em `Sinestify.WebApi`.

1. Criar um projeto ASP.NET Core Web API e adiciona-lo a `Sinestify.sln`.
2. Configurar Controllers (`AddControllers` e `MapControllers`) e documentacao OpenAPI/Swagger.
3. Criar controllers para musicas, generos e emocoes, com rotas REST consistentes, por exemplo `/api/musicas` e `/api/generos`.
4. Definir DTOs para entrada e saida. Nao expor entidades do EF Core diretamente nem serializar grafos de navegacao.
5. Usar status HTTP apropriados: `200`, `201`, `204`, `400`, `404` e `409` conforme o resultado do caso de uso.
6. Validar formato e obrigatoriedade dos dados de request; regras de dominio ficam em Business e Service.
7. Injetar Services nos controllers. Nao referenciar `Sinestify.Data` nem abrir `ContextoSinestify` nos controllers.
8. Registrar dependencias e configuracao no `Program.cs` da API; nao copiar regras de cadastro para a WebAPI.
9. Criar testes dos endpoints para sucesso, entrada invalida e item inexistente.

### Leandro - Service

**Responsabilidade:** criar a orquestracao compartilhada em `Sinestify.Service`.

1. Criar Services para os casos de uso de musicas, generos e emocoes.
2. Injetar as dependencias de Business e Data; nao instanciar repositorios com `new` dentro dos Services.
3. Executar Business antes de solicitar gravacao ou atualizacao em Data.
4. Traduzir resultados de Data e Business em resultados claros para WebAPI e ConsoleApp, incluindo item nao encontrado e dados invalidos.
5. Manter os mesmos casos de uso disponiveis para ConsoleApp e WebAPI; as interfaces nao devem duplicar regras.
6. Usar operacoes assincronas quando o acesso ao banco for assincrono e propagar `CancellationToken` nos fluxos HTTP quando aplicavel.
7. Nao ler Console, montar resposta HTTP ou configurar EF Core dentro dos Services.

### Guilherme - Data

**Responsabilidade:** persistencia em `Sinestify.Data` e integracao com MySQL.

1. Manter `ContextoSinestify`, configuracao EF Core, migrations e implementacoes de repositorio nesta camada.
2. Atualizar o projeto para referenciar `Sinestify.Model`; nao manter copias das entidades em Data.
3. Receber `DbContextOptions<ContextoSinestify>` por injecao e retirar a connection string padrao com usuario/senha do codigo.
4. Configurar `ConnectionStrings:Sinestify` por ambiente, usando User Secrets ou variaveis de ambiente para credenciais. Nao versionar segredos.
5. Garantir que ConsoleApp e WebAPI registrem o mesmo provider e DbContext por seus pontos de composicao, sem duas configuracoes conflitantes.
6. Preservar as migrations atuais. Ao mover entidades para Model, gerar e revisar uma migration de alteracao, conferir o SQL e testar contra uma copia do banco antes de aplica-la; nao recriar o banco nem substituir migrations existentes sem validar o impacto.
7. Manter consultas e CRUD em repositorios; nao colocar regras de negocio nessa camada.
8. Testar CRUD, relacionamentos Musica-Genero/Emocao, restricoes e migrations com MySQL de teste.

### Gustavo - Business e Model

**Responsabilidade:** dominio em `Sinestify.Model` e regras em `Sinestify.Business`.

1. Criar `Sinestify.Model` e mover para ele as entidades existentes `Musica`, `Genero` e `Emocao`, sem criar tipos duplicados.
2. Criar `Sinestify.Business` com validacoes e regras do dominio, por exemplo nome e cantor obrigatorios, velocidade permitida e existencia de genero/emocao selecionados.
3. Definir limites e invariantes com o grupo antes de fixa-los; manter o mapeamento de banco consistente com as regras aprovadas.
4. Retornar falhas de validacao estruturadas para Service, sem acessar banco, chamar Service ou escrever mensagens no console.
5. Manter Model independente de EF Core, Controllers, ConsoleApp e detalhes de persistencia. Configuracoes especificas do mapeamento ficam em Data.
6. Coordenar com Guilherme a movimentacao de namespaces e o impacto nas migrations.
7. Criar testes unitarios das regras de dominio.

### Integracao do ConsoleApp - responsabilidade compartilhada

O ConsoleApp ja existe e deve continuar como cliente da aplicacao, nao como camada de acesso a dados.

- Leandro adapta o fluxo do menu para chamar Services.
- Otto pode alinhar os contratos de entrada com os DTOs da API quando isso ajudar, sem fazer o ConsoleApp depender da WebAPI.
- O `Programa` do ConsoleApp mantem a composicao de dependencias e configura o DbContext, Data e Services por injecao.
- O `Menu` nao acessa `RepositorioMusicas` ou `ContextoSinestify` diretamente.
- O `Tocador` pode permanecer no ConsoleApp como funcionalidade de apresentacao; ele nao deve conter persistencia ou validacao de dominio.

## Ordem sugerida de implementacao

1. Criar os projetos Model, Business, Service e WebApi; adiciona-los a solucao e acertar as referencias conforme a direcao definida acima.
2. Mover entidades para Model e atualizar Data para usa-las, revisando namespace, snapshot e migrations.
3. Retirar credenciais do codigo e validar a conexao com MySQL em ambiente local de desenvolvimento.
4. Implementar regras de Business e testes unitarios.
5. Implementar Services e fazer o ConsoleApp usar esses casos de uso.
6. Implementar Controllers, DTOs, OpenAPI e testes HTTP.
7. Validar build, testes, migrations e CRUD em MySQL; corrigir README com comandos de configuracao e execucao.

## Criterios para considerar concluido

- `dotnet build Sinestify.sln` compila todos os projetos sem erros.
- WebAPI e ConsoleApp executam os mesmos casos de uso por meio de Service.
- Controllers e menu nao acessam Data diretamente.
- Business nao depende de banco, Service ou interfaces de apresentacao.
- Entidades nao estao duplicadas entre projetos.
- Credenciais nao estao hardcoded nem versionadas.
- Migrations existentes continuam preservadas e uma migracao de banco foi testada sem perda de dados.
- Existem testes de regras, endpoints e persistencia cobrindo os fluxos principais.