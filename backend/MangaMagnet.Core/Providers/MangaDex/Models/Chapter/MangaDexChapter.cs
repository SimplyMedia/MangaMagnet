using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Chapter;

public record MangaDexChapter(
	[property: JsonPropertyName("id")] string Id,
	[property: JsonPropertyName("type")] string Type,
	[property: JsonPropertyName("attributes")] MangaDexChapterAttributes Attributes,
	[property: JsonPropertyName("relationships")] IReadOnlyList<MangaDexRelationship> Relationships
);
