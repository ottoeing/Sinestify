using System.ComponentModel.DataAnnotations;

namespace Sinestify.WebAPI.Dtos;

/// <summary>Dados necessários para criar uma conta no Sinestify.</summary>
public sealed record RegistrarUsuarioRequest
{
    [Required, MaxLength(100)]
    [RegularExpression(@".*\S.*", ErrorMessage = "O nome não pode conter apenas espaços.")]
    public required string Nome { get; init; }

    [Required, EmailAddress, MaxLength(254)]
    public required string Email { get; init; }

    [Required, MinLength(8), MaxLength(128)]
    [RegularExpression(@".*\S.*", ErrorMessage = "A senha não pode conter apenas espaços.")]
    public required string Senha { get; init; }
}
