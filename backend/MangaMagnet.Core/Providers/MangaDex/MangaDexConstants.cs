using MangaMagnet.Core.Providers.MangaDex.Models;
using MangaMagnet.Core.Providers.MangaDex.Models.Download;
using MangaMagnet.Core.Util;

namespace MangaMagnet.Core.Providers.MangaDex;

public static class MangaDexConstants
{
	public static string ReportMangaUrl => $"{ApiNetworkBaseUrl}/{ReportEndpoint}";

    public static string SearchMangaByNameUrl => $"{ApiBaseUrl}/{MangaEndpoint}";

    public static string FetchMangaMetadataByIdUrl(string mangaDexId) => $"{ApiBaseUrl}/{MangaEndpoint}/{mangaDexId}";

    public static string FetchMangaStatisticsUrl(string mangaDexId) => $"{ApiBaseUrl}/{StatisticsEndpoint}/{MangaEndpoint}/{mangaDexId}";

    public static string FetchMangaChapterUrl(string mangaDexId) => $"{ApiBaseUrl}/{MangaEndpoint}/{mangaDexId}/{ChapterEndpoint}";

    public static string FetchMangaChapterPagesUrl(string chapterId) => $"{ApiBaseUrl}/{AtHomeEndpoint}/{ServerEndpoint}/{chapterId}";

    public static string DownloadMangaChapterPageUrl(string baseUrl, MangaDexQuality quality, string chapterHash, string fileName)
	    => $"{baseUrl}/{quality.GetDisplayName()}/{chapterHash}/{fileName}";

    private static string ApiBaseUrl => "https://api.mangadex.org";
    private static string MangaEndpoint => "manga";
    private static string StatisticsEndpoint => "statistics";
    private static string ChapterEndpoint => "feed";
    private static string AtHomeEndpoint => "at-home";
    private static string ServerEndpoint => "server";
    private static string ApiNetworkBaseUrl => "https://api.mangadex.network";
    private static string ReportEndpoint => "report";
}
