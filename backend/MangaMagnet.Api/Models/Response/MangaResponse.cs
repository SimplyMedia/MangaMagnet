using System.ComponentModel.DataAnnotations;

namespace MangaMagnet.Api.Models.Response;

public record MangaResponse(
    Guid Id,
    string Path,
    MangaMetadataResponse Metadata,
    List<ChapterMetadataResponse> ChapterMetadata,
    List<ChapterResponse> Chapters,
    List<VolumeResponse> Volumes,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
