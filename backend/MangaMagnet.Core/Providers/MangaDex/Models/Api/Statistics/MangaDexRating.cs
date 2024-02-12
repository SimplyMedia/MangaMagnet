using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Statistics;

public record MangaDexRating(
    [property: JsonPropertyName("average")] double? Average,
    [property: JsonPropertyName("bayesian")] double? Bayesian,
    [property: JsonPropertyName("distribution")] MangaDexRatingDistribution Distribution
);
