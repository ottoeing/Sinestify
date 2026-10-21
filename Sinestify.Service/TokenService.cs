using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sinesify;

namespace Sinesify.Service;

public sealed class TokenService
{
    private const double ValidadePadraoEmHoras = 2;
    private readonly double validadeEmHoras;

    public SymmetricSecurityKey ChaveAssinatura { get; }

    public TokenService(IConfiguration configuration)
    {
        var secretKey = configuration["JWTSettings:SecretKey"];
        if (string.IsNullOrWhiteSpace(secretKey) || Encoding.UTF8.GetByteCount(secretKey) < 32)
        {
            throw new InvalidOperationException(
                "JWTSettings:SecretKey deve conter ao menos 32 bytes UTF-8 e ser configurada por um segredo privado.");
        }

        ChaveAssinatura = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var expirationSetting = configuration["JWTSettings:ExpirationInHours"];
        if (string.IsNullOrWhiteSpace(expirationSetting))
        {
            validadeEmHoras = ValidadePadraoEmHoras;
            return;
        }

        if (!double.TryParse(expirationSetting, NumberStyles.Float, CultureInfo.InvariantCulture, out var expirationInHours)
            || !double.IsFinite(expirationInHours)
            || expirationInHours <= 0)
        {
            throw new InvalidOperationException(
                "JWTSettings:ExpirationInHours deve ser um número maior que zero.");
        }

        validadeEmHoras = expirationInHours;
    }

    public string GerarToken(Usuario usuario)
    {
        var credentials = new SigningCredentials(ChaveAssinatura, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Nome)
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(validadeEmHoras)
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }
}
