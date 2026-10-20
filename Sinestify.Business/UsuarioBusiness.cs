using System.ComponentModel.DataAnnotations;
using Sinesify;

namespace Sinesify.Business;

public class UsuarioBusiness
{
    public IReadOnlyList<string> ValidarCadastro(Usuario? usuario)
    {
        if (usuario is null)
            return ["Os dados do usuário são obrigatórios."];

        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(usuario.Nome))
            erros.Add("O nome é obrigatório.");
        else if (usuario.Nome.Length > 100)
            erros.Add("O nome deve ter no máximo 100 caracteres.");

        if (string.IsNullOrWhiteSpace(usuario.Email))
            erros.Add("O e-mail é obrigatório.");
        else if (usuario.Email.Length > 254)
            erros.Add("O e-mail deve ter no máximo 254 caracteres.");
        else if (!new EmailAddressAttribute().IsValid(usuario.Email))
            erros.Add("Informe um e-mail válido.");

        if (string.IsNullOrWhiteSpace(usuario.Senha))
            erros.Add("A senha é obrigatória.");
        else if (usuario.Senha.Length < 8)
            erros.Add("A senha deve ter pelo menos 8 caracteres.");
        else if (usuario.Senha.Length > 128)
            erros.Add("A senha deve ter no máximo 128 caracteres.");

        return erros;
    }

    public IReadOnlyList<string> ValidarLogin(Usuario? usuario)
    {
        if (usuario is null)
            return ["As credenciais são obrigatórias."];

        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(usuario.Email))
            erros.Add("O e-mail é obrigatório.");

        if (string.IsNullOrWhiteSpace(usuario.Senha))
            erros.Add("A senha é obrigatória.");

        return erros;
    }
}
