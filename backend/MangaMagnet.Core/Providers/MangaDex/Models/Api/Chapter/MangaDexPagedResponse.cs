using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Chapter;

public record MangaDexPagedResponse<T>(
	string Result,
	string? Response,
	T Data,
	[property: JsonPropertyName("limit")] int Limit,
	[property: JsonPropertyName("offset")] int Offset,
	[property: JsonPropertyName("total")] int Total
	) : MangaDexResponse<T>(Result, Response, Data);
