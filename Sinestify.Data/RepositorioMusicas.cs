using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioMusicas
{
    private readonly ContextoSinestify conexao = new();

    public List<Musica> Listar()
    {
        return conexao.Musicas
            .Include(m => m.Genero)
            .Include(m => m.Emocao)
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .ToList();
    }

    public Musica? ObterPorId(int id)
    {
        return conexao.Musicas
            .Include(m => m.Genero)
            .Include(m => m.Emocao)
            .AsNoTracking()
            .SingleOrDefault(m => m.Id == id);
    }

    public Musica Adicionar(Musica musica)
    {
        conexao.Musicas.Add(musica);
        conexao.SaveChanges();
        return ObterPorId(musica.Id)!;
    }

    public bool Atualizar(Musica musica)
    {
        var musicaPersistida = conexao.Musicas.SingleOrDefault(m => m.Id == musica.Id);
        if (musicaPersistida is null)
            return false;

        musicaPersistida.Nome = musica.Nome;
        musicaPersistida.Cantor = musica.Cantor;
        musicaPersistida.GeneroId = musica.GeneroId;
        musicaPersistida.EmocaoId = musica.EmocaoId;
        musicaPersistida.Velocidade = musica.Velocidade;
        conexao.SaveChanges();
        return true;
    }

    public bool Excluir(int id)
    {
        var musica = conexao.Musicas.Find(id);
        if (musica is null)
            return false;

        conexao.Musicas.Remove(musica);
        conexao.SaveChanges();
        return true;
    }
}
