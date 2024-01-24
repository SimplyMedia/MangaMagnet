using MangaMagnet.Core.Database;

namespace MangaMagnet.Api.Models.Response;

public record MangaSearchResponse(
    string MangaDexId,
    string? AnilistId,
    string? MyAnimeListId,
    string? MangaUpdatesId,
    string Title,
    string Description,
    string? CoverUrl,
    MangaStatus? Status,
    int? Year);