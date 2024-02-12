namespace MangaMagnet.Core.Providers.MangaDex;

public static class MangaDexHeaderUtil
{
    public static Dictionary<string, string> GetFakeHeaders()
    {
        var headers = new Dictionary<string, string>
        {
            {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"},
            {"Sec-Fetch-Site", "same-site"},
            {"Sec-Fetch-Mode", "cors"},
            {"Sec-Fetch-Dest", "empty"},
            {"Sec-Ch-Ua-Platform", "\"Windows\""},
            {"Sec-Ch-Ua-Mobile", "?0"},
            {"Referer", "https://mangadex.org/"},
            {"Origin", "https://mangadex.org"},
            {"Dnt", "1"},
            {"Sec-Ch-Ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\""},
            {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng," +
                       "*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"},
            {"Accept-Encoding", "gzip, deflate, br"},
            {"Accept-Language", "en-US,en;q=0.9"},
        };
        return headers;
    }

    public static Dictionary<string, string> GetRealHeaders()
    {
	    var headers = new Dictionary<string, string>()
	    {
		    {"User-Agent", "MangaMagnet v0.0.1 (https://github.com/SimplyMedia/MangaMagnet)"},
		    {"Accept-Encoding", "gzip, deflate, br"},
	    };

	    return headers;
    }
}
