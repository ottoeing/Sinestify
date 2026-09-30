using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
        return Executar(
            () => conexao.Musicas
                .Include(m => m.Genero)
                .Include(m => m.Emocao)
                .OrderBy(m => m.Id)
                .ToList(),
            "Não foi possível listar as músicas.");
    }

    public Musica? ObterPorId(int id)
    {
        return Executar(
            () => conexao.Musicas
                .Include(m => m.Genero)
                .Include(m => m.Emocao)
                .SingleOrDefault(m => m.Id == id),
            "Não foi possível consultar a música.");
    }

    public void Adicionar(Musica musica)
    {
        ArgumentNullException.ThrowIfNull(musica);

        Executar(() =>
        {
            conexao.Musicas.Add(musica);
            conexao.SaveChanges();
        }, "Não foi possível cadastrar a música.");
    }

    public bool Atualizar(Musica musica)
    {
        ArgumentNullException.ThrowIfNull(musica);

        return Executar(() =>
        {
            var existente = conexao.Musicas.Find(musica.Id);
            if (existente is null)
            {
                return false;
            }

            conexao.Entry(existente).CurrentValues.SetValues(musica);
            conexao.SaveChanges();
            return true;
        }, "Não foi possível atualizar a música.");
    }

    public bool Remover(int id)
    {
        return Executar(() =>
        {
            var musica = conexao.Musicas.Find(id);
            if (musica is null)
            {
                return false;
            }

            conexao.Musicas.Remove(musica);
            conexao.SaveChanges();
            return true;
        }, "Não foi possível remover a música.");
    }

    private static T Executar<T>(Func<T> operacao, string mensagem)
    {
        try
        {
            return operacao();
        }
        catch (Exception exception) when (exception is DbUpdateException or DbException)
        {
            throw new ErroPersistenciaException(mensagem, exception);
        }
    }

    private static void Executar(Action operacao, string mensagem)
    {
        Executar(() =>
        {
            operacao();
            return true;
        }, mensagem);
    }
}

public sealed class ErroPersistenciaException : Exception
{
    public ErroPersistenciaException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
