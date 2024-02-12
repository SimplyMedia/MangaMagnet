namespace MangaMagnet.Core.Providers.MangaDex.Models.Download;

public record MangaDexDownloadMetadata(string ScanlationGroup, string? ChapterTitle, DateTimeOffset? UploadedAt, int? Volume);
