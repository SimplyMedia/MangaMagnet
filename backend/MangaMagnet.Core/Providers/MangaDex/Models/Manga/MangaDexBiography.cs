using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Manga;

public record MangaDexBiography(
    [property: JsonPropertyName("en")] string En
);