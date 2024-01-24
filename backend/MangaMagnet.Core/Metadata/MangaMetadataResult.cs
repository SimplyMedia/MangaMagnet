using MangaMagnet.Core.Database;

namespace MangaMagnet.Core.Metadata;

public record MangaMetadataResult(
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
    long? MyAnimeListId
    );
