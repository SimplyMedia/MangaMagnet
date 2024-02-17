using MangaMagnet.Api.Service;
using MangaMagnet.Core.Local;
using Quartz;

namespace MangaMagnet.Api.Job;

public class RefreshMetadataJob(MetadataService metadataService, LocalFileService localFileService, ILogger<RefreshMetadataJob> logger) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		logger.LogDebug("RefreshMetadataJob Started");
		await metadataService.UpdateAllMetadataAsync();
		await localFileService.VerifyAllLocalVolumeAndChapters();
		logger.LogDebug("RefreshMetadataJob Finished");
	}
}
