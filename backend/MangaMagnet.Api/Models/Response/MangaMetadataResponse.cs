using MangaMagnet.Core.Database;

namespace MangaMagnet.Api.Models.Response;

public record MangaMetadataResponse(
    Guid Id,
    string DisplayTitle,
    List<string> Aliases,
    MangaStatus Status,
    int? Year,
    string Author,
    string Artist,
    string Description,
    List<string> Genres,
    List<string> Tags,
    double UserScore,
    string? CoverImageUrl,
    long? AnilistId,
    string MangaDexId,
    string? MangaUpdatesId,
    long? MyAnimeListId,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
