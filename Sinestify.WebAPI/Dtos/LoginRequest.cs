using System.ComponentModel.DataAnnotations;

namespace Sinestify.WebAPI.Dtos;

/// <summary>Credenciais usadas para iniciar uma sessão.</summary>
public sealed record LoginRequest
{
    [Required, EmailAddress, MaxLength(254)]
    public required string Email { get; init; }

    [Required, MaxLength(128)]
    public required string Senha { get; init; }
}
