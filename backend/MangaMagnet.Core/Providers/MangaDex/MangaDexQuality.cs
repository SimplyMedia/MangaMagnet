using System.ComponentModel.DataAnnotations;

namespace MangaMagnet.Core.Providers.MangaDex;

/// <summary>
///		Different qualities of the manga pages.
/// </summary>
public enum MangaDexQuality
{
	/// <summary>
	///		Original quality - pixel-for-pixel accurate to how the image was originally sent to us
	/// </summary>
	[Display(Name = "data")]
	ORIGINAL,

	/// <summary>
	///		Compressed quality - Large size savings at the expense of image quality
	/// </summary>
	[Display(Name = "data-saver")]
	COMPRESSED,
}
