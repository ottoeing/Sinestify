using Sinesify;

namespace Sinesify.Business;

public class MusicaBusiness
{
    public bool Validar(Musica musica)
    {
        if (string.IsNullOrWhiteSpace(musica.Nome))
            return false;
        if (string.IsNullOrWhiteSpace(musica.Cantor))
            return false;
        if (musica.GeneroId <= 0)
            return false;
        if (musica.EmocaoId <= 0)
            return false;
        if (musica.Velocidade <= 0)
            return false;

        return true;
    }
}