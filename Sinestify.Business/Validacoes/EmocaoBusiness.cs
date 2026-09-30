using Sinesify;

namespace Sinestify.Business.Validacoes;

public static class EmocaoBusiness
{
    private static readonly HashSet<string> EmocoesPermitidas = new(StringComparer.OrdinalIgnoreCase)
    {
        "Alegria",
        "Tristeza",
        "Euforia",
        "Saudade",
        "Calma",
        "Nostalgia",
        "Esperança",
        "Melancolia",
        "Empolgação",
        "Romantismo"
    };

    public static bool Validar(Emocao? emocao)
    {
        if (emocao is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(emocao.Sentimento))
        {
            return false;
        }

        var sentimento = emocao.Sentimento.Trim();

        if (!EmocoesPermitidas.Contains(sentimento))
        {
            return false;
        }

        return true;
    }
}
