﻿using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Chapter;

public record MangaDexChapterResponse<T>(
	[property: JsonPropertyName("result")] string Result,
	[property: JsonPropertyName("response")] string? Response,
	[property: JsonPropertyName("data")] T Data,
	[property: JsonPropertyName("limit")] int Limit,
	[property: JsonPropertyName("offset")] int Offset,
	[property: JsonPropertyName("total")] int Total
	) : MangaDexResponse<T>(Result, Response, Data);