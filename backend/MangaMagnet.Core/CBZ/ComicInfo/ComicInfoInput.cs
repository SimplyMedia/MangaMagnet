using MangaMagnet.Core.Database;

namespace MangaMagnet.Core.CBZ.ComicInfo;

public record ComicInfoInput(IEnumerable<string> PagePaths, ComicInfoChapterMetadata ComicMetadata, MangaMetadata MangaMetadata, ComicInfoVersion Version);
