namespace MangaMagnet.Core.CBZ.ComicInfo;

/// <summary>
///    Different versions of the ComicInfo.xml format.
/// </summary>
public enum ComicInfoVersion
{
	/// <summary>
	///   Version 1 of the ComicInfo.xml format. <see href="https://anansi-project.github.io/docs/comicinfo/schemas/v1.0"/>
	/// </summary>
	V1,

	/// <summary>
	/// Version 2 of the ComicInfo.xml format. <see href="https://anansi-project.github.io/docs/comicinfo/schemas/v2.0"/>
	/// </summary>
	V2,

	/// <summary>
	/// (Draft) Version 2.1 of the ComicInfo.xml format. <see href="https://anansi-project.github.io/docs/comicinfo/schemas/v2.1"/>
	/// </summary>
	V2_1,
}
