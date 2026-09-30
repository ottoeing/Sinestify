using Sinesify;

namespace Sinestify.Business.Validacoes;

public static class GeneroBusiness
{
    public static bool Validar(Genero? genero)
    {
        if (genero is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(genero.Nome))
        {
            return false;
        }

        if (genero.Nome.Trim().Length < 2)
        {
            return false;
        }

        return true;
    }
}
