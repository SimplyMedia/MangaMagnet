using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Manga;

public record MangaDexTag(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("attributes")] MangaDexTagAttributes Attributes,
    [property: JsonPropertyName("relationships")] IReadOnlyList<object> Relationships
);
