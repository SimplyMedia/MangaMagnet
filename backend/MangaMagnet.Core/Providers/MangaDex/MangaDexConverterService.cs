using MangaMagnet.Core.Database;
using MangaMagnet.Core.Metadata;
using MangaMagnet.Core.Providers.MangaDex.Models;
using MangaMagnet.Core.Providers.MangaDex.Models.Api;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Covers;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Manga;
using MangaMagnet.Core.Providers.MangaDex.Models.Api.Statistics;

namespace MangaMagnet.Core.Providers.MangaDex;

public class MangaDexConverterService
{
    public MangaSearchMetadataResult ConvertToMangaSearchMetadataResult(MangaDexMangaData manga)
        => new(
            manga.Id,
            manga.Attributes.Links?.Anilist,
            manga.Attributes.Links?.MyAnimeList,
            manga.Attributes.Links?.MangaUpdates,
            manga.Attributes.Title.First().Value,
            manga.Attributes.Description.En,
            GetLatestCoverUrl(manga),
            ConvertMangaDexStatusToMangaStatus(manga.Attributes.Status),
            manga.Attributes.Year
        );

    public MangaMetadataResult ConvertToMangaMetadataResult(MangaDexResponse<MangaDexMangaData> mangaDexMangaData, MangaDexStatistics mangaDexStatistics, List<MangaDexCover> covers)
    {
	    var mangaDexData = mangaDexMangaData.Data;
	    var attributes = mangaDexData.Attributes;

	    var aliases = new List<string>();

	    foreach (var altTitle in attributes.AltTitles)
	    {
		    foreach (var (_, value) in altTitle)
		    {
			    aliases.Add(value);
		    }
	    }

	    var status = ConvertMangaDexStatusToMangaStatus(attributes.Status);

	    if (status == null)
		    throw new Exception($"Unknown status: {attributes.Status}");


	    var author = GetAuthor(mangaDexData);
	    var artist = GetArtist(mangaDexData);

	    if (author == null || artist == null)
		    throw new Exception("No author or artist found");

	    var genres = new List<string>();
	    var tags = new List<string>();

	    foreach (var tag in attributes.Tags)
	    {
			var tagAttributes = tag.Attributes;
		    if (tagAttributes.Group == "genre")
			    genres.Add(tagAttributes.Name.En);
		    else
			    tags.Add(tagAttributes.Name.En);
	    }

	    return new MangaMetadataResult(
		    attributes.Title.First().Value,
		    aliases,
		    status.Value,
		    attributes.Year,
		    author,
		    artist,
		    attributes.Description.En,
		    genres,
		    tags,
		    mangaDexStatistics.Rating.Average ?? mangaDexStatistics.Rating.Bayesian ?? 0.0,
		    GetFirstCoverUrl(mangaDexData, covers),
		    GetAnilistId(mangaDexData),
		    mangaDexData.Id,
		    attributes.Links?.MangaUpdates,
		    GetMyAnimeListId(mangaDexData)
	    );
    }

    private static long? GetAnilistId(MangaDexMangaData manga)
	{
		var anilistId = manga.Attributes.Links?.Anilist;

		return anilistId != null ? long.Parse(anilistId) : null;
	}

	private static long? GetMyAnimeListId(MangaDexMangaData manga)
	{
		var myAnimeListId = manga.Attributes.Links?.MyAnimeList;

		return myAnimeListId != null ? long.Parse(myAnimeListId) : null;
	}

    private static string? GetArtist(MangaDexMangaData manga)
	{
		var artist = manga.Relationships.FirstOrDefault(r => r.Type == "artist")?.Attributes.Name;

		return artist;
	}

	private static string? GetAuthor(MangaDexMangaData manga)
	{
		var author = manga.Relationships.FirstOrDefault(r => r.Type == "author")?.Attributes.Name;

		return author;
	}

	private static string? GetFirstCoverUrl(MangaDexMangaData manga, IEnumerable<MangaDexCover> covers)
	{
		var coverArtFileName = covers
				.Where(c => c.Type == "cover_art")
				.OrderBy(c => c.Attributes.Volume)
				.ThenBy(c =>
					c.Attributes.Locale switch
					{
						"ja" => 0,
						"en" => 1,
						_ => 2,
					})
				.FirstOrDefault()?
				.Attributes.FileName;

		return string.IsNullOrEmpty(coverArtFileName)
			? GetLatestCoverUrl(manga)
			: MangaDexConstants.MangaCoverImageUrl(manga.Id, coverArtFileName, MangaDexCoverUrlQuality.ORIGINAL);
	}

    private static string? GetLatestCoverUrl(MangaDexMangaData manga)
    {
        var coverArtFileName = manga.Relationships.FirstOrDefault(r => r.Type == "cover_art")?.Attributes.FileName;

        return string.IsNullOrEmpty(coverArtFileName)
	        ? null
	        : MangaDexConstants.MangaCoverImageUrl(manga.Id, coverArtFileName, MangaDexCoverUrlQuality.ORIGINAL);
    }

    private static MangaStatus? ConvertMangaDexStatusToMangaStatus(string status)
        => status switch
        {
            "ongoing" => MangaStatus.OnGoing,
            "completed" => MangaStatus.Completed,
            "hiatus" => MangaStatus.OnHold,
            "cancelled" => MangaStatus.Cancelled,
            _ => null
        };
}
