using Microsoft.AspNetCore.Mvc;
using Sinestify.WebAPI.Dtos;
using Sinestify.WebAPI.Services;

namespace Sinestify.WebAPI.Controllers;

[ApiController]
[Route("api/musicas")]
public sealed class MusicasController : ControllerBase
{
    private readonly IMusicaApiService musicaService;

    public MusicasController(IMusicaApiService musicaService)
    {
        this.musicaService = musicaService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MusicaResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<MusicaResponse>> GetAll()
    {
        return Ok(musicaService.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MusicaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MusicaResponse> GetById(int id)
    {
        var musica = musicaService.GetById(id);
        return musica is null ? NotFound() : Ok(musica);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MusicaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<MusicaResponse> Create(CreateMusicaRequest request)
    {
        var resultado = musicaService.Criar(request);

        if (resultado.Status == ApiOperationStatus.Invalid)
            return BadRequest(new { erros = resultado.Errors });

        return CreatedAtAction(nameof(GetById), new { id = resultado.Value!.Id }, resultado.Value);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MusicaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<MusicaResponse> Update(int id, UpdateMusicaRequest request)
    {
        var resultado = musicaService.Atualizar(id, request);
        if (resultado.Status == ApiOperationStatus.NotFound)
            return NotFound();
        if (resultado.Status == ApiOperationStatus.Invalid)
            return BadRequest(new { erros = resultado.Errors });

        return Ok(resultado.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        return musicaService.Excluir(id) ? NoContent() : NotFound();
    }

}