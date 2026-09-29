using Sinesify;

namespace Sinesify.Business;

public class MusicaBusiness
{
    public IReadOnlyList<string> Validar(Musica musica)
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(musica.Nome))
            erros.Add("O nome da música é obrigatório.");
        if (string.IsNullOrWhiteSpace(musica.Cantor))
            erros.Add("O cantor é obrigatório.");
        if (musica.GeneroId <= 0)
            erros.Add("Selecione um gênero válido.");
        if (musica.EmocaoId <= 0)
            erros.Add("Selecione uma emoção válida.");
        if (musica.Velocidade <= 0)
            erros.Add("A velocidade deve ser maior que zero.");

        return erros;
    }
}