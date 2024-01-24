namespace MangaMagnet.Core.Metadata;

public interface IMetadataFetcher
{
    Task<List<MangaSearchMetadataResult>> SearchMangaMetadataAsync(string mangaName, CancellationToken cancellationToken = default);

    Task<MangaMetadataResult> FetchMangaMetadataAsync(string id, CancellationToken cancellationToken = default);

    Task<List<ChapterMetadataResult>> FetchAllChapterMetadataAsync(string id, CancellationToken cancellationToken = default);
}