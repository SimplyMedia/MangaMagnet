using MangaMagnet.Core.Database;

namespace MangaMagnet.Core.Metadata;

public record MangaSearchMetadataResult(
    string MangaDexId,
    string? AnilistId,
    string? MyAnimeListId,
    string? MangaUpdatesId,
    string Title,
    string Description,
    string? CoverUrl,
    MangaStatus? Status,
    int? Year
    );