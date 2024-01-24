namespace MangaMagnet.Api.Models.Response;

public record VolumeResponse(
    Guid Id,
    int VolumeNumber,
    string Path,
    long SizeInBytes,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);