using MangaMagnet.Core.CBZ;
using MangaMagnet.Core.CBZ.ComicInfo;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Providers.MangaDex;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Download;

public class DownloadService(ILogger<DownloadService> logger, CbzService cbzService, ComicInfoService comicInfoService, MangaDexDownloadService mangaDexDownloadService, BaseDatabaseContext dbContext)
{
	public async Task DownloadChapterAsCBZAsync(double chapterNumber, string mangaDexId, string outputPath, CancellationToken cancellationToken = default)
	{
		var mangaMetadata = dbContext.MangaMetadata.FirstOrDefault(m => m.MangaDexId == mangaDexId)
		                    ?? throw new Exception("Manga metadata not found");

		var (pages, tempPath, metadata) = await mangaDexDownloadService.DownloadChapterPagesAsync(chapterNumber, mangaDexId, cancellationToken);

		var comicInfoInput = new ComicInfoInput(pages,
			new ComicInfoChapterMetadata(metadata.ScanlationGroup, metadata.ChapterTitle, chapterNumber, metadata.Volume, metadata.UploadedAt),
			mangaMetadata,
			ComicInfoVersion.V2);

		var comicInfo = comicInfoService.Create(comicInfoInput);
		await comicInfoService.WriteAsync(comicInfo, tempPath, cancellationToken);

		var fileName = $"{mangaMetadata.DisplayTitle} {chapterNumber}.cbz";

		await cbzService.CreateAsync(tempPath, outputPath, fileName, cancellationToken);

		await Task.Run(() => Directory.Delete(tempPath, true), cancellationToken);

		logger.LogDebug("Deleted temporary directory {Path}", tempPath);
	}
}
