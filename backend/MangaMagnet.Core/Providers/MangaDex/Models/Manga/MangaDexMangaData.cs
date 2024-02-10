using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Manga;

public record MangaDexMangaData(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("attributes")] MangaDexAttributes Attributes,
    [property: JsonPropertyName("relationships")] IReadOnlyList<MangaDexRelationship> Relationships
);
