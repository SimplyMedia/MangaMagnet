using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Manga;

public record MangaDexTagDescription(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("attributes")]
    MangaDexTagAttributes Attributes
);