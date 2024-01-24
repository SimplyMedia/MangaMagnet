using System.Collections.Specialized;

namespace MangaMagnet.Core.Util;

public static class UriUtils
{
    public static string BuildUrlWithQueryString(string baseUri, NameValueCollection nvc)
    {
        var queryParams = from key in nvc.AllKeys from value in nvc.GetValues(key) select $"{key}={Uri.EscapeDataString(value)}";

        var uriBuilder = new UriBuilder(baseUri)
        {
            Query = string.Join('&', queryParams)
        };

        return uriBuilder.Uri.AbsoluteUri;
    }
}