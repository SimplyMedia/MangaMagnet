using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Manga;

public record MangaDexRelationshipAttribute(
	[property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("volume")] string Volume,
    [property: JsonPropertyName("fileName")] string FileName,
    [property: JsonPropertyName("locale")] string Locale,
    [property: JsonPropertyName("description")] string Description
);
