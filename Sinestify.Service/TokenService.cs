using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sinesify;

namespace Sinesify.Service;

public sealed class TokenService(IConfiguration configuration)
{
    public const string ChaveSecreta = "Sinestify";

    public string GerarToken(Usuario usuario)
    {
        var jwtSettings = configuration.GetSection("JWTSettings");
        var secretKey = jwtSettings["SecretKey"] ?? ChaveSecreta;
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, usuario.Nome)
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpirationInHours"]!))
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }
}
