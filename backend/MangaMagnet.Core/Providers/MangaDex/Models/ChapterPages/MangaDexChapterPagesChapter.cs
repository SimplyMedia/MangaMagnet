using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.ChapterPages;

public record MangaDexChapterPagesChapter(
	[property: JsonPropertyName("hash")] string Hash,
	[property: JsonPropertyName("data")] IReadOnlyList<string> Data,
	[property: JsonPropertyName("dataSaver")] IReadOnlyList<string> DataSaver
);
