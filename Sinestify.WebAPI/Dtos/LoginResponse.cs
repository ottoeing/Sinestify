namespace Sinestify.WebAPI.Dtos;

/// <summary>Token de acesso e dados públicos do usuário autenticado.</summary>
public sealed record LoginResponse(
    string AccessToken,
    string TokenType,
    DateTimeOffset ExpiresAt,
    UsuarioResponse Usuario);
