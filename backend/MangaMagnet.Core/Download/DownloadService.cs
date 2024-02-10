using MangaMagnet.Core.CBZ;
using MangaMagnet.Core.CBZ.ComicInfo;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Providers.MangaDex;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Download;

public class DownloadService(ILogger<DownloadService> logger, CbzService cbzService, ComicInfoService comicInfoService, MangaDexApiService mangaDexApiService, BaseDatabaseContext dbContext)
{
	public async Task DownloadChapterAsync(double chapterNumber, string mangaDexId, string outputDirectory, CancellationToken cancellationToken = default)
	{
		var mangaMetadata = dbContext.MangaMetadata.FirstOrDefault(m => m.MangaDexId == mangaDexId)
		                    ?? throw new Exception("Manga metadata not found");

		var mangadexChapters = await mangaDexApiService.FetchMangaChapters(mangaDexId, cancellationToken);

		var chapters = mangadexChapters.FindAll(x => x.Attributes.Chapter == chapterNumber.ToString());

		var (chapterId, _, attributes, relationships) = chapters.First(c => c.Attributes.TranslatedLanguage == "en");
		var chapterTitle = attributes.Title;
		var uploadedAt = attributes.PublishAt;
		int? volume = string.IsNullOrEmpty(attributes.Volume) ? null : int.Parse(attributes.Volume);
		var scanlationGroup = relationships.First(x => x.Type == "scanlation_group").Attributes.Name;

		var tempPageFolder = $"{Path.GetTempPath()}/MangaMagnet-{Guid.NewGuid().ToString()}";

		await Task.Run(() => Directory.CreateDirectory(tempPageFolder), cancellationToken);
		logger.LogDebug("Created temporary directory {Path}", tempPageFolder);

		var imagePaths = await mangaDexApiService.DownloadMangaChapterPagesAsync(tempPageFolder, chapterId, MangaDexQuality.ORIGINAL, cancellationToken);

		var comicInfo = comicInfoService.Create(imagePaths, scanlationGroup, chapterTitle, chapterNumber, volume, uploadedAt, mangaMetadata, ComicInfoVersion.V2);

		await comicInfoService.WriteAsync(comicInfo, tempPageFolder, cancellationToken);

		await cbzService.CreateAsync(tempPageFolder, outputDirectory, "test", cancellationToken);

		await Task.Run(() => Directory.Delete(tempPageFolder, true), cancellationToken);

		logger.LogDebug("Deleted temporary directory {Path}", tempPageFolder);
	}
}
