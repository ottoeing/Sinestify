using Sinesify;
using Sinesify.Business;

namespace Sinesify.Service;

public class MusicaService
{
    private readonly MusicaBusiness business;
    private readonly RepositorioMusicas repositorio;

    public MusicaService(MusicaBusiness business, RepositorioMusicas repositorio)
    {
        this.business = business;
        this.repositorio = repositorio;
    }

    public async Task<ResultadoService<IReadOnlyList<Musica>>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var musicas = await repositorio.ListarAsync(cancellationToken);
        return ResultadoService<IReadOnlyList<Musica>>.Ok(musicas);
    }

    public async Task<ResultadoService<Musica>> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ResultadoService<Musica>.DadosInvalidos(["O identificador da música deve ser maior que zero."]);

        var musica = await repositorio.ObterPorIdAsync(id, cancellationToken);
        return musica is null
            ? ResultadoService<Musica>.NaoEncontrado("Música não encontrada.")
            : ResultadoService<Musica>.Ok(musica);
    }

    public async Task<ResultadoService<Musica>> AdicionarAsync(Musica musica, CancellationToken cancellationToken = default)
    {
        var erros = business.Validar(musica);
        if (erros.Count > 0)
            return ResultadoService<Musica>.DadosInvalidos(erros);

        var criada = await repositorio.AdicionarAsync(musica, cancellationToken);
        return ResultadoService<Musica>.Ok(criada, "Música cadastrada com sucesso.");
    }

    public async Task<ResultadoService<Musica>> AtualizarAsync(Musica musica, CancellationToken cancellationToken = default)
    {
        if (musica.Id <= 0)
            return ResultadoService<Musica>.DadosInvalidos(["O identificador da música deve ser maior que zero."]);

        var erros = business.Validar(musica);
        if (erros.Count > 0)
            return ResultadoService<Musica>.DadosInvalidos(erros);

        var atualizada = await repositorio.AtualizarAsync(musica, cancellationToken);
        if (!atualizada)
            return ResultadoService<Musica>.NaoEncontrado("Música não encontrada.");

        var resultado = await repositorio.ObterPorIdAsync(musica.Id, cancellationToken);
        return ResultadoService<Musica>.Ok(resultado!, "Música atualizada com sucesso.");
    }

    public async Task<ResultadoService<bool>> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            return ResultadoService<bool>.DadosInvalidos(["O identificador da música deve ser maior que zero."]);

        var excluida = await repositorio.ExcluirAsync(id, cancellationToken);
        return excluida
            ? ResultadoService<bool>.Ok(true, "Música excluída com sucesso.")
            : ResultadoService<bool>.NaoEncontrado("Música não encontrada.");
    }
}