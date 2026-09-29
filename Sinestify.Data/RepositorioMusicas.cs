using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioMusicas
{
    private readonly ContextoSinestify conexao;

    public RepositorioMusicas(ContextoSinestify conexao)
    {
        this.conexao = conexao;
    }

    public async Task<List<Musica>> ListarAsync(CancellationToken cancellationToken = default)
    {
        return await conexao.Musicas
            .Include(m => m.Genero)
            .Include(m => m.Emocao)
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Musica?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await conexao.Musicas
            .Include(m => m.Genero)
            .Include(m => m.Emocao)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Musica> AdicionarAsync(Musica musica, CancellationToken cancellationToken = default)
    {
        conexao.Musicas.Add(musica);
        await conexao.SaveChangesAsync(cancellationToken);
        return (await ObterPorIdAsync(musica.Id, cancellationToken))!;
    }

    public async Task<bool> AtualizarAsync(Musica musica, CancellationToken cancellationToken = default)
    {
        var musicaPersistida = await conexao.Musicas.SingleOrDefaultAsync(m => m.Id == musica.Id, cancellationToken);
        if (musicaPersistida is null)
            return false;

        musicaPersistida.Nome = musica.Nome;
        musicaPersistida.Cantor = musica.Cantor;
        musicaPersistida.GeneroId = musica.GeneroId;
        musicaPersistida.EmocaoId = musica.EmocaoId;
        musicaPersistida.Velocidade = musica.Velocidade;
        await conexao.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        var musica = await conexao.Musicas.FindAsync(new object[] { id }, cancellationToken);
        if (musica is null)
            return false;

        conexao.Musicas.Remove(musica);
        await conexao.SaveChangesAsync(cancellationToken);
        return true;
    }
}
