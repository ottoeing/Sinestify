using Sinesify;

namespace Sinesify.Business;

public class EmocaoBusiness
{
    public IReadOnlyList<string> Validar(Emocao emocao)
    {
        if (string.IsNullOrWhiteSpace(emocao.Sentimento))
            return ["O sentimento é obrigatório."];

        return [];
    }
}