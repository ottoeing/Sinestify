using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioEmocoes
{
    private readonly ContextoSinestify conexao;

    public RepositorioEmocoes(ContextoSinestify conexao)
    {
        this.conexao = conexao;
    }

    public Task<List<Emocao>> ListarAsync(CancellationToken cancellationToken = default) =>
        conexao.Emocoes.AsNoTracking().OrderBy(e => e.Id).ToListAsync(cancellationToken);

    public Task<Emocao?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        conexao.Emocoes.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Emocao> AdicionarAsync(Emocao emocao, CancellationToken cancellationToken = default)
    {
        conexao.Emocoes.Add(emocao);
        await conexao.SaveChangesAsync(cancellationToken);
        return emocao;
    }

    public async Task<bool> AtualizarAsync(Emocao emocao, CancellationToken cancellationToken = default)
    {
        var persistida = await conexao.Emocoes.SingleOrDefaultAsync(e => e.Id == emocao.Id, cancellationToken);
        if (persistida is null)
            return false;

        persistida.Sentimento = emocao.Sentimento;
        await conexao.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        var emocao = await conexao.Emocoes.FindAsync(new object[] { id }, cancellationToken);
        if (emocao is null)
            return false;

        conexao.Emocoes.Remove(emocao);
        await conexao.SaveChangesAsync(cancellationToken);
        return true;
    }
}