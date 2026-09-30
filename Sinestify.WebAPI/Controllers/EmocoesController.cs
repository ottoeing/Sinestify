using Microsoft.AspNetCore.Mvc;
using Sinestify.WebAPI.Dtos;
using Sinestify.WebAPI.Services;

namespace Sinestify.WebAPI.Controllers;

[ApiController]
[Route("api/emocoes")]
public sealed class EmocoesController : ControllerBase
{
    private readonly IEmocaoApiService emocaoService;

    public EmocoesController(IEmocaoApiService emocaoService)
    {
        this.emocaoService = emocaoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmocaoResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<EmocaoResponse>> GetAll()
    {
        return Ok(emocaoService.GetAll());
    }
}