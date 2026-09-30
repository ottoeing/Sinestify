using Sinesify;
using Sinesify.Business;

namespace Sinesify.Service;

public class EmocaoService
{
    private readonly EmocaoBusiness business;
    private readonly RepositorioEmocoes repositorio;

    public EmocaoService(EmocaoBusiness business, RepositorioEmocoes repositorio)
    {
        this.business = business;
        this.repositorio = repositorio;
    }

    public async Task<ResultadoService<IReadOnlyList<Emocao>>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var emocoes = await repositorio.ListarAsync(cancellationToken);
        return ResultadoService<IReadOnlyList<Emocao>>.Ok(emocoes);
    }

    public async Task<ResultadoService<Emocao>> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ResultadoService<Emocao>.DadosInvalidos(["O identificador da emoção deve ser maior que zero."]);

        var emocao = await repositorio.ObterPorIdAsync(id, cancellationToken);
        return emocao is null
            ? ResultadoService<Emocao>.NaoEncontrado("Emoção não encontrada.")
            : ResultadoService<Emocao>.Ok(emocao);
    }

    public async Task<ResultadoService<Emocao>> AdicionarAsync(Emocao emocao, CancellationToken cancellationToken = default)
    {
        var erros = business.Validar(emocao);
        if (erros.Count > 0)
            return ResultadoService<Emocao>.DadosInvalidos(erros);

        var criada = await repositorio.AdicionarAsync(emocao, cancellationToken);
        return ResultadoService<Emocao>.Ok(criada, "Emoção cadastrada com sucesso.");
    }

    public async Task<ResultadoService<Emocao>> AtualizarAsync(Emocao emocao, CancellationToken cancellationToken = default)
    {
        if (emocao.Id <= 0)
            return ResultadoService<Emocao>.DadosInvalidos(["O identificador da emoção deve ser maior que zero."]);

        var erros = business.Validar(emocao);
        if (erros.Count > 0)
            return ResultadoService<Emocao>.DadosInvalidos(erros);

        var atualizada = await repositorio.AtualizarAsync(emocao, cancellationToken);
        if (!atualizada)
            return ResultadoService<Emocao>.NaoEncontrado("Emoção não encontrada.");

        return ResultadoService<Emocao>.Ok(emocao, "Emoção atualizada com sucesso.");
    }

    public async Task<ResultadoService<bool>> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ResultadoService<bool>.DadosInvalidos(["O identificador da emoção deve ser maior que zero."]);

        var excluida = await repositorio.ExcluirAsync(id, cancellationToken);
        return excluida
            ? ResultadoService<bool>.Ok(true, "Emoção excluída com sucesso.")
            : ResultadoService<bool>.NaoEncontrado("Emoção não encontrada.");
    }
}