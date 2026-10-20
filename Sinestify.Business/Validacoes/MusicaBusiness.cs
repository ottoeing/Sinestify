using Sinesify;

namespace Sinestify.Business.Validacoes;

public static class MusicaBusiness
{
    public static bool Validar(Musica? musica)
    {
        if (musica is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(musica.Nome))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(musica.Cantor))
        {
            return false;
        }

        if (musica.Velocidade <= 0 || musica.Velocidade > 220)
        {
            return false;
        }

        if (musica.Genero is null && musica.GeneroId <= 0)
        {
            return false;
        }

        if (musica.Emocao is null && musica.EmocaoId <= 0)
        {
            return false;
        }

        return true;
    }

    public static bool PodeSerTocada(Musica? musica)
    {
        return Validar(musica);
    }
}
