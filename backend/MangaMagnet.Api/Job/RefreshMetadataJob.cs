using MangaMagnet.Api.Service;
using Quartz;

namespace MangaMagnet.Api.Job;

public class RefreshMetadataJob(MetadataService metadataService, ILogger<RefreshMetadataJob> logger) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		logger.LogDebug("RefreshMetadataJob Started");
		await metadataService.UpdateAllMetadataAsync();
		logger.LogDebug("RefreshMetadataJob Finished");
	}
}
