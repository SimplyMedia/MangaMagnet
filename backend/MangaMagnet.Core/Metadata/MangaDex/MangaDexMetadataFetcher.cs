using MangaMagnet.Core.Providers.MangaDex;

namespace MangaMagnet.Core.Metadata.MangaDex;

public class MangaDexMetadataFetcher(MangaDexApiService mangaDexApiService, MangaDexConverterService mangaDexConverterService) : IMetadataFetcher
{
    public async Task<List<MangaSearchMetadataResult>> SearchMangaMetadataAsync(string mangaName, CancellationToken cancellationToken = default)
    {
        var mangaDexSearchResult = await mangaDexApiService.SearchMangaByNameAsync(mangaName, cancellationToken);

        return mangaDexSearchResult.Select(mangaDexConverterService.ConvertToMangaSearchMetadataResult).ToList();
    }

    public async Task<MangaMetadataResult> FetchMangaMetadataAsync(string id, CancellationToken cancellationToken = default)
    {
        var mangaDexMangaData = await mangaDexApiService.FetchMangaMetadataByIdAsync(id, cancellationToken);

        var mangaDexStatistic = await mangaDexApiService.FetchMangaStatistics(id, cancellationToken);

        return mangaDexConverterService.ConvertToMangaMetadataResult(mangaDexMangaData, mangaDexStatistic.Statistics.First().Value);
    }

    public async Task<List<ChapterMetadataResult>> FetchAllChapterMetadataAsync(string id, CancellationToken cancellationToken = default)
    {
        var mangaDexChapterData = await mangaDexApiService.FetchMangaChapters(id, cancellationToken);
        var grouped = mangaDexChapterData.GroupBy(d => d.Attributes.Chapter);

        var chapterMetadataResults = new List<ChapterMetadataResult>();

        foreach (var group in grouped)
        {
	        if (!double.TryParse(group.Key, out var chapterNumber))
		        continue; // We ignore chapters that don't have a number, likely oneshot specials or something

	        var anyChapterReleaseWithVolume = group.FirstOrDefault(c => string.IsNullOrEmpty(c.Attributes.Volume) == false);

	        var volumeNumberNullable = anyChapterReleaseWithVolume?.Attributes.Volume;
	        int? volumeNumber = volumeNumberNullable != null ? int.Parse(volumeNumberNullable) : null;

	        // find first uploaded chapter with a title
	        var title = group.FirstOrDefault(c => c.Attributes.TranslatedLanguage == "en" && !string.IsNullOrEmpty(c.Attributes.Title))?.Attributes.Title ?? null;

	        chapterMetadataResults.Add(new ChapterMetadataResult(
		        chapterNumber,
		        volumeNumber,
		        title));
        }

        return chapterMetadataResults;
    }
}
