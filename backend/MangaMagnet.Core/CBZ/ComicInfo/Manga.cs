using System.ComponentModel.DataAnnotations;

namespace MangaMagnet.Core.CBZ.ComicInfo;

public enum Manga
{
	[Display(Name = "Unknown")]
	UNKNOWN,

	[Display(Name = "No")]
	NO,

	[Display(Name = "Yes")]
	YES,

	[Display(Name = "YesAndRightToLeft")]
	YES_AND_LEFT_TO_RIGHT
}
