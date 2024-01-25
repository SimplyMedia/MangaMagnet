using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Chapter;

public record MangaDexRelationship(
	[property: JsonPropertyName("id")] string Id,
	[property: JsonPropertyName("type")] string Type,
	[property: JsonPropertyName("attributes")] MangaDexChapterAttributes Attributes
);
