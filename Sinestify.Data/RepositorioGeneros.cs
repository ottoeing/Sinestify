using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioGeneros
{
    private readonly ContextoSinestify conexao = new();

    public List<Genero> Listar() =>
        conexao.Generos.AsNoTracking().OrderBy(g => g.Id).ToList();
}