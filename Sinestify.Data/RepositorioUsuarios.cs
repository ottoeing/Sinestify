using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public sealed class RepositorioUsuarios
{
    private readonly ContextoSinestify conexao = new();

    public Usuario? ObterPorEmail(string email)
    {
        return conexao.Usuarios
            .AsNoTracking()
            .SingleOrDefault(usuario => usuario.Email == email);
    }

    public Usuario? ObterPorId(int id)
    {
        return conexao.Usuarios
            .AsNoTracking()
            .SingleOrDefault(usuario => usuario.Id == id);
    }

    public Usuario? Adicionar(Usuario usuario)
    {
        conexao.Usuarios.Add(usuario);

        try
        {
            conexao.SaveChanges();
            return usuario;
        }
        catch (DbUpdateException)
        {
            conexao.Entry(usuario).State = EntityState.Detached;

            if (conexao.Usuarios.Any(existente => existente.Email == usuario.Email))
            {
                return null;
            }

            throw;
        }
    }
}
