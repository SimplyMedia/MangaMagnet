namespace MangaMagnet.Core.Metadata;

public record ChapterMetadataResult(
	double ChapterNumber,
    int? VolumeNumber,
	string? Title
    );
