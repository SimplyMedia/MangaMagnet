using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models;

public record MangaDexResponse<T>(
        [property: JsonPropertyName("result")] string Result,
        [property: JsonPropertyName("response")] string? Response,
        [property: JsonPropertyName("data")] T Data
    );
