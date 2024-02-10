using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.ChapterPages;

public record MangaDexChapterPagesResponse(
	[property: JsonPropertyName("result")] string Result,
	[property: JsonPropertyName("baseUrl")] string BaseUrl,
	[property: JsonPropertyName("chapter")] MangaDexChapterPagesChapter Chapter);
