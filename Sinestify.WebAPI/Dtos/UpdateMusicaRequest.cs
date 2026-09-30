using System.ComponentModel.DataAnnotations;

namespace Sinestify.WebAPI.Dtos;

/// <summary>Dados para atualizar uma música existente.</summary>
public sealed record UpdateMusicaRequest
{
    [Required, MaxLength(200)]
    public required string Nome { get; init; }

    [Required, MaxLength(200)]
    public required string Cantor { get; init; }

    [Range(1, int.MaxValue)]
    public int GeneroId { get; init; }

    [Range(1, int.MaxValue)]
    public int EmocaoId { get; init; }

    [Range(1, int.MaxValue)]
    public int Velocidade { get; init; }
}