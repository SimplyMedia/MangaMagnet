using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Manga;

public record MangaDexResponse<T>(
        [property: JsonPropertyName("result")] string Result,
        [property: JsonPropertyName("response")] string? Response,
        [property: JsonPropertyName("data")] T Data
    );
