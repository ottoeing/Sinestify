using Sinesify;

namespace Sinesify.Business;

public class GeneroBusiness
{
    public IReadOnlyList<string> Validar(Genero genero)
    {
        if (string.IsNullOrWhiteSpace(genero.Nome))
            return ["O nome do gênero é obrigatório."];

        return [];
    }
}