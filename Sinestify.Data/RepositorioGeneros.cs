using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioGeneros
{
    private readonly ContextoSinestify conexao;

    public RepositorioGeneros(ContextoSinestify conexao)
    {
        this.conexao = conexao;
    }

    public Task<List<Genero>> ListarAsync(CancellationToken cancellationToken = default) =>
        conexao.Generos.AsNoTracking().OrderBy(g => g.Id).ToListAsync(cancellationToken);

    public Task<Genero?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        conexao.Generos.AsNoTracking().SingleOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<Genero> AdicionarAsync(Genero genero, CancellationToken cancellationToken = default)
    {
        conexao.Generos.Add(genero);
        await conexao.SaveChangesAsync(cancellationToken);
        return genero;
    }

    public async Task<bool> AtualizarAsync(Genero genero, CancellationToken cancellationToken = default)
    {
        var persistido = await conexao.Generos.SingleOrDefaultAsync(g => g.Id == genero.Id, cancellationToken);
        if (persistido is null)
            return false;

        persistido.Nome = genero.Nome;
        await conexao.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        var genero = await conexao.Generos.FindAsync(new object[] { id }, cancellationToken);
        if (genero is null)
            return false;

        conexao.Generos.Remove(genero);
        await conexao.SaveChangesAsync(cancellationToken);
        return true;
    }
}