namespace MangaMagnet.Api.Models.Response;

public record ChapterMetadataResponse(
    Guid Id,
    string? Title,
    double ChapterNumber,
    int? VolumeNumber,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);
