using Sinesify;
namespace Sinesify.Service;

public class EmocaoService
{
    private readonly RepositorioEmocoes repositorio;

    public EmocaoService()
    {
        repositorio = new RepositorioEmocoes();
    }

    public List<Emocao> Listar()
    {
        return repositorio.Listar();
    }
}