using System.Net;
using Asp.Versioning;
using MangaMagnet.Api.Models.Request;
using MangaMagnet.Api.Models.Response;
using MangaMagnet.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace MangaMagnet.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class MangaController(MangaService mangaService) : ControllerBase
{
    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(MangaResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMangaRequest request)
    {
        var manga = await mangaService.CreateAsync(request.MangaDexId, request.Path);

        return CreatedAtAction("GetSingle", new { id = manga.Id }, manga);
    }

    [HttpPost("batch")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(MangaResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateBatchAsync([FromBody] IReadOnlyList<CreateMangaRequest> request)
    {
	    var responses = await Task.WhenAll(request.Select(r => mangaService.CreateAsync(r.MangaDexId, r.Path)));

	    return Created("/api/manga", responses);
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(List<MangaResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync()
        => Ok(await mangaService.GetAllAsync());

    [HttpGet("{id:guid}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(MangaResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSingleAsync([FromRoute] Guid id)
        => Ok(await mangaService.GetByIdAsync(id));

    [HttpDelete("{id:guid}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(MangaResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        => Ok(await mangaService.DeleteByIdAsync(id));
}
