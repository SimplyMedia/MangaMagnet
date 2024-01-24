using System.Net;
using Asp.Versioning;
using MangaMagnet.Api.Models.Response;
using MangaMagnet.Api.Service;
using MangaMagnet.Core.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace MangaMagnet.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class MetadataController(MetadataService metadataService) : ControllerBase
{
	[HttpGet("search")]
	[MapToApiVersion("1.0")]
	[ProducesResponseType(typeof(List<MangaSearchMetadataResult>), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> SearchAsync([FromQuery] string query)
		=> Ok(await metadataService.SearchMangaMetadataByNameAsync(query));

	[HttpGet("{mangaDexId}")]
	[MapToApiVersion("1.0")]
	[ProducesResponseType(typeof(MangaSearchMetadataResult), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> GetAsync([FromRoute] string mangaDexId)
		=> Ok(await metadataService.FetchMangaMetadataAsync(mangaDexId));

	[HttpGet("{mangaDexId}/chapters")]
	[MapToApiVersion("1.0")]
	[ProducesResponseType(typeof(List<ChapterMetadataResult>), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> GetAllChaptersAsync([FromRoute] string mangaDexId)
		=> Ok(await metadataService.FetchAllChapterMetadataAsync(mangaDexId));

	[HttpPatch("{mangaDexId}")]
	[MapToApiVersion("1.0")]
	[ProducesResponseType(typeof(MangaSearchMetadataResult), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> UpdateAsync([FromRoute] string mangaDexId)
		=> Ok(await metadataService.RefreshMetadataAsync(mangaDexId));

	[HttpPatch("batch")]
	[MapToApiVersion("1.0")]
	[ProducesResponseType(typeof(List<MangaMetadataResponse>), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> UpdateBatchAsync([FromBody] IReadOnlyList<string> mangaDexIds)
	{
		var responses = await Task.WhenAll(mangaDexIds.Select(id => metadataService.RefreshMetadataAsync(id)));

		return Ok(responses);
	}

	[HttpPatch("all")]
	[MapToApiVersion("1.0")]
	[ProducesResponseType(typeof(List<MangaMetadataResponse>), (int)HttpStatusCode.OK)]
	public async Task<IActionResult> UpdateAllAsync()
	{
		var responses = await metadataService.UpdateAllMetadataAsync();

		return Ok(responses);
	}
}
