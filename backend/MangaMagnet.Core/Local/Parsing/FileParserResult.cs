namespace MangaMagnet.Core.Local.Parsing;

public record FileParserResult(
	string Title,
	ParsedReleaseType ParsedType,
	int? Year,
	int? VolumeNumber,
	double? ChapterNumber,
	string? ReleaseGroup,
	bool IsDigital = false,
	bool IsFixed = false,
	int? FixedNumber = null
);
