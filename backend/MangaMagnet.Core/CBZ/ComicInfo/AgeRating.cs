using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MangaMagnet.Core.CBZ.ComicInfo;

/// <summary>
/// Age rating of the comic.
/// </summary>
public enum AgeRating
{
	[Display(Name = "Unknown")]
	UNKNOWN,

	[Display(Name = "Adults Only 18+")]
	ADULT_ONLY18_PLUS,

	[Display(Name = "Early Childhood")]
	EARLY_CHILDHOOD,

	[Display(Name = "Everyone")]
	EVERYONE,

	[Display(Name = "Everyone 10+")]
	EVERYONE10_PLUS,

	[Display(Name = "G")]
	G,

	[Display(Name = "Kids to Adults")]
	KIDS_TO_ADULTS,

	[Display(Name = "M")]
	M,

	[Display(Name = "M15+")]
	MA15_PLUS,

	[Display(Name = "Mature 17+")]
	MATURE17_PLUS,

	[Display(Name = "PG")]
	PG,

	[Display(Name = "R18+")]
	R_18_PLUS,

	[Display(Name = "Rating Pending")]
	RATING_PENDING,

	[Display(Name = "Teen")]
	TEEN,

	[Display(Name = "X18+")]
	X18_PLUS
}
