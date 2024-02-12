using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Report;

public record MangaDexReportRequest(
	[property: JsonPropertyName("url")] string Url,
	[property: JsonPropertyName("success")] bool Success,
	[property: JsonPropertyName("cached")] bool Cached,
	[property: JsonPropertyName("bytes")] int Bytes,
	[property: JsonPropertyName("duration")] int Duration);
