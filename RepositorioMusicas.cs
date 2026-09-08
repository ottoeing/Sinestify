using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioMusicas
{
    private readonly ContextoSinestify conexao;

    public RepositorioMusicas(ContextoSinestify conexao)
    {
        this.conexao = conexao;
    }

    public List<Musica> ListarMusicas()
    {
        return conexao.Musicas
            .Include(m => m.Genero)
            .Include(m => m.Emocao)
            .OrderBy(m => m.Id)
            .ToList();
    }
}
