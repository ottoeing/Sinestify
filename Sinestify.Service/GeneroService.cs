using Sinesify;
namespace Sinesify.Service;

public class GeneroService
{
    private readonly RepositorioGeneros repositorio;

    public GeneroService()
    {
        repositorio = new RepositorioGeneros();
    }

    public List<Genero> Listar()
    {
        return repositorio.Listar();
    }
}