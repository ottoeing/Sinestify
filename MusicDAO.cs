using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class MusicDAO
{
    private readonly SinestifyContext conexao;

    public MusicDAO(SinestifyContext conexao)
    {
        this.conexao = conexao;
    }

    public List<Music> ListarMusicas()
    {
        return conexao.Musicas
            .Include(m => m.Genero)
            .Include(m => m.Sentimento)
            .OrderBy(m => m.Id)
            .ToList();
    }
}
