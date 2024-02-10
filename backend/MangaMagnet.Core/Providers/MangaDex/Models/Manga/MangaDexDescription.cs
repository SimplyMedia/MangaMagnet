using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Manga;

public record MangaDexDescription(
    [property: JsonPropertyName("en")] string En,
    [property: JsonPropertyName("fr")] string Fr,
    [property: JsonPropertyName("ru")] string Ru,
    [property: JsonPropertyName("tr")] string Tr,
    [property: JsonPropertyName("uk")] string Uk,
    [property: JsonPropertyName("pt-br")] string PtBr
);