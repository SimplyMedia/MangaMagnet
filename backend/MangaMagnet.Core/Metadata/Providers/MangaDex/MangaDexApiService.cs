using System.Collections.Specialized;
using System.Net.Http.Json;
using MangaMagnet.Core.Metadata.Providers.MangaDex.Models;
using MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Chapter;
using MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Manga;
using MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Statistics;
using MangaMagnet.Core.Util;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Metadata.Providers.MangaDex;

public class MangaDexApiService(HttpClient httpClient, ILogger<MangaDexApiService> logger) : SimpleRatelimitedProvider
{
    public async Task<List<MangaDexMangaData>> SearchMangaByNameAsync(string input, CancellationToken cancellationToken = default)
    {
        var queryParams = new NameValueCollection
        {
            { "title", input },
            { "limit", "25" },
            { "offset", "0" },
            { "contentRating[]", "safe" },
            { "contentRating[]", "suggestive" },
            { "contentRating[]", "erotica" },
            { "includes[]", "cover_art" },
            { "order[relevance]", "desc" }
        };

        var requestUri = UriUtils.BuildUrlWithQueryString($"{MangaDexConstants.ApiBaseUrl}/{MangaDexConstants.MangaEndpoint}", queryParams);

        logger.LogDebug("Request Uri: {Request}", requestUri);

        var req = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var responseMessage = await SendRequestAsync(req, cancellationToken);

        var response = await responseMessage.Content.ReadFromJsonAsync<MangaDexResponse<List<MangaDexMangaData>>>(cancellationToken: cancellationToken) ??
                   throw new Exception("Failed to deserialize response");

        return response.Data;
    }

    public async Task<MangaDexResponse<MangaDexMangaData>> FetchMangaMetadataByIdAsync(string mangaDexId, CancellationToken cancellationToken = default)
    {
        var queryParams = new NameValueCollection
        {
            { "includes[]", "artist" },
            { "includes[]", "author" },
            { "includes[]", "cover_art" },
        };

        var requestUri = UriUtils.BuildUrlWithQueryString($"{MangaDexConstants.ApiBaseUrl}/{MangaDexConstants.MangaEndpoint}/{mangaDexId}", queryParams);

        logger.LogDebug("Request Uri: {Request}", requestUri);

        var req = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var responseMessage = await SendRequestAsync(req, cancellationToken);

        return await responseMessage.Content.ReadFromJsonAsync<MangaDexResponse<MangaDexMangaData>>(cancellationToken: cancellationToken) ??
               throw new Exception("Failed to deserialize response");
    }

    public async Task<MangaDexStatisticResponse> FetchMangaStatistics(string mangaDexId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{MangaDexConstants.ApiBaseUrl}/{MangaDexConstants.StatisticsEndpoint}/{MangaDexConstants.MangaEndpoint}/{mangaDexId}";

        logger.LogDebug("Request Uri: {Request}", requestUri);

        var req = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var responseMessage = await SendRequestAsync(req, cancellationToken);

        return await responseMessage.Content.ReadFromJsonAsync<MangaDexStatisticResponse>(cancellationToken: cancellationToken) ??
                       throw new Exception("Failed to deserialize response");
    }

    public async Task<List<MangaDexChapter>> FetchMangaChapters(string mangaDexId, CancellationToken cancellationToken = default)
    {
	    var doNextRequest = true;
	    var offset = 0;
		var chapters = new List<MangaDexChapter>();

	    while (doNextRequest)
	    {
		    const int limit = 500;

		    var queryParams = new NameValueCollection
		    {
			    { "limit", limit.ToString() },
			    { "offset", offset.ToString() },
			    { "includes[]", "scanlation_group" },
			    { "includes[]", "user" },
			    { "order[volume]", "desc" },
			    { "order[chapter]", "desc" },
			    { "contentRating[]", "safe" },
			    { "contentRating[]", "suggestive" },
			    { "contentRating[]", "erotica" },
			    { "contentRating[]", "pornographic" }
		    };

		    var requestUri = UriUtils.BuildUrlWithQueryString($"{MangaDexConstants.ApiBaseUrl}/{MangaDexConstants.MangaEndpoint}/{mangaDexId}/{MangaDexConstants.ChapterEndpoint}", queryParams);

		    logger.LogDebug("Request Uri: {Request}", requestUri);

		    var req = new HttpRequestMessage(HttpMethod.Get, requestUri);

		    var responseMessage = await SendRequestAsync(req, cancellationToken);

		    var response = await responseMessage.Content.ReadFromJsonAsync<MangaDexChapterResponse<List<MangaDexChapter>>>(cancellationToken: cancellationToken) ??
		                   throw new Exception("Failed to deserialize response");

		    var responseDataCount = response.Data.Count;

		    chapters.AddRange(response.Data);

		    offset += responseDataCount;
		    doNextRequest = response.Total > offset;
	    }

	    return chapters;
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        foreach (var (key, value) in HttpHeaderUtil.GetFakeHeaders())
            request.Headers.Add(key, value);

        await RateLimiter.AcquireAsync(cancellationToken: cancellationToken);

        var response = await httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");

        return response;
    }
}
