namespace MangaMagnet.Core.Providers.MangaDex;

public static class MangaDexHeaderUtil
{
    public static Dictionary<string, string> GetHeaders()
    {
	    var headers = new Dictionary<string, string>()
	    {
		    {"User-Agent", "MangaMagnet v0.0.1 (https://github.com/SimplyMedia/MangaMagnet)"},
		    {"Accept-Encoding", "gzip, deflate, br"},
	    };

	    return headers;
    }
}
