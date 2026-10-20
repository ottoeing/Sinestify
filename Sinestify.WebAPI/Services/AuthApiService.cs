using Microsoft.AspNetCore.Identity;
using Sinestify.WebAPI.Dtos;
using Sinesify;

namespace Sinestify.WebAPI.Services;

public sealed class AuthApiService(
    RepositorioUsuarios repositorio,
    PasswordHasher<Usuario> passwordHasher,
    TokenService tokenService) : IAuthApiService
{
    public async Task<UsuarioResponse?> RegistrarAsync(
        RegistrarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        var nome = request.Nome.Trim();
        var email = NormalizarEmail(request.Email);
        if (nome.Length == 0 || email.Length == 0 || string.IsNullOrWhiteSpace(request.Senha))
        {
            return null;
        }

        var usuario = new Usuario
        {
            Nome = nome,
            Email = email
        };
        usuario.SenhaHash = passwordHasher.HashPassword(usuario, request.Senha);

        var usuarioCriado = await repositorio.AdicionarAsync(usuario, cancellationToken);
        return usuarioCriado is null ? null : Mapear(usuarioCriado);
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var email = NormalizarEmail(request.Email);
        if (email.Length == 0 || string.IsNullOrWhiteSpace(request.Senha))
        {
            return null;
        }

        var usuario = await repositorio.ObterPorEmailAsync(email, cancellationToken);
        if (usuario is null
            || passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, request.Senha)
                == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var (token, expiresAt) = tokenService.GerarToken(usuario);
        return new LoginResponse(token, "Bearer", expiresAt, Mapear(usuario));
    }

    public async Task<UsuarioResponse?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var usuario = await repositorio.ObterPorIdAsync(id, cancellationToken);
        return usuario is null ? null : Mapear(usuario);
    }

    private static string NormalizarEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static UsuarioResponse Mapear(Usuario usuario)
    {
        return new UsuarioResponse(usuario.Id, usuario.Nome, usuario.Email);
    }
}
