using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MangaMagnet.Core.Providers.MangaDex.Models;
using MangaMagnet.Core.Providers.MangaDex.Models.Api;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Chapter;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.ChapterPages;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Manga;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Report;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Statistics;
using MangaMagnet.Core.Providers.MangaDex.Models.Download;
using MangaMagnet.Core.Util;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Providers.MangaDex;

public class MangaDexApiService(IHttpClientFactory httpClientFactory, ILogger<MangaDexApiService> logger)
	: SimpleRatelimitedProvider
{
	public async Task<List<MangaDexMangaData>> SearchMangaByNameAsync(string input,
		CancellationToken cancellationToken = default)
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

		var requestUri = UriUtils.BuildUrlWithQueryString(MangaDexConstants.SearchMangaByNameUrl, queryParams);

		var response =
			await SendAndParseGetRequestAsync<MangaDexResponse<List<MangaDexMangaData>>>(requestUri, false,
				cancellationToken);

		return response.Data;
	}

	public async Task<MangaDexResponse<MangaDexMangaData>> FetchMangaMetadataByIdAsync(string mangaDexId,
		CancellationToken cancellationToken = default)
	{
		var queryParams = new NameValueCollection
		{
			{ "includes[]", "artist" },
			{ "includes[]", "author" },
			{ "includes[]", "cover_art" },
		};

		var requestUri =
			UriUtils.BuildUrlWithQueryString(MangaDexConstants.FetchMangaMetadataByIdUrl(mangaDexId), queryParams);

		return await SendAndParseGetRequestAsync<MangaDexResponse<MangaDexMangaData>>(requestUri, false,
			cancellationToken);
	}

	public Task<MangaDexStatisticResponse> FetchMangaStatistics(string mangaDexId, CancellationToken cancellationToken = default)
		=> SendAndParseGetRequestAsync<MangaDexStatisticResponse>(MangaDexConstants.FetchMangaStatisticsUrl(mangaDexId),
			false, cancellationToken);

	public async Task<List<MangaDexChapter>> FetchMangaChapters(string mangaDexId,
		CancellationToken cancellationToken = default)
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

			var requestUri =
				UriUtils.BuildUrlWithQueryString(MangaDexConstants.FetchMangaChapterUrl(mangaDexId), queryParams);

			var response =
				await SendAndParseGetRequestAsync<MangaDexChapterResponse<List<MangaDexChapter>>>(requestUri, false,
					cancellationToken);

			var responseDataCount = response.Data.Count;

			chapters.AddRange(response.Data);

			offset += responseDataCount;
			doNextRequest = response.Total > offset;
		}

		return chapters;
	}

	public async Task<List<string>> DownloadMangaChapterPagesAsync(string downloadPath, string chapterId, MangaDexQuality quality,
		CancellationToken cancellationToken = default)
	{
		var (_, baseUrl, chapter) = await SendAndParseGetRequestAsync<MangaDexChapterPagesResponse>(
			MangaDexConstants.FetchMangaChapterPagesUrl(chapterId), false,
			cancellationToken);

		var pageHashes = quality == MangaDexQuality.ORIGINAL
			? chapter.Data
			: chapter.DataSaver;

		const int chunkSize = 20;

		var paths = new List<string>();

		foreach (var chunkedPageHashes in pageHashes.Chunk(chunkSize))
		{
			var tasks = chunkedPageHashes.Select(pageName
				=> DownloadAndWritePageToDiskAsync(baseUrl, quality, pageName, chapter.Hash, downloadPath, cancellationToken)).ToList();

			paths.AddRange(await Task.WhenAll(tasks));

			logger.LogInformation("Downloaded {Count} pages", tasks.Count);
		}

		return paths;
	}

	private async Task<string> DownloadAndWritePageToDiskAsync(string baseUrl, MangaDexQuality quality, string fileName,
		string chapterHash, string basePath, CancellationToken cancellationToken = default)
	{
		var pageNumber = fileName.Split("-").First();
		var fileExtension = fileName.Split(".").Last();

		var imageStream = new MemoryStream();
		await DownloadMangaChapterPageAsync(baseUrl, quality, fileName, chapterHash, imageStream, cancellationToken);

		var imagePath = $"{basePath}/{pageNumber}.{fileExtension}";

		await using var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

		await imageStream.CopyToAsync(fileStream, cancellationToken);
		await fileStream.FlushAsync(cancellationToken);

		logger.LogDebug("Wrote page {Page} to disk", pageNumber);

		return imagePath;
	}

	private async Task DownloadMangaChapterPageAsync(string baseUrl, MangaDexQuality quality, string fileName,
		string chapterHash, Stream stream, CancellationToken cancellationToken = default)
	{
		var requestUri = MangaDexConstants.DownloadMangaChapterPageUrl(baseUrl, quality, chapterHash, fileName);

		using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

		var success = false;
		var cached = false;
		var bytes = 0;
		var duration = 0;

		// TODO: add retry logic

		try
		{
			var stopwatch = Stopwatch.StartNew();

			var response = await SendRequestAsync(request, true, cancellationToken);
			success = true;

			response.Headers.TryGetValues("X-Cache", out var cacheHeader);

			cached = cacheHeader?.FirstOrDefault()?.StartsWith("HIT") ?? false;

			await using var content = await response.Content.ReadAsStreamAsync(cancellationToken);

			await content.CopyToAsync(stream, cancellationToken);

			await stream.FlushAsync(cancellationToken);

			stream.Position = 0;

			duration = (int)stopwatch.ElapsedMilliseconds;

			bytes = (int)stream.Length;

			logger.LogDebug("Downloaded page {Page} in {Duration}ms", requestUri, duration);
		}
		catch (Exception e)
		{
			logger.LogError(e, "Failed to download page {Page}", requestUri);
		}

		var reportBody = new MangaDexReportRequest(requestUri, success, cached, bytes, duration);

		if (!baseUrl.Contains("mangadex.org"))
			await SendImageDownloadReportToMangaDex(reportBody, cancellationToken);
	}

	private async Task SendImageDownloadReportToMangaDex(MangaDexReportRequest requestBody,
		CancellationToken cancellationToken = default)
	{
		using var request = new HttpRequestMessage(HttpMethod.Post, MangaDexConstants.ReportMangaUrl);

		request.Content = JsonContent.Create(requestBody, new MediaTypeHeaderValue("application/json"));

		await SendRequestAsync(request, true, cancellationToken);
	}

	private async Task<T> SendAndParseGetRequestAsync<T>(string requestUri, bool useRealHeaders = false,
		CancellationToken cancellationToken = default)
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

		return await SendAndParseRequestAsync<T>(request, useRealHeaders, cancellationToken);
	}

	private async Task<T> SendAndParseRequestAsync<T>(HttpRequestMessage request, bool
			useRealHeaders = false,
		CancellationToken cancellationToken = default)
	{
		using var responseMessage = await SendRequestAsync(request, useRealHeaders, cancellationToken);

		return await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken) ??
		       throw new Exception("Failed to deserialize response");
	}

	private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, bool useRealHeaders = false,
		CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Request Uri: {Request}", request.RequestUri);

		var headers = useRealHeaders ? MangaDexHeaderUtil.GetRealHeaders() : MangaDexHeaderUtil.GetFakeHeaders();

		foreach (var (key, value) in headers)
			request.Headers.Add(key, value);

		await RateLimiter.AcquireAsync(cancellationToken: cancellationToken);

		using var httpClient = httpClientFactory.CreateClient("MangaDex");
		var response = await httpClient.SendAsync(request, cancellationToken);

		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync(cancellationToken)}");

		return response;
	}
}
