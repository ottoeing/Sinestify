using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.IdentityModel.Tokens;
using Sinesify;
using Sinesify.Service;

var builder = WebApplication.CreateBuilder(args);
var tokenService = new TokenService(builder.Configuration);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = tokenService.ChaveAssinatura,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

using (var contexto = new ContextoSinestify())
{
    contexto.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login/auth", ([FromBody] Usuario usuario) =>
{
    var usuarioAutenticado = new UsuarioService().Login(usuario);
    if (usuarioAutenticado is null)
    {
        return Results.Unauthorized();
    }

    return Results.Text(tokenService.GerarToken(usuarioAutenticado));
});

var usuariosGroup = app.MapGroup("/usuarios");

usuariosGroup.MapPost("/registrar", ([FromBody] Usuario usuario) =>
{
    var usuarioCriado = new UsuarioService().Registrar(usuario);
    if (usuarioCriado is null)
    {
        return "Não foi possível registrar o usuário.";
    }

    return $"Usuário {usuarioCriado.Nome} registrado com sucesso.";
});

usuariosGroup.MapPost("/login", ([FromBody] Usuario usuario) =>
{
    var usuarioAutenticado = new UsuarioService().Login(usuario);
    if (usuarioAutenticado is null)
    {
        return "Credenciais inválidas.";
    }

    return $"Bem-vindo, {usuarioAutenticado.Nome}!";
});

var musicasGroup = app.MapGroup("/api/musicas").RequireAuthorization();

musicasGroup.MapGet("/", () => new MusicaService().Listar());

musicasGroup.MapGet("/{id:int}", (int id) =>
{
    var musica = new MusicaService().ObterPorId(id);
    return (object?)musica ?? "Música não encontrada.";
});

musicasGroup.MapPost("/", ([FromBody] Musica musica) =>
{
    musica.Id = 0;
    var musicaCriada = new MusicaService().Adicionar(musica);
    return musicaCriada is null
        ? "Não foi possível cadastrar a música."
        : $"Música {musicaCriada.Nome} cadastrada com sucesso.";
});

musicasGroup.MapPut("/{id:int}", (int id, [FromBody] Musica musica) =>
{
    musica.Id = id;
    var musicaAtualizada = new MusicaService().Atualizar(musica);
    return musicaAtualizada is null
        ? "Não foi possível atualizar a música."
        : $"Música {musicaAtualizada.Nome} atualizada com sucesso.";
});

musicasGroup.MapDelete("/{id:int}", (int id) =>
{
    return new MusicaService().Excluir(id)
        ? "Música removida com sucesso."
        : "Música não encontrada.";
});

app.MapGet("/api/generos", () => new GeneroService().Listar())
.RequireAuthorization();

app.MapGet("/api/emocoes", () => new EmocaoService().Listar())
.RequireAuthorization();

Console.WriteLine("Endpoints disponíveis:");
foreach (var endpoint in app.Services.GetRequiredService<EndpointDataSource>().Endpoints
    .OfType<RouteEndpoint>()
    .OrderBy(endpoint => endpoint.RoutePattern.RawText)
    .ThenBy(endpoint => endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.FirstOrDefault()))
{
    var methods = endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods;
    var methodList = methods is { Count: > 0 } ? string.Join(", ", methods) : "ANY";
    var access = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>().Count > 0
        ? "Autenticação obrigatória"
        : "Público";

    Console.WriteLine($"  {methodList,-7} {endpoint.RoutePattern} - {access}");
}

app.Run();
