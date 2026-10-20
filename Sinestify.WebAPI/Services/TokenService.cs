using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Sinesify;

namespace Sinestify.WebAPI.Services;

public sealed class TokenService(
    SymmetricSecurityKey signingKey,
    string issuer,
    string audience)
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(2);

    public (string Token, DateTimeOffset ExpiresAt) GerarToken(Usuario usuario)
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.Add(TokenLifetime);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            now.UtcDateTime,
            expiresAt.UtcDateTime,
            credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
