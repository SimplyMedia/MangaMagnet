namespace MangaMagnet.Core.CBZ.ComicInfo;

public record ComicInfoChapterMetadata(string ScanlationGroup, string? ChapterTitle, double ChapterNumber, int? VolumeNumber, DateTimeOffset? UploadedAt);
