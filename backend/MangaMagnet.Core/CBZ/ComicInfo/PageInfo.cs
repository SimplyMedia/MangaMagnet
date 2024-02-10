namespace MangaMagnet.Core.CBZ.ComicInfo;

public class PageInfo
{
	public int Image { get; set; }

	public ComicPageType Type { get; set; } = ComicPageType.STORY;

	public bool DoublePage = false;

	public long ImageSize { get; set; }

	public string Key { get; set; }

	public string Bookmark { get; set; }

	public int ImageWidth { get; set; }

	public int ImageHeight { get; set; }
}
