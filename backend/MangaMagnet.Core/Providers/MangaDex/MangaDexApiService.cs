﻿using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MangaMagnet.Core.Progress.Models;
using MangaMagnet.Core.Providers.MangaDex.Models;
using MangaMagnet.Core.Providers.MangaDex.Models.Api;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Chapter;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.ChapterPages;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Covers;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Manga;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Report;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Statistics;
using MangaMagnet.Core.Providers.MangaDex.Models.Download;
using MangaMagnet.Core.Util;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Polly.Retry;

namespace MangaMagnet.Core.Providers.MangaDex;

public class MangaDexApiService(IHttpClientFactory httpClientFactory, ILogger<MangaDexApiService> logger, ResiliencePipelineProvider<string> resiliencePipelineProvider)
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

	public Task<MangaDexStatisticResponse> FetchMangaStatisticsAsync(string mangaDexId, CancellationToken cancellationToken = default)
		=> SendAndParseGetRequestAsync<MangaDexStatisticResponse>(MangaDexConstants.FetchMangaStatisticsUrl(mangaDexId),
			false, cancellationToken);

	public Task<List<MangaDexCover>> FetchMangaCoversAsync(string mangaDexId,
		CancellationToken cancellationToken = default)
		=> FetchAllPagesAsync(mangaDexId, FetchMangaCoverAsync, cancellationToken);

	public Task<List<MangaDexChapter>> FetchAllMangaChaptersAsync(string mangaDexId,
		CancellationToken cancellationToken = default)
		=> FetchAllPagesAsync(mangaDexId, FetchMangaChapterPageAsync, cancellationToken);

	public async Task<List<MangaDexChapter>> FetchLatestMangaChaptersAsync(string mangaDexId,
		CancellationToken cancellationToken = default)
	{
		var response = await FetchMangaChapterPageAsync(mangaDexId, 0, 50, cancellationToken);

		return response.Data;
	}

	public async Task<List<string>> DownloadMangaChapterPagesAsync(string downloadPath, string chapterId, MangaDexQuality quality, ProgressTask task,
		CancellationToken cancellationToken = default)
	{
		var (_, baseUrl, chapter) = await SendAndParseGetRequestAsync<MangaDexChapterPagesResponse>(
			MangaDexConstants.FetchMangaChapterPagesUrl(chapterId), false,
			cancellationToken);

		var pageHashes = quality == MangaDexQuality.ORIGINAL
			? chapter.Data
			: chapter.DataSaver;

		task.Total = pageHashes.Count;

		const int chunkSize = 20;

		var paths = new List<string>();

		foreach (var chunkedPageHashes in pageHashes.Chunk(chunkSize))
		{
			var tasks = chunkedPageHashes.Select(pageName
				=> DownloadAndWritePageToDiskAsync(baseUrl, quality, pageName, chapter.Hash, downloadPath, cancellationToken)).ToList();

			paths.AddRange(await Task.WhenAll(tasks));

			for (var i = 0; i < tasks.Count; i++)
				task.Increment();

			logger.LogDebug("Downloaded {Count} pages", tasks.Count);
		}

		return paths;
	}

	private Task<MangaDexPagedResponse<List<MangaDexCover>>> FetchMangaCoverAsync(string mangaDexId, int offset, int limit, CancellationToken cancellationToken = default)
	{
		var queryParams = new NameValueCollection
		{
			{ "order[volume]", "asc" },
			{ "manga[]", mangaDexId },
			{ "limit", limit.ToString()},
			{ "offset", offset.ToString() },
		};

		var requestUri = UriUtils.BuildUrlWithQueryString(MangaDexConstants.FetchMangaCoversUrl, queryParams);

		return SendAndParseGetRequestAsync<MangaDexPagedResponse<List<MangaDexCover>>>(requestUri, false, cancellationToken);
	}

	private async Task<List<T>> FetchAllPagesAsync<T>(string mangaDexId, Func<string, int, int, CancellationToken, Task<MangaDexPagedResponse<List<T>>>> func, CancellationToken cancellationToken = default)
	{
		var doNextRequest = true;
		var offset = 0;
		var inner = new List<T>();

		while (doNextRequest)
		{
			const int limit = 100;

			var pipeline = resiliencePipelineProvider.GetPipeline("MangaDex-Pipeline");

			await pipeline.ExecuteAsync( async cancelToken =>
			{
				try
				{
					var response = await func(mangaDexId, offset, limit, cancelToken);

					var responseDataCount = response.Data.Count;

					inner.AddRange(response.Data);

					offset += responseDataCount;
					doNextRequest = response.Total > offset;
				}
				catch (Exception e)
				{
					var page = offset / limit;

					logger.LogWarning("Failed to fetch Page {Page}: {Exception}", page, e.Message);
					throw;
				}
			}, cancellationToken);
		}

		return inner;
	}

	private Task<MangaDexPagedResponse<List<MangaDexChapter>>> FetchMangaChapterPageAsync(string mangaDexId,
		int offset,
		int limit,
		CancellationToken cancellationToken = default)
	{
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

		return SendAndParseGetRequestAsync<MangaDexPagedResponse<List<MangaDexChapter>>>(requestUri, false,
			cancellationToken);
	}

	private async Task<string> DownloadAndWritePageToDiskAsync(string baseUrl, MangaDexQuality quality, string fileName,
		string chapterHash, string basePath, CancellationToken cancellationToken = default)
	{
		var pageNumberMatch = MangaDexRegex.MangaDexPageNumberRegex().Match(fileName);

		var pageNumber = pageNumberMatch.Groups["pageNumber"].Value;

		var fileExtension = fileName.Split(".").Last();

		var imageStream = new MemoryStream();
		await DownloadMangaChapterPageAsync(baseUrl, quality, fileName, chapterHash, imageStream, cancellationToken);

		var imagePath = Path.Combine(basePath, $"{pageNumber}.{fileExtension}");

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
			await SendImageDownloadReportToMangaDexAsync(reportBody, cancellationToken);
	}

	private async Task SendImageDownloadReportToMangaDexAsync(MangaDexReportRequest requestBody,
		CancellationToken cancellationToken = default)
	{
		using var request = new HttpRequestMessage(HttpMethod.Post, MangaDexConstants.ReportMangaUrl);

		request.Content = JsonContent.Create(requestBody, new MediaTypeHeaderValue("application/json"));

		await SendRequestAsync(request, true, cancellationToken);

		logger.LogDebug("Sent image download report to MangaDex");
	}

	private async Task<T> SendAndParseGetRequestAsync<T>(string requestUri, bool ignoreRateLimit, CancellationToken cancellationToken = default)
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

		return await SendAndParseRequestAsync<T>(request, ignoreRateLimit, cancellationToken);
	}

	private async Task<T> SendAndParseRequestAsync<T>(HttpRequestMessage request, bool ignoreRateLimit, CancellationToken cancellationToken = default)
	{
		using var responseMessage = await SendRequestAsync(request, ignoreRateLimit, cancellationToken);

		return await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken) ??
		       throw new Exception("Failed to deserialize response");
	}

	private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, bool ignoreRateLimit = false,
		CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Request Uri: {Request}", request.RequestUri);

		foreach (var (key, value) in MangaDexHeaderUtil.GetHeaders())
			request.Headers.Add(key, value);

		if (!ignoreRateLimit)
		{
			using var lease = await RateLimiter.AcquireAsync(cancellationToken: cancellationToken);

			if (lease.IsAcquired != true)
				throw new Exception("Failed to acquire rate limit lease");
		}

		using var httpClient = httpClientFactory.CreateClient("MangaDex");
		var response = await httpClient.SendAsync(request, cancellationToken);

		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync(cancellationToken)}");

		return response;
	}
}
