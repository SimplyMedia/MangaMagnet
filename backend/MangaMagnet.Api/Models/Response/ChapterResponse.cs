namespace MangaMagnet.Api.Models.Response;

public record ChapterResponse(
    Guid Id,
    double ChapterNumber,
    string Path,
    long SizeInBytes,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
