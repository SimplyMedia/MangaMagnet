using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Covers;

public record MangaDexCoverRelationship(
	[property: JsonPropertyName("id")] string Id,
	[property: JsonPropertyName("type")] string Type
);
