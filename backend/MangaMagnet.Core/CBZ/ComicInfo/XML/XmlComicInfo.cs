using System.Xml.Serialization;

namespace MangaMagnet.Core.CBZ.ComicInfo.XML;

[XmlRoot(ElementName="Page")]
public class Page {

	[XmlAttribute(AttributeName="Image")]
	public int Image { get; set; }

	[XmlAttribute(AttributeName="Type")]
	public string Type { get; set; }

	[XmlAttribute(AttributeName="DoublePage")]
	public bool DoublePage { get; set; }

	[XmlAttribute(AttributeName="ImageSize")]
	public long ImageSize { get; set; }

	[XmlAttribute(AttributeName="Key")]
	public string Key { get; set; }

	[XmlAttribute(AttributeName="Bookmark")]
	public string Bookmark { get; set; }

	[XmlAttribute(AttributeName="ImageWidth")]
	public int ImageWidth { get; set; }

	[XmlAttribute(AttributeName="ImageHeight")]
	public int ImageHeight { get; set; }
}

[XmlRoot(ElementName="Pages")]
public class Pages {

	[XmlElement(ElementName="Page")]
	public List<Page> Page { get; set; }
}

[XmlRoot(ElementName="ComicInfo")]
public class XmlComicInfo {

	[XmlElement(ElementName="Title")]
	public string Title { get; set; }

	[XmlElement(ElementName="Series")]
	public string Series { get; set; }

	[XmlElement(ElementName="Number")]
	public string Number { get; set; }

	[XmlElement(ElementName="Count")]
	public int Count { get; set; }

	[XmlElement(ElementName="Volume")]
	public int Volume { get; set; }

	[XmlElement(ElementName="AlternateSeries")]
	public string AlternateSeries { get; set; }

	[XmlElement(ElementName="AlternateNumber")]
	public string AlternateNumber { get; set; }

	[XmlElement(ElementName="AlternateCount")]
	public int AlternateCount { get; set; }

	[XmlElement(ElementName="Summary")]
	public string Summary { get; set; }

	[XmlElement(ElementName="Notes")]
	public string Notes { get; set; }

	[XmlElement(ElementName="Year")]
	public int Year { get; set; }

	[XmlElement(ElementName="Month")]
	public int Month { get; set; }

	[XmlElement(ElementName="Day")]
	public int Day { get; set; }

	[XmlElement(ElementName="Writer")]
	public string Writer { get; set; }

	[XmlElement(ElementName="Penciller")]
	public string Penciller { get; set; }

	[XmlElement(ElementName="Inker")]
	public string Inker { get; set; }

	[XmlElement(ElementName="Colorist")]
	public string Colorist { get; set; }

	[XmlElement(ElementName="Letterer")]
	public string Letterer { get; set; }

	[XmlElement(ElementName="CoverArtist")]
	public string CoverArtist { get; set; }

	[XmlElement(ElementName="Editor")]
	public string Editor { get; set; }

	[XmlElement(ElementName="Publisher")]
	public string Publisher { get; set; }

	[XmlElement(ElementName="Imprint")]
	public string Imprint { get; set; }

	[XmlElement(ElementName="Genre")]
	public string Genre { get; set; }

	[XmlElement(ElementName="Web")]
	public string Web { get; set; }

	[XmlElement(ElementName="PageCount")]
	public int PageCount { get; set; }

	[XmlElement(ElementName="LanguageISO")]
	public string LanguageISO { get; set; }

	[XmlElement(ElementName="Format")]
	public string Format { get; set; }

	[XmlElement(ElementName="BlackAndWhite")]
	public string BlackAndWhite { get; set; }

	[XmlElement(ElementName="Manga")]
	public string Manga { get; set; }

	[XmlElement(ElementName="Characters")]
	public string Characters { get; set; }

	[XmlElement(ElementName="Teams")]
	public string Teams { get; set; }

	[XmlElement(ElementName="Locations")]
	public string Locations { get; set; }

	[XmlElement(ElementName="ScanInformation")]
	public string ScanInformation { get; set; }

	[XmlElement(ElementName="StoryArc")]
	public string StoryArc { get; set; }

	[XmlElement(ElementName="SeriesGroup")]
	public string SeriesGroup { get; set; }

	[XmlElement(ElementName="AgeRating")]
	public string AgeRating { get; set; }

	[XmlElement(ElementName="Pages")]
	public Pages Pages { get; set; }

	[XmlElement(ElementName="CommunityRating")]
	public double CommunityRating { get; set; }

	[XmlElement(ElementName="MainCharacterOrTeam")]
	public string MainCharacterOrTeam { get; set; }

	[XmlElement(ElementName="Review")]
	public string Review { get; set; }

	[XmlAttribute(AttributeName="noNamespaceSchemaLocation")]
	public string NoNamespaceSchemaLocation { get; set; }

	[XmlAttribute(AttributeName="xsi")]
	public string Xsi { get; set; }

	[XmlText]
	public string Text { get; set; }
}
