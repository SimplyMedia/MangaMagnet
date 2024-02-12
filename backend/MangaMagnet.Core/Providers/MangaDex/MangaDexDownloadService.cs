using MangaMagnet.Core.Providers.MangaDex.Models;
using MangaMagnet.Core.Providers.MangaDex.Models.Download;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Providers.MangaDex;

public class MangaDexDownloadService(ILogger<MangaDexDownloadService> logger, MangaDexApiService mangaDexApiService)
{
	public async Task<MangaDexDownloadResult> DownloadChapterPagesAsync(double chapterNumber, string mangaDexId, CancellationToken cancellationToken)
	{
		var mangadexChapters = await mangaDexApiService.FetchMangaChapters(mangaDexId, cancellationToken);

		var chapters = mangadexChapters.FindAll(x => x.Attributes.Chapter == chapterNumber.ToString());

		var englishChapter = chapters.FirstOrDefault(c => c.Attributes.TranslatedLanguage == "en");

		if (englishChapter == null)
			throw new Exception("No English chapter found");

		var (chapterId, _, attributes, relationships) = englishChapter;

		var chapterTitle = attributes.Title;
		var uploadedAt = attributes.PublishAt;
		int? volume = string.IsNullOrEmpty(attributes.Volume) ? null : int.Parse(attributes.Volume);
		var scanlationGroup = relationships.First(x => x.Type == "scanlation_group").Attributes.Name;

		var tempPageFolder = $"{Path.GetTempPath()}/MangaMagnet-{Guid.NewGuid().ToString()}";

		await Task.Run(() => Directory.CreateDirectory(tempPageFolder), cancellationToken);
		logger.LogDebug("Created temporary directory {Path}", tempPageFolder);

		var imagePaths = await mangaDexApiService.DownloadMangaChapterPagesAsync(tempPageFolder, chapterId, MangaDexQuality.ORIGINAL, cancellationToken);
		return new MangaDexDownloadResult(imagePaths, tempPageFolder, new MangaDexDownloadMetadata(scanlationGroup, chapterTitle, uploadedAt, volume));
	}
}
