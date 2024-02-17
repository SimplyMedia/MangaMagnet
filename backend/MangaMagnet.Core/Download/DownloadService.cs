using MangaMagnet.Core.CBZ;
using MangaMagnet.Core.CBZ.ComicInfo;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Progress;
using MangaMagnet.Core.Providers.MangaDex;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Download;

public class DownloadService(ILogger<DownloadService> logger, CbzService cbzService, ComicInfoService comicInfoService, MangaDexDownloadService mangaDexDownloadService, ProgressService progressService, BaseDatabaseContext dbContext)
{
	public async Task DownloadChapterAsCBZAsync(Guid id, double chapterNumber, string outputPath, CancellationToken cancellationToken = default)
	{
		var localManga = dbContext.LocalMangas
			.Include(m => m.Metadata)
			.FirstOrDefault(m => m.Id == id) ?? throw new Exception("Manga not found");

		var mangaMetadata = localManga.Metadata;

		using var task = progressService.CreateTask($"Downloading Chapter {chapterNumber}");
		task.Description = mangaMetadata.DisplayTitle;

		var (pages, tempPath, metadata) = await mangaDexDownloadService.DownloadChapterPagesAsync(chapterNumber, mangaMetadata.MangaDexId, task, cancellationToken);

		task.Description = $"Writing ComicInfo for Chapter {chapterNumber}";

		var comicInfoInput = new ComicInfoInput(pages,
			new ComicInfoChapterMetadata(metadata.ScanlationGroup, metadata.ChapterTitle, chapterNumber, metadata.Volume, metadata.UploadedAt),
			mangaMetadata,
			ComicInfoVersion.V2);

		var comicInfo = comicInfoService.Create(comicInfoInput);
		await comicInfoService.WriteAsync(comicInfo, tempPath, cancellationToken);

		task.Description = $"Compressing Pages into .cbz";

		var fileName = $"{mangaMetadata.DisplayTitle} {chapterNumber} (Scan) ({metadata.ScanlationGroup})";

		await cbzService.CreateAsync(tempPath, outputPath, fileName, cancellationToken);

		task.Description = $"Deleting temporary folder";

		await Task.Run(() => Directory.Delete(tempPath, true), cancellationToken);

		logger.LogDebug("Deleted temporary directory {Path}", tempPath);
	}
}
