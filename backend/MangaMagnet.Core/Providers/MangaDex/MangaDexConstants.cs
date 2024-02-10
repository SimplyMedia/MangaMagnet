namespace MangaMagnet.Core.Providers.MangaDex;

public static class MangaDexConstants
{
    public static string ApiBaseUrl => "https://api.mangadex.org";
    public static string MangaEndpoint => "manga";
    public static string StatisticsEndpoint => "statistics";
    public static string ChapterEndpoint => "feed";

    public static string SearchMangaByNameUrl => $"{ApiBaseUrl}/{MangaEndpoint}";

    public static string FetchMangaMetadataByIdUrl(string mangaDexId) => $"{ApiBaseUrl}/{MangaEndpoint}/{mangaDexId}";

    public static string FetchMangaStatisticsUrl(string mangaDexId) => $"{ApiBaseUrl}/{StatisticsEndpoint}/{MangaEndpoint}/{mangaDexId}";

    public static string FetchMangaChapterUrl(string mangaDexId) => $"{ApiBaseUrl}/{MangaEndpoint}/{mangaDexId}/{ChapterEndpoint}";
}
