using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sinesify;
using Sinestify.WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Sinestify")
    ?? throw new InvalidOperationException("A connection string 'Sinestify' precisa ser configurada.");
var jwtSection = builder.Configuration.GetSection("Authentication:Jwt");
var secretKey = jwtSection["SecretKey"]
    ?? throw new InvalidOperationException("A configuração Authentication:Jwt:SecretKey é obrigatória.");
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
if (secretKeyBytes.Length < 32)
{
    throw new InvalidOperationException("A chave JWT deve ter pelo menos 32 bytes.");
}

var issuer = jwtSection["Issuer"] ?? "Sinestify";
var audience = jwtSection["Audience"] ?? "Sinestify";
var signingKey = new SymmetricSecurityKey(secretKeyBytes);

builder.Services.AddDbContext<ContextoSinestify>(options =>
    options.UseSinestifyMySql(connectionString));
builder.Services.AddScoped<RepositorioUsuarios>();
builder.Services.AddScoped<PasswordHasher<Usuario>>();
builder.Services.AddSingleton(new TokenService(signingKey, issuer, audience));
builder.Services.AddScoped<IAuthApiService, AuthApiService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();