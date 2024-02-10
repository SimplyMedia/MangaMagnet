using MangaMagnet.Api.Service;
using Quartz;

namespace MangaMagnet.Api.Job;

public class UpdateMetadataJob(MetadataService metadataService, ILogger<UpdateMetadataJob> logger) : IJob
{
	public Task Execute(IJobExecutionContext context)
	{
		logger.LogDebug("Updating all metadata of manga");
		return metadataService.UpdateAllMetadataAsync();
	}
}
