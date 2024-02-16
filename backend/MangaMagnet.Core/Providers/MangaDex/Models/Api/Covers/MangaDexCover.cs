using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Covers;

public record MangaDexCover(
	[property: JsonPropertyName("id")] string Id,
	[property: JsonPropertyName("type")] string Type,
	[property: JsonPropertyName("attributes")] MangaDexCoverAttributeResponse Attributes,
	[property: JsonPropertyName("relationships")] IReadOnlyList<MangaDexCoverRelationship> Relationships
);
