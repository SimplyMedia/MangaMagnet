using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Statistics;

public record MangaDexStatistics(
    [property: JsonPropertyName("comments")] MangaDexCommentsStatistics Comments,
    [property: JsonPropertyName("rating")] MangaDexRating Rating,
    [property: JsonPropertyName("follows")] int Follows
);