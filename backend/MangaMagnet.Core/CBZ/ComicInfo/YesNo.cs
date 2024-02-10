using System.ComponentModel.DataAnnotations;

namespace MangaMagnet.Core.CBZ.ComicInfo;

public enum YesNo
{
	[Display(Name = "Unknown")]
	UNKNOWN,

	[Display(Name = "Yes")]
	YES,

	[Display(Name = "No")]
	NO
}
