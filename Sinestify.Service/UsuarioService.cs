using Microsoft.AspNetCore.Identity;
using Sinesify;
using Sinesify.Business;

namespace Sinesify.Service;

public sealed class UsuarioService
{
    private readonly UsuarioBusiness business = new();
    private readonly RepositorioUsuarios repositorio;
    private readonly PasswordHasher<Usuario> passwordHasher = new();

    public UsuarioService()
    {
        repositorio = new RepositorioUsuarios();
    }

    public Usuario? Registrar(Usuario usuario)
    {
        usuario.Nome = usuario.Nome?.Trim() ?? string.Empty;
        usuario.Email = NormalizarEmail(usuario.Email);
        usuario.Senha ??= string.Empty;

        if (business.ValidarCadastro(usuario).Count > 0)
            return null;

        usuario.Id = 0;
        usuario.SenhaHash = passwordHasher.HashPassword(usuario, usuario.Senha);
        usuario.Senha = string.Empty;

        return repositorio.Adicionar(usuario);
    }

    public IReadOnlyList<string> ValidarCadastro(Usuario usuario)
    {
        return business.ValidarCadastro(new Usuario
        {
            Nome = usuario.Nome?.Trim() ?? string.Empty,
            Email = NormalizarEmail(usuario.Email),
            Senha = usuario.Senha ?? string.Empty
        });
    }

    public Usuario? Login(Usuario credenciais)
    {
        credenciais.Email = NormalizarEmail(credenciais.Email);
        credenciais.Senha ??= string.Empty;

        if (business.ValidarLogin(credenciais).Count > 0)
            return null;

        var usuario = repositorio.ObterPorEmail(credenciais.Email);
        if (usuario is null
            || passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, credenciais.Senha)
                == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return usuario;
    }

    public Usuario? ObterPorId(int id)
    {
        return repositorio.ObterPorId(id);
    }

    private static string NormalizarEmail(string? email)
    {
        return email?.Trim().ToLowerInvariant() ?? string.Empty;
    }
}
