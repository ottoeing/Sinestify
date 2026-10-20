using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public sealed class RepositorioUsuarios(ContextoSinestify conexao)
{
    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return conexao.Usuarios
            .AsNoTracking()
            .SingleOrDefaultAsync(usuario => usuario.Email == email, cancellationToken);
    }

    public Task<Usuario?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return conexao.Usuarios
            .AsNoTracking()
            .SingleOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public async Task<Usuario?> AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        conexao.Usuarios.Add(usuario);

        try
        {
            await conexao.SaveChangesAsync(cancellationToken);
            return usuario;
        }
        catch (DbUpdateException)
        {
            conexao.Entry(usuario).State = EntityState.Detached;

            if (await conexao.Usuarios.AnyAsync(
                    existente => existente.Email == usuario.Email,
                    cancellationToken))
            {
                return null;
            }

            throw;
        }
    }
}
