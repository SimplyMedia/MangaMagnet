using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Manga;

public record MangaDexLinks(
    [property: JsonPropertyName("al")] string Anilist,
    [property: JsonPropertyName("ap")] string Ap,
    [property: JsonPropertyName("bw")] string Bw,
    [property: JsonPropertyName("kt")] string Kt,
    [property: JsonPropertyName("mu")] string MangaUpdates,
    [property: JsonPropertyName("amz")] string Amz,
    [property: JsonPropertyName("cdj")] string Cdj,
    [property: JsonPropertyName("ebj")] string Ebj,
    [property: JsonPropertyName("mal")] string MyAnimeList,
    [property: JsonPropertyName("raw")] string Raw,
    [property: JsonPropertyName("engtl")] string Engtl
);