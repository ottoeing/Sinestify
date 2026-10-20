using Sinestify.WebAPI.Dtos;

namespace Sinestify.WebAPI.Services;

public interface IAuthApiService
{
    Task<UsuarioResponse?> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken cancellationToken);
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<UsuarioResponse?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
}
