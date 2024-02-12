using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Statistics;

public record MangaDexStatisticResponse(
    [property: JsonPropertyName("result")] string Result,
    [property: JsonPropertyName("statistics")] Dictionary<string, MangaDexStatistics> Statistics);

