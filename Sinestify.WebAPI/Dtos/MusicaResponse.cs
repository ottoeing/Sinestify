namespace Sinestify.WebAPI.Dtos;

/// <summary>Representação pública de uma música.</summary>
public sealed record MusicaResponse(
    int Id,
    string Nome,
    string Cantor,
    int GeneroId,
    string? Genero,
    int EmocaoId,
    string? Emocao,
    int Velocidade);