#if DEBUG
using Asp.Versioning;
using MangaMagnet.Core.Download;
using MangaMagnet.Core.Progress;
using Microsoft.AspNetCore.Mvc;

namespace MangaMagnet.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class TestController(ProgressService progressService, DownloadService downloadService) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> ProgressTask()
	{
		using var task = progressService.CreateTask("Test");

		for (var i = 0; i < 100; i++)
		{
			task.Progress = i;
			await Task.Delay(20);
		}

		return Ok();
	}

	[HttpPost("download/{id:guid}/{chapterNumber:double}")]
	public async Task<IActionResult> TestDownload(Guid id, double chapterNumber)
	{
		await downloadService.DownloadChapterAsync(chapterNumber, id.ToString(), "E:/Temp");

		return Ok();
	}
}
#endif
