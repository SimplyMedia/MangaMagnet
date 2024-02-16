using MangaMagnet.Api.Service;
using Quartz;

namespace MangaMagnet.Api.Job;

public class CheckForNewChaptersJob(MetadataService metadataService, ILogger<RefreshMetadataJob> logger) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		logger.LogDebug("CheckForNewChaptersJob Started");
		await metadataService.CheckAllMangaForNewChapterMetadataAsync();
		logger.LogDebug("CheckForNewChaptersJob Finished");
	}
}
