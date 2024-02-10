using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Statistics;

public record MangaDexCommentsStatistics(
    [property: JsonPropertyName("threadId")] int ThreadId,
    [property: JsonPropertyName("repliesCount")] int RepliesCount
);