using System.ComponentModel.DataAnnotations;

namespace MangaMagnet.Core.CBZ.ComicInfo;

public enum ComicPageType
{
	[Display(Name = "FrontCover")]
	FRONT_COVER,

	[Display(Name = "InnerCover")]
	INNER_COVER,

	[Display(Name = "Roundup")]
	ROUNDUP,

	[Display(Name = "Story")]
	STORY,

	[Display(Name = "Advertisement")]
	ADVERTISEMENT,

	[Display(Name = "Editorial")]
	EDITORIAL,

	[Display(Name = "Letters")]
	LETTERS,

	[Display(Name = "Preview")]
	PREWIEW,

	[Display(Name = "BackCover")]
	BACK_COVER,

	[Display(Name = "Other")]
	OTHER,

	[Display(Name = "Deleted")]
	DELETED
}
