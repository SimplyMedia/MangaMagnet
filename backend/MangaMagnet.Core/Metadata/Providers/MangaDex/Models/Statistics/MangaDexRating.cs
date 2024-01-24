using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Statistics;

public record MangaDexRating(
    [property: JsonPropertyName("average")] double Average,
    [property: JsonPropertyName("bayesian")] double Bayesian,
    [property: JsonPropertyName("distribution")] MangaDexRatingDistribution Distribution
);