using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Covers;

public record MangaDexCoverAttributeResponse(
	[property: JsonPropertyName("description")] string Description,
	[property: JsonPropertyName("volume")] string Volume,
	[property: JsonPropertyName("fileName")] string FileName,
	[property: JsonPropertyName("locale")] string Locale,
	[property: JsonPropertyName("createdAt")] DateTime? CreatedAt,
	[property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt,
	[property: JsonPropertyName("version")] int? Version
);
