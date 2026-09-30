using Microsoft.AspNetCore.Mvc;
using Sinestify.WebAPI.Dtos;
using Sinestify.WebAPI.Services;

namespace Sinestify.WebAPI.Controllers;

[ApiController]
[Route("api/generos")]
public sealed class GenerosController : ControllerBase
{
    private readonly IGeneroApiService generoService;

    public GenerosController(IGeneroApiService generoService)
    {
        this.generoService = generoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<GeneroResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<GeneroResponse>> GetAll()
    {
        return Ok(generoService.GetAll());
    }
}