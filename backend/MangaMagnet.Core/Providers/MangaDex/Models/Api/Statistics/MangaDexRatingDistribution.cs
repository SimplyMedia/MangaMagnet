using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Statistics;

public record MangaDexRatingDistribution(
    [property: JsonPropertyName("1")] int _1,
    [property: JsonPropertyName("2")] int _2,
    [property: JsonPropertyName("3")] int _3,
    [property: JsonPropertyName("4")] int _4,
    [property: JsonPropertyName("5")] int _5,
    [property: JsonPropertyName("6")] int _6,
    [property: JsonPropertyName("7")] int _7,
    [property: JsonPropertyName("8")] int _8,
    [property: JsonPropertyName("9")] int _9,
    [property: JsonPropertyName("10")] int _10
);