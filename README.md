# Funções do código

## Projetos

| Projeto | Código |
| --- | --- |
| [Sinestify.Model](./Sinestify.Model/) | Entidades `Usuario`, `Musica`, `Genero` e `Emocao`. |
| [Sinestify.Business](./Sinestify.Business/) | Validação de dados de usuário e música. |
| [Sinestify.Data](./Sinestify.Data/) | Contexto MySQL, repositórios e migrações. |
| [Sinestify.Service](./Sinestify.Service/) | Regras e operações que usam os repositórios. |
| [Sinestify.ConsoleApp](./Sinestify.ConsoleApp/) | Menus que chamam os serviços. |
| [Sinestify.WebAPI](./Sinestify.WebAPI/) | Rotas HTTP que chamam os serviços. |

O fluxo das operações é: menu ou rota → serviço → validação de negócio, quando aplicável → repositório → `ContextoSinestify`. Os serviços são instanciados diretamente e as operações do banco são síncronas.

## Regras de negócio

### `UsuarioBusiness`

- `ValidarCadastro(usuario)`: exige nome, e-mail válido e senha entre 8 e 128 caracteres; também verifica os limites de tamanho para nome e e-mail.
- `ValidarLogin(usuario)`: exige e-mail e senha.

### `MusicaBusiness`

- `Validar(musica)`: exige nome, cantor, IDs positivos de gênero e emoção, e BPM maior que zero.

## Serviços

### `UsuarioService`

- `Registrar(usuario)`: remove espaços externos do nome, converte o e-mail para minúsculas, valida os dados, gera `SenhaHash` com `PasswordHasher<Usuario>` e pede ao repositório para salvar. Retorna `null` quando os dados são inválidos ou o e-mail já existe.
- `Login(credenciais)`: normaliza o e-mail, valida as credenciais, busca o usuário e compara a senha informada com o hash. Retorna o usuário ou `null`.
- `ObterPorId(id)`: delega a busca ao repositório.

### `MusicaService`

- `Listar()`: retorna as músicas do repositório.
- `ObterPorId(id)`: rejeita IDs menores ou iguais a zero; caso contrário, busca a música.
- `Adicionar(musica)`: valida os campos e grava a música; retorna `null` se a validação falhar.
- `Atualizar(musica)`: exige ID positivo e dados válidos, atualiza o registro e retorna a música atualizada; retorna `null` se não for possível atualizar.
- `Excluir(id)`: exige ID positivo e retorna se o registro foi excluído.

### Serviços auxiliares

- `GeneroService.Listar()` e `EmocaoService.Listar()`: retornam os dados de referência.
- `TokenService.GerarToken(usuario)`: gera um JWT com o nome do usuário, assinatura HMAC-SHA256 e prazo configurado.

## Persistência

### `ContextoSinestify`

- `OnConfiguring()`: configura o provedor MySQL e detecta a versão do servidor com `ServerVersion.AutoDetect`.
- `OnModelCreating(modelBuilder)`: define tabelas, chaves, limites de tamanho, índices únicos, relacionamentos e dados iniciais.

O banco é `sinestify` em `127.0.0.1:3306`, usuário `root`. A senha é lida da variável de ambiente `SINESTIFY_DB_PASSWORD`.

### Repositórios

- `RepositorioMusicas.Listar()` e `ObterPorId(id)`: consultam músicas incluindo `Genero` e `Emocao`.
- `RepositorioMusicas.Adicionar(musica)`, `Atualizar(musica)` e `Excluir(id)`: inserem, alteram e removem músicas.
- `RepositorioUsuarios.ObterPorEmail(email)` e `ObterPorId(id)`: consultam usuários sem rastreamento do EF.
- `RepositorioUsuarios.Adicionar(usuario)`: insere o usuário; retorna `null` para e-mail duplicado e propaga outras falhas de persistência.
- `RepositorioGeneros.Listar()` e `RepositorioEmocoes.Listar()`: consultam os dados de referência.

### Migrações

Os arquivos ficam em [Sinestify.Data/Migrations](./Sinestify.Data/Migrations/):

- `CriacaoInicial`: cria tabelas e insere os dados iniciais.
- `RestringirRelacionamentosEDefinirConstraints`: adiciona limites, índices únicos e regras de relacionamento.
- `AddUsuarios`: cria a tabela de usuários.

Aplicar as migrações ao banco:

```powershell
dotnet ef database update --project .\Sinestify.Data\Sinestify.Data.csproj --startup-project .\Sinestify.ConsoleApp\Sinestify.ConsoleApp.csproj
```

## ConsoleApp

`Programa.Main()` instancia `MusicaService`, `GeneroService` e `EmocaoService`, cria `Menu` e chama `Iniciar()`.

Funções de `Menu`:

- `Iniciar()`: alterna entre autenticação e menu principal de acordo com `usuarioAtual`.
- `MostrarTelaAutenticacao()`: exibe as opções de registrar, entrar ou sair.
- `RegistrarUsuario()` e `FazerLogin()`: leem os dados e chamam `UsuarioService`; após cadastro bem-sucedido, o usuário entra automaticamente.
- `MostrarMenuPrincipal()`: exibe as operações de música e a opção de desconectar.
- `ListarMusicas()`, `CadastrarMusica()`, `EditarMusica()` e `ExcluirMusica()`: chamam `MusicaService`.
- `EscolherGenero()` e `EscolherEmocao()`: carregam e selecionam itens pelos serviços de consulta.
- `TocarMusica()`: busca uma música e chama `Tocador.Tocar(musica)`.

`Tocador.Tocar(musica)` anima barras no terminal. `ObterCorPorEmocao()` seleciona a cor pelo sentimento; `ObterIncrementoPorVelocidade()` e `ObterAtrasoPorVelocidade()` ajustam o movimento pelo BPM. Uma tecla encerra a animação.

Executar o ConsoleApp em um terminal:

```powershell
dotnet run --project .\Sinestify.ConsoleApp\Sinestify.ConsoleApp.csproj
```

## WebAPI

As rotas estão em [Sinestify.WebAPI/Program.cs](./Sinestify.WebAPI/Program.cs). `UseAuthentication()` lê a autenticação da requisição; `UseAuthorization()` aplica a exigência às rotas marcadas com `RequireAuthorization()`.

### Exemplo de criação de música

O grupo `musicasGroup` tem o prefixo `/api/musicas`. Portanto, `MapPost("/")` registra `POST /api/musicas`:

```csharp
musicasGroup.MapPost("/", ([FromBody] Musica musica) =>
{
    musica.Id = 0;
    var musicaCriada = new MusicaService().Adicionar(musica);
    return musicaCriada is null
        ? "Não foi possível cadastrar a música."
        : $"Música {musicaCriada.Nome} cadastrada com sucesso.";
});
```

Essa rota **cria** uma música; não consulta uma já existente. `[FromBody]` converte o JSON do corpo da requisição em `Musica`. O ID é zerado para o banco gerar um novo. `MusicaService.Adicionar()` valida os campos e, se forem válidos, `RepositorioMusicas.Adicionar()` grava o registro. A rota retorna texto de sucesso ou falha, não o objeto criado.

Corpo JSON esperado:

```json
{
  "nome": "Minha música",
  "cantor": "Artista",
  "generoId": 1,
  "emocaoId": 1,
  "velocidade": 100
}
```

Envie esse JSON no corpo de um `POST /api/musicas`. Inclua no cabeçalho `Authorization` o token JWT obtido em `POST /login/auth`, usando o esquema Bearer. `generoId` e `emocaoId` devem corresponder a registros existentes.

### Rotas e funções

| Método e rota | Função |
| --- | --- |
| `POST /usuarios/registrar` | Recebe os dados do usuário e chama `UsuarioService.Registrar()`. |
| `POST /usuarios/login` | Verifica as credenciais com `UsuarioService.Login()` e responde com texto. |
| `POST /login/auth` | Autentica com `UsuarioService.Login()` e gera JWT com `TokenService.GerarToken()` se válido. |
| `GET /api/musicas` | Chama `MusicaService.Listar()` e retorna a lista. |
| `GET /api/musicas/{id}` | Chama `MusicaService.ObterPorId(id)`. |
| `POST /api/musicas` | Cria música com os dados JSON enviados no corpo. |
| `PUT /api/musicas/{id}` | Define o ID da rota e chama `MusicaService.Atualizar()`. |
| `DELETE /api/musicas/{id}` | Chama `MusicaService.Excluir(id)`. |
| `GET /api/generos` | Chama `GeneroService.Listar()`. |
| `GET /api/emocoes` | Chama `EmocaoService.Listar()`. |

As rotas `/api/musicas`, `/api/generos` e `/api/emocoes` exigem autenticação JWT. As rotas de usuário não estão marcadas com `RequireAuthorization()`. Respostas de validação e não encontrado são mensagens textuais; essas rotas não usam `BadRequest` nem `NotFound`.

## Configuração lida pelas funções

### Banco de dados

Defina `SINESTIFY_DB_PASSWORD` no terminal antes de executar a API ou o ConsoleApp:

```powershell
$env:SINESTIFY_DB_PASSWORD = "senha-do-mysql"
```

### JWT

O código define `TokenService.ChaveSecreta` como chave padrão com o valor literal `Sinestify`. A WebAPI usa essa chave para validar tokens e o `TokenService` para gerá-los quando `JWTSettings:SecretKey` não está configurada. `TokenService` também lê `JWTSettings:ExpirationInHours` ao gerar o token.

```powershell
$env:JWTSettings__ExpirationInHours = "2"
```

Esse valor é curto e conhecido, portanto qualquer pessoa que conheça a chave pode gerar tokens aceitos pela API. Use-o apenas para estudos e testes locais, não numa API publicada. Para substituir a chave via ambiente, defina `JWTSettings__SecretKey` com uma chave aleatória privada.

Inicie a API no mesmo terminal para que receba as variáveis:

```powershell
dotnet run --project .\Sinestify.WebAPI\Sinestify.WebAPI.csproj
```
