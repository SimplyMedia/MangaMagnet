using MangaMagnet.Api.Models.Response;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Metadata;

namespace MangaMagnet.Api.Service;

public class EntityConverterService
{
    public MangaResponse ConvertLocalMangaToResponse(LocalManga localManga)
        => new(
            localManga.Id,
            localManga.Path,
            ConvertMangaMetadataToResponse(localManga.Metadata),
            localManga.Metadata.ChapterMetadata.Select(ConvertChapterMetadataToResponse).ToList(),
            localManga.Chapters.Select(ConvertLocalChapterToResponse).ToList(),
            localManga.Volumes.Select(ConvertLocalVolumeToResponse).ToList(),
            localManga.CreatedAt,
            localManga.UpdatedAt
            );

    public MangaMetadataResponse ConvertMangaMetadataToResponse(MangaMetadata mangaMetadata)
        => new(
            mangaMetadata.Id,
            mangaMetadata.DisplayTitle,
            mangaMetadata.Aliases,
            mangaMetadata.Status,
            mangaMetadata.Year,
            mangaMetadata.Author,
            mangaMetadata.Artist,
            mangaMetadata.Description,
            mangaMetadata.Genres,
            mangaMetadata.Tags,
            mangaMetadata.UserScore,
            mangaMetadata.CoverImageUrl,
            mangaMetadata.AnilistId,
            mangaMetadata.MangaDexId,
            mangaMetadata.MangaUpdatesId,
            mangaMetadata.MyAnimeListId,
            mangaMetadata.CreatedAt,
            mangaMetadata.UpdatedAt
        );

    public ChapterMetadataResponse ConvertChapterMetadataToResponse(ChapterMetadata chapterMetadata)
        => new(
            chapterMetadata.Id,
            chapterMetadata.Title,
            chapterMetadata.ChapterNumber,
            chapterMetadata.VolumeNumber,
            chapterMetadata.CreatedAt,
            chapterMetadata.UpdatedAt
        );

    public ChapterResponse ConvertLocalChapterToResponse(LocalChapter chapter)
        => new(
            chapter.Id,
            chapter.ChapterNumber,
            chapter.Path,
            chapter.SizeInBytes,
            chapter.CreatedAt,
            chapter.UpdatedAt
        );

    public VolumeResponse ConvertLocalVolumeToResponse(LocalVolume volume)
        => new(
            volume.Id,
            volume.VolumeNumber,
            volume.Path,
            volume.SizeInBytes,
            volume.CreatedAt,
            volume.UpdatedAt
        );

    public MangaMetadata ConvertMangaMetadataResultToDbEntity(MangaMetadataResult result)
        => new()
        {
            DisplayTitle = result.DisplayTitle,
            Aliases = result.Aliases,
            Status = result.Status,
            Year = result.Year,
            Author = result.Author,
            Artist = result.Artist,
            Description = result.Description,
            Genres = result.Genres,
            Tags = result.Tags,
            UserScore = result.UserScore,
            CoverImageUrl = result.CoverImageUrl,
            AnilistId = result.AnilistId,
            MangaDexId = result.MangaDexId,
            MangaUpdatesId = result.MangaUpdatesId,
            MyAnimeListId = result.MyAnimeListId
        };

    public ChapterMetadata ConvertChapterMetadataResultToDbEntity(ChapterMetadataResult result, Guid mangaId)
        => new()
        {
            MangaId = mangaId,
            Title = result.Title,
            ChapterNumber = result.ChapterNumber,
            VolumeNumber = result.VolumeNumber,
        };
}