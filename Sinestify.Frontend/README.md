# Sinestify Frontend

Front-end React + TypeScript do Sinestify. A aplicação consome a WebAPI existente e funciona como um projeto independente da solução .NET.

## Executar localmente

Na raiz da solução, inicie a API em um terminal. A API usa a conexão configurada em `Sinestify.WebAPI/appsettings.json`; defina a senha do MySQL no ambiente:

```powershell
$env:SINESTIFY_DB_PASSWORD = "senha-do-mysql"
dotnet run --project .\Sinestify.WebAPI\Sinestify.WebAPI.csproj --urls http://localhost:5000
```

Em outro terminal:

```powershell
cd .\Sinestify.Frontend
npm.cmd install
npm.cmd start
```

Abra o endereço mostrado pelo Vite (por padrão `http://localhost:5173`). O proxy do Vite encaminha as rotas da API para `http://localhost:5000`. Para outra URL local, defina `SINESTIFY_API_URL` antes de iniciar `npm.cmd start`.

Se a porta `5000` já estiver em uso, encerre a outra instância da API com `Ctrl+C` ou execute a API em outra porta. Por exemplo, para usar `5001`, inicie a API com `dotnet watch --project .\Sinestify.WebAPI\Sinestify.WebAPI.csproj --urls http://localhost:5001` e, no terminal do frontend, defina `$env:SINESTIFY_API_URL = "http://localhost:5001"` antes de `npm.cmd start`.

No PowerShell do Windows, use `npm.cmd` em vez de `npm` se a política de execução bloquear `npm.ps1`. Também é possível iniciar com `npm.cmd run dev`.

Para uma compilação de produção servida em outra origem, defina `VITE_API_BASE_URL` com a URL base da API antes de executar `npm.cmd run build` e configure a variável `FrontendOrigin` da API com a origem do site (por exemplo, `https://sinestify.exemplo.com`). O token JWT é enviado como Bearer; a API deve estar publicada com HTTPS.

## Recursos

- Cadastro e login com JWT; o token fica em `sessionStorage` e é enviado como Bearer nas rotas protegidas.
- Consulta de músicas, gêneros e emoções.
- Busca e filtros por gênero e emoção.
- Criação, edição e remoção de músicas.
- Visualização rítmica animada baseada na coleção; não há reprodução de áudio na API atual.

Para compilar a versão de produção, execute `npm.cmd run build`.
