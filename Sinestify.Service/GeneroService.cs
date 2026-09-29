using Sinesify;
using Sinesify.Business;

namespace Sinesify.Service;

public class GeneroService
{
    private readonly GeneroBusiness business;
    private readonly RepositorioGeneros repositorio;

    public GeneroService(GeneroBusiness business, RepositorioGeneros repositorio)
    {
        this.business = business;
        this.repositorio = repositorio;
    }

    public async Task<ResultadoService<IReadOnlyList<Genero>>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var generos = await repositorio.ListarAsync(cancellationToken);
        return ResultadoService<IReadOnlyList<Genero>>.Ok(generos);
    }

    public async Task<ResultadoService<Genero>> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ResultadoService<Genero>.DadosInvalidos(["O identificador do gênero deve ser maior que zero."]);

        var genero = await repositorio.ObterPorIdAsync(id, cancellationToken);
        return genero is null
            ? ResultadoService<Genero>.NaoEncontrado("Gênero não encontrado.")
            : ResultadoService<Genero>.Ok(genero);
    }

    public async Task<ResultadoService<Genero>> AdicionarAsync(Genero genero, CancellationToken cancellationToken = default)
    {
        var erros = business.Validar(genero);
        if (erros.Count > 0)
            return ResultadoService<Genero>.DadosInvalidos(erros);

        var criado = await repositorio.AdicionarAsync(genero, cancellationToken);
        return ResultadoService<Genero>.Ok(criado, "Gênero cadastrado com sucesso.");
    }

    public async Task<ResultadoService<Genero>> AtualizarAsync(Genero genero, CancellationToken cancellationToken = default)
    {
        if (genero.Id <= 0)
            return ResultadoService<Genero>.DadosInvalidos(["O identificador do gênero deve ser maior que zero."]);

        var erros = business.Validar(genero);
        if (erros.Count > 0)
            return ResultadoService<Genero>.DadosInvalidos(erros);

        var atualizado = await repositorio.AtualizarAsync(genero, cancellationToken);
        if (!atualizado)
            return ResultadoService<Genero>.NaoEncontrado("Gênero não encontrado.");

        return ResultadoService<Genero>.Ok(genero, "Gênero atualizado com sucesso.");
    }

    public async Task<ResultadoService<bool>> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ResultadoService<bool>.DadosInvalidos(["O identificador do gênero deve ser maior que zero."]);

        var excluido = await repositorio.ExcluirAsync(id, cancellationToken);
        return excluido
            ? ResultadoService<bool>.Ok(true, "Gênero excluído com sucesso.")
            : ResultadoService<bool>.NaoEncontrado("Gênero não encontrado.");
    }
}