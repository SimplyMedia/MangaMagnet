using MangaMagnet.Core.Providers.MangaDex.Models.Download;
using MangaMagnet.Core.Util;

namespace MangaMagnet.Core.Providers.MangaDex;

public static class MangaDexConstants
{
	public static string ReportMangaUrl = $"{ApiNetworkBaseUrl}/{ReportEndpoint}";

    public static string SearchMangaByNameUrl = $"{ApiBaseUrl}/{MangaEndpoint}";

    public static string FetchMangaCoversUrl = $"{ApiBaseUrl}/{CoverEndpoint}";

    public static string FetchMangaMetadataByIdUrl(string mangaDexId) => $"{ApiBaseUrl}/{MangaEndpoint}/{mangaDexId}";

    public static string FetchMangaStatisticsUrl(string mangaDexId) => $"{ApiBaseUrl}/{StatisticsEndpoint}/{MangaEndpoint}/{mangaDexId}";

    public static string FetchMangaChapterUrl(string mangaDexId) => $"{ApiBaseUrl}/{MangaEndpoint}/{mangaDexId}/{ChapterEndpoint}";

    public static string FetchMangaChapterPagesUrl(string chapterId) => $"{ApiBaseUrl}/{AtHomeEndpoint}/{ServerEndpoint}/{chapterId}";

    public static string DownloadMangaChapterPageUrl(string baseUrl, MangaDexQuality quality, string chapterHash, string fileName)
	    => $"{baseUrl}/{quality.GetDisplayName()}/{chapterHash}/{fileName}";

    private const string ApiBaseUrl = "https://api.mangadex.org";
    private const string MangaEndpoint = "manga";
    private const string StatisticsEndpoint = "statistics";
    private const string CoverEndpoint = "cover";
    private const string ChapterEndpoint = "feed";
    private const string AtHomeEndpoint = "at-home";
    private const string ServerEndpoint = "server";
    private const string ApiNetworkBaseUrl = "https://api.mangadex.network";
    private const string ReportEndpoint = "report";
}
