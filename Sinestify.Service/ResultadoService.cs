namespace Sinesify.Service;

public enum StatusResultadoService
{
    Sucesso,
    DadosInvalidos,
    NaoEncontrado
}

public sealed record ResultadoService<T>(
    StatusResultadoService Status,
    T? Valor,
    string Mensagem,
    IReadOnlyList<string> Erros)
{
    public bool Sucesso => Status == StatusResultadoService.Sucesso;

    public static ResultadoService<T> Ok(T valor, string mensagem = "Operação realizada com sucesso.") =>
        new(StatusResultadoService.Sucesso, valor, mensagem, []);

    public static ResultadoService<T> DadosInvalidos(IReadOnlyList<string> erros) =>
        new(StatusResultadoService.DadosInvalidos, default, string.Join(" ", erros), erros);

    public static ResultadoService<T> NaoEncontrado(string mensagem) =>
        new(StatusResultadoService.NaoEncontrado, default, mensagem, []);
}