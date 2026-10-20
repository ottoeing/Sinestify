using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sinesify;
using Sinesify.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("A configuração JWTSettings:SecretKey é obrigatória.");
var secretKeyBytes = Encoding.ASCII.GetBytes(secretKey);

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
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login/auth", ([FromBody] Usuario usuario) =>
{
    var usuarioAutenticado = new UsuarioService().Login(usuario);
    if (usuarioAutenticado is null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new TokenService(builder.Configuration).GerarToken(usuarioAutenticado));
});

var usuariosGroup = app.MapGroup("/usuarios");

usuariosGroup.MapPost("/registrar", ([FromBody] Usuario usuario) =>
{
    var usuarioCriado = new UsuarioService().Registrar(usuario);
    if (usuarioCriado is null)
    {
        return Results.BadRequest(new { mensagem = "Não foi possível registrar o usuário." });
    }

    return Results.Ok(usuarioCriado);
});

usuariosGroup.MapPost("/login", ([FromBody] Usuario usuario) =>
{
    var usuarioAutenticado = new UsuarioService().Login(usuario);
    if (usuarioAutenticado is null)
    {
        return Results.BadRequest(new { mensagem = "Credenciais inválidas." });
    }

    return Results.Ok(usuarioAutenticado);
});

var musicasGroup = app.MapGroup("/api/musicas").RequireAuthorization();

musicasGroup.MapGet("/", () => new MusicaService().Listar());

musicasGroup.MapGet("/{id:int}", (int id) =>
{
    var musica = new MusicaService().ObterPorId(id);
    return musica is null ? Results.NotFound() : Results.Ok(musica);
});

musicasGroup.MapPost("/", ([FromBody] Musica musica) =>
{
    musica.Id = 0;
    var musicaCriada = new MusicaService().Adicionar(musica);
    return musicaCriada is null
        ? Results.BadRequest(new { mensagem = "Não foi possível cadastrar a música." })
        : Results.Ok(musicaCriada);
});

musicasGroup.MapPut("/{id:int}", (int id, [FromBody] Musica musica) =>
{
    musica.Id = id;
    var musicaAtualizada = new MusicaService().Atualizar(musica);
    return musicaAtualizada is null
        ? Results.BadRequest(new { mensagem = "Não foi possível atualizar a música." })
        : Results.Ok(musicaAtualizada);
});

musicasGroup.MapDelete("/{id:int}", (int id) =>
{
    return new MusicaService().Excluir(id)
        ? Results.Ok("Música removida com sucesso.")
        : Results.NotFound();
});

app.MapGet("/api/generos", () => new GeneroService().Listar())
.RequireAuthorization();

app.MapGet("/api/emocoes", () => new EmocaoService().Listar())
.RequireAuthorization();

app.Run();
