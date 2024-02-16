using MangaMagnet.Core.Providers.MangaDex;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Chapter;

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

        var mangaDexStatistic = await mangaDexApiService.FetchMangaStatisticsAsync(id, cancellationToken);

        var mangaDexCovers = await mangaDexApiService.FetchMangaCoversAsync(id, cancellationToken);

        return mangaDexConverterService.ConvertToMangaMetadataResult(mangaDexMangaData, mangaDexStatistic.Statistics.First().Value, mangaDexCovers);
    }

    public async Task<List<ChapterMetadataResult>> FetchAllChapterMetadataAsync(string id, CancellationToken cancellationToken = default)
    {
	    var mangaDexChapterData = await mangaDexApiService.FetchAllMangaChaptersAsync(id, cancellationToken);
	    return MapMangaDexChaptersToChapterMetadataResults(mangaDexChapterData);
    }

    public async Task<List<ChapterMetadataResult>> FetchLatestChapterMetadataAsync(string id, CancellationToken cancellationToken = default)
    {
	    var latestChapters = await mangaDexApiService.FetchLatestMangaChaptersAsync(id, cancellationToken);
	    return MapMangaDexChaptersToChapterMetadataResults(latestChapters);
    }

    private List<ChapterMetadataResult> MapMangaDexChaptersToChapterMetadataResults(IEnumerable<MangaDexChapter> mangaDexChapterData)
    {
	    var grouped = mangaDexChapterData.GroupBy(d => d.Attributes.Chapter);

	    var chapterMetadataResults = new List<ChapterMetadataResult>();

	    foreach (var group in grouped)
	    {
		    if (!double.TryParse(group.Key, out var chapterNumber))
			    continue; // We ignore chapters that don't have a number, likely oneshot specials or something

		    var volumeNumberNullable = group.FirstOrDefault(c => string.IsNullOrEmpty(c.Attributes.Volume) == false)?.Attributes.Volume;
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
