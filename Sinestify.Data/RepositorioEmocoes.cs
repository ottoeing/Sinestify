using Microsoft.EntityFrameworkCore;

namespace Sinesify;

public class RepositorioEmocoes
{
    private readonly ContextoSinestify conexao = new();

    public List<Emocao> Listar() =>
        conexao.Emocoes.AsNoTracking().OrderBy(e => e.Id).ToList();
}