using MangaMagnet.Core.CBZ.ComicInfo.XML;
using MangaMagnet.Core.Util;

namespace MangaMagnet.Core.CBZ.ComicInfo;

public class ComicInfo : IVersioned<XmlComicInfo>
{
	public string? Title { get; set; }

	public string? Series { get; set; }

	public string? Number { get; set; }

	public int? Count { get; set; }

	public int? Volume { get; set; }

	public string? AlternateSeries { get; set; }

	public string? AlternateNumber { get; set; }

	public int? AlternateCount { get; set; }

	public string? Summary { get; set; }

	public string? Notes { get; set; }

	public int? Year { get; set; }

	public int? Month { get; set; }

	public int? Day { get; set; }

	public string? Writer { get; set; }

	public string? Penciller { get; set; }

	public string? Inker { get; set; }

	public string? Colorist { get; set; }

	public string? Letterer { get; set; }

	public string? CoverArtist { get; set; }

	public string? Editor { get; set; }

	public string? Publisher { get; set; }

	public string? Imprint { get; set; }

	public string? Genre { get; set; }

	public string? Web { get; set; }

	public int? PageCount { get; set; }

	public string? LanguageISO { get; set; }

	public string? Format { get; set; }

	public YesNo? BlackAndWhite { get; set; }

	public Manga? Manga { get; set; }

	public string? Characters { get; set; }

	public string? Teams { get; set; }

	public string? Locations { get; set; }

	public string? ScanInformation { get; set; }

	public string? StoryArc { get; set; }

	public string? SeriesGroup { get; set; }

	public AgeRating? AgeRating { get; set; }

	public List<PageInfo>? Pages { get; set; }

	public double? CommunityRating { get; set; }

	public string? MainCharacterOrTeam { get; set; }

	public string? Review { get; set; }

	public XmlComicInfo GetForVersion(Version version)
		=> version.Major switch
	{
		1 => ToV1Xml(),
		2 => ToV2Xml(),
		_ => throw new NotSupportedException($"Version {version} is not supported")
	};

	private XmlComicInfo ToV1Xml()
		=> new()
		{
			Title = Title,
			Series = Series,
			Number = Number,
			Count = Count,
			Volume = Volume,
			AlternateSeries = AlternateSeries,
			AlternateNumber = AlternateNumber,
			AlternateCount = AlternateCount,
			Summary = Summary,
			Notes = Notes,
			Year = Year,
			Month = Month,
			Writer = Writer,
			Penciller = Penciller,
			Inker = Inker,
			Colorist = Colorist,
			Letterer = Letterer,
			CoverArtist = CoverArtist,
			Editor = Editor,
			Publisher = Publisher,
			Imprint = Imprint,
			Genre = Genre,
			Web = Web,
			PageCount = PageCount,
			LanguageISO = LanguageISO,
			Format = Format,
			BlackAndWhite = BlackAndWhite?.GetDisplayName(),
			Manga = Manga == null ? null : ComicInfoUtil.GetYesNoFromManga((Manga) Manga).GetDisplayName(),
			Pages = Pages is { Count: 0 } ?  null : new Pages
			{
				Page = Pages!.Select(p => new Page
				{
					Image = p.Image,
					Type = p.Type.GetDisplayName(),
					DoublePage = p.DoublePage,
					ImageSize = p.ImageSize,
					Key = p.Key,
					ImageWidth = p.ImageWidth,
					ImageHeight = p.ImageHeight
				}).ToList()
			},
		};

	private XmlComicInfo ToV2Xml()
		=> new()
		{
			Title = Title,
			Series = Series,
			Number = Number,
			Count = Count,
			Volume = Volume,
			AlternateSeries = AlternateSeries,
			AlternateNumber = AlternateNumber,
			AlternateCount = AlternateCount,
			Summary = Summary,
			Notes = Notes,
			Year = Year,
			Month = Month,
			Day = Day,
			Writer = Writer,
			Penciller = Penciller,
			Inker = Inker,
			Colorist = Colorist,
			Letterer = Letterer,
			CoverArtist = CoverArtist,
			Editor = Editor,
			Publisher = Publisher,
			Imprint = Imprint,
			Genre = Genre,
			Web = Web,
			PageCount = PageCount,
			LanguageISO = LanguageISO,
			Format = Format,
			BlackAndWhite = BlackAndWhite?.GetDisplayName(),
			Manga = Manga?.GetDisplayName(),
			Characters = Characters,
			Teams = Teams,
			Locations = Locations,
			ScanInformation = ScanInformation,
			StoryArc = StoryArc,
			SeriesGroup = SeriesGroup,
			AgeRating = AgeRating?.GetDisplayName(),
			Pages = Pages is { Count: 0 } ?  null : new Pages
			{
				Page = Pages!.Select(p => new Page
				{
					Image = p.Image,
					Type = p.Type.GetDisplayName(),
					DoublePage = p.DoublePage,
					ImageSize = p.ImageSize,
					Key = p.Key,
					Bookmark = p.Bookmark,
					ImageWidth = p.ImageWidth,
					ImageHeight = p.ImageHeight
				}).ToList()
			},
			CommunityRating = CommunityRating,
			MainCharacterOrTeam = MainCharacterOrTeam,
			Review = Review,
		};
}
