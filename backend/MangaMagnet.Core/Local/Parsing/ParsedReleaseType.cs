namespace MangaMagnet.Core.Local.Parsing;

/// <summary>
/// Represents the result of parsing a manga file name.
/// </summary>
public enum ParsedReleaseType
{
	/// <summary>
	/// The release is a volume.
	/// </summary>
	VOLUME,

	/// <summary>
	/// The release is a chapter.
	/// </summary>
	CHAPTER,

	/// <summary>
	/// The release is neither a Volume nor a Chapter. Possibly a one-shot.
	/// </summary>
	NONE
}
