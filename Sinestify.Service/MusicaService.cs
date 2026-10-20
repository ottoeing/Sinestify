using Sinesify;
using Sinesify.Business;

namespace Sinesify.Service;

public class MusicaService
{
    private readonly MusicaBusiness business;
    private readonly RepositorioMusicas repositorio;

    public MusicaService()
    {
        business = new MusicaBusiness();
        repositorio = new RepositorioMusicas();
    }

    public List<Musica> Listar()
    {
        return repositorio.Listar();
    }

    public Musica? ObterPorId(int id)
    {
        if (id <= 0)
            return null;

        return repositorio.ObterPorId(id);
    }

    public Musica? Adicionar(Musica musica)
    {
        if (!business.Validar(musica))
            return null;

        return repositorio.Adicionar(musica);
    }

    public Musica? Atualizar(Musica musica)
    {
        if (musica.Id <= 0 || !business.Validar(musica))
            return null;

        var atualizada = repositorio.Atualizar(musica);
        if (!atualizada)
            return null;

        return repositorio.ObterPorId(musica.Id);
    }

    public bool Excluir(int id)
    {
        if (id <= 0)
            return false;

        return repositorio.Excluir(id);
    }
}