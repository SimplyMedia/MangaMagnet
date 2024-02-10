namespace MangaMagnet.Core.CBZ.ComicInfo;

public static class ComicInfoUtil
{
	public static YesNo GetYesNoFromManga(Manga manga)
		=> manga switch
			{
				Manga.NO => YesNo.NO,
				Manga.YES => YesNo.YES,
				Manga.YES_AND_LEFT_TO_RIGHT => YesNo.YES,
				_ => YesNo.UNKNOWN
			};
}
