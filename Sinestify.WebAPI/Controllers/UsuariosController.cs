using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sinestify.WebAPI.Dtos;
using Sinestify.WebAPI.Services;

namespace Sinestify.WebAPI.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(IAuthApiService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("registrar")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UsuarioResponse>> Registrar(
        RegistrarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        var usuario = await authService.RegistrarAsync(request, cancellationToken);
        if (usuario is null)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "E-mail já cadastrado",
                Detail = "Já existe uma conta cadastrada com esse e-mail."
            });
        }

        return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await authService.LoginAsync(request, cancellationToken);
        return resultado is null
            ? Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Credenciais inválidas",
                Detail = "E-mail ou senha incorretos."
            })
            : Ok(resultado);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioResponse>> ObterPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var usuarioAutenticado = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(usuarioAutenticado, out var usuarioId) || usuarioId != id)
        {
            return Forbid();
        }

        var usuario = await authService.ObterPorIdAsync(id, cancellationToken);
        return usuario is null ? NotFound() : Ok(usuario);
    }
}
