namespace Sinestify.WebAPI.Dtos;

/// <summary>Dados públicos de uma conta, sem informações de autenticação.</summary>
public sealed record UsuarioResponse(int Id, string Nome, string Email);
