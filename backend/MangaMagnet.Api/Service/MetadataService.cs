using MangaMagnet.Api.Exceptions;
using MangaMagnet.Api.Models.Response;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Metadata;
using MangaMagnet.Core.Progress;
using MangaMagnet.Core.Progress.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaMagnet.Api.Service;

public class MetadataService(ILogger<MetadataService> logger, IMetadataFetcher metadataFetcher, BaseDatabaseContext dbContext, EntityConverterService entityConverterService, ProgressService progressService)
{
	public Task<List<MangaSearchMetadataResult>> SearchMangaMetadataByNameAsync(string mangaName, CancellationToken cancellationToken = default)
		=> metadataFetcher.SearchMangaMetadataAsync(mangaName, cancellationToken);

	public async Task<MangaMetadataResult> FetchMangaMetadataAsync(string mangaDexId, CancellationToken cancellationToken = default)
	{
		var mangaDexMangaData = await metadataFetcher.FetchMangaMetadataAsync(mangaDexId, cancellationToken);

		return mangaDexMangaData;
	}

	public Task<List<ChapterMetadataResult>> FetchAllChapterMetadataAsync(string mangaDexId, CancellationToken cancellationToken = default)
		=> metadataFetcher.FetchAllChapterMetadataAsync(mangaDexId, cancellationToken);

	public async Task<MangaMetadata> CreateOrUpdateMangaMetadata(string mangaDexId, CancellationToken cancellationToken)
    {
        var mangaMetadata = await dbContext.MangaMetadata.FirstOrDefaultAsync(m => m.MangaDexId == mangaDexId, cancellationToken);

        if (mangaMetadata is null)
	        return await CreateMangaMetadataAsync(mangaDexId, cancellationToken);

        return await UpdateMangaMetadataAsync(mangaDexId, mangaMetadata, cancellationToken);

    }

	public async Task<MangaMetadataResponse> RefreshMetadataAsync(string mangaDexId, CancellationToken cancellationToken = default)
	{
		if (!await dbContext.LocalMangas.AnyAsync(m => m.Metadata.MangaDexId == mangaDexId, cancellationToken))
			throw new NotFoundException($"Manga with id {mangaDexId} does not exist in database");

		logger.LogDebug("Updating Metadata for manga with id {MangaDexId} in database", mangaDexId);

		var updated = await CreateOrUpdateMangaMetadata(mangaDexId, cancellationToken);

		logger.LogDebug("Updated Metadata for manga with id {MangaDexId} in database", mangaDexId);

		updated.ChapterMetadata = await UpdateMangaChapterMetadataAsync(updated, cancellationToken);

		return entityConverterService.ConvertMangaMetadataToResponse(updated);
	}

	public async Task<IEnumerable<MangaMetadataResponse>> UpdateAllMetadataAsync(CancellationToken cancellationToken = default)
	{
		using var progressTask = progressService.CreateTask("Refreshing Metadata");

		var metadata = await dbContext.MangaMetadata.ToListAsync(cancellationToken);

		progressTask.Total = metadata.Count;

		var updated = new List<MangaMetadataResponse>();

		foreach (var mangaMetadata in metadata)
		{
			progressTask.Description = mangaMetadata.DisplayTitle;
			updated.Add(await RefreshMetadataAsync(mangaMetadata.MangaDexId, cancellationToken));
			progressTask.Increment();
		}

		logger.LogDebug("Updated all metadata");

		return updated;
	}

	public async Task CheckAllMangaForNewChapterMetadataAsync(CancellationToken cancellationToken = default)
	{
		using var progressTask = progressService.CreateTask("Checking for new Chapters");

		var mangaMetadata = await dbContext.MangaMetadata
			.Include(mangaMetadata => mangaMetadata.ChapterMetadata)
			.Where(metadata => metadata.Status != MangaStatus.Completed)
			.ToListAsync(cancellationToken);

		progressTask.Total = mangaMetadata.Count;

		foreach (var metadata in mangaMetadata)
			await CheckMangaForNewChapterAsync(progressTask, metadata, cancellationToken);
	}

	public async Task CheckMangaForNewChapterMetadataByIdAsync(string mangaDexId, CancellationToken cancellationToken = default)
	{
		using var progressTask = progressService.CreateTask("Checking for new Chapters");

		var mangaMetadata = await dbContext.MangaMetadata
			.Include(mangaMetadata => mangaMetadata.ChapterMetadata)
			.FirstOrDefaultAsync(m => m.MangaDexId == mangaDexId, cancellationToken);

		if (mangaMetadata is null)
			throw new Exception("");

		progressTask.Total = 1;

		await CheckMangaForNewChapterAsync(progressTask, mangaMetadata, cancellationToken);
	}

	private async Task CheckMangaForNewChapterAsync(ProgressTask progressTask, MangaMetadata metadata, CancellationToken cancellationToken)
	{
		progressTask.Description = metadata.DisplayTitle;
		var latestChapters = await metadataFetcher.FetchLatestChapterMetadataAsync(metadata.MangaDexId, cancellationToken);

		foreach (var chapter in latestChapters)
		{
			var alreadyExists = metadata.ChapterMetadata.Exists(c => Math.Abs(c.ChapterNumber - chapter.ChapterNumber) < 0.01);

			if (alreadyExists) continue;

			var newChapter = entityConverterService.ConvertChapterMetadataResultToDbEntity(chapter, metadata.Id);
			await dbContext.ChapterMetadata.AddAsync(newChapter, cancellationToken);
			metadata.ChapterMetadata.Add(newChapter);
		}

		await dbContext.SaveChangesAsync(cancellationToken);

		progressTask.Increment();
	}

	private async Task<MangaMetadata> CreateMangaMetadataAsync(string mangaDexId, CancellationToken cancellationToken)
    {
        if (await dbContext.MangaMetadata.AnyAsync(m => m.MangaDexId == mangaDexId, cancellationToken))
            throw new AlreadyExistException($"Manga with id {mangaDexId} already exists in database");

        var mangaMetadataResult = await metadataFetcher.FetchMangaMetadataAsync(mangaDexId, cancellationToken);

        logger.LogTrace("Manga metadata: {@MangaMetadataResult}", mangaMetadataResult);

        var mangaMetadata = entityConverterService.ConvertMangaMetadataResultToDbEntity(mangaMetadataResult);

        await dbContext.MangaMetadata.AddAsync(mangaMetadata, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await CreateMangaChapterMetadataAsync(mangaDexId, mangaMetadata, cancellationToken);

        return mangaMetadata;
    }

	private async Task CreateMangaChapterMetadataAsync(string mangaDexId, MangaMetadata mangaMetadata,
		CancellationToken cancellationToken)
	{
		var chaptersMetadata = await metadataFetcher.FetchAllChapterMetadataAsync(mangaDexId, cancellationToken);

		logger.LogTrace("Chapters metadata: {@ChaptersMetadata}", chaptersMetadata);

		var chapters = chaptersMetadata.Select(chapterMetadata => entityConverterService.ConvertChapterMetadataResultToDbEntity(chapterMetadata, mangaMetadata.Id)).ToList();

		await Task.WhenAll(chapters.Select(c => dbContext.ChapterMetadata.AddAsync(c, cancellationToken).AsTask()));
		await dbContext.SaveChangesAsync(cancellationToken);

		mangaMetadata.ChapterMetadata = chapters;
	}

	private async Task<List<ChapterMetadata>> UpdateMangaChapterMetadataAsync(MangaMetadata metadata, CancellationToken cancellationToken)
	{
		var chaptersMetadata = await metadataFetcher.FetchAllChapterMetadataAsync(metadata.MangaDexId, cancellationToken);

		logger.LogTrace("Chapters metadata: {@ChaptersMetadata}", chaptersMetadata);

		var dbMangaChapterMetadata = await dbContext.ChapterMetadata
			.Where(c => c.MangaId == metadata.Id)
			.ToListAsync(cancellationToken);

		logger.LogTrace("DB Chapters metadata: {@DbChaptersMetadata}", dbMangaChapterMetadata);

		var newOrUpdatedChapters = new List<ChapterMetadata>();

		foreach (var newOrUpdatedChapter in chaptersMetadata)
		{
			var existingChapter = dbMangaChapterMetadata.FirstOrDefault(c => Math.Abs(c.ChapterNumber - newOrUpdatedChapter.ChapterNumber) < 0.01);

			if (existingChapter is null)
			{
				var newChapter = entityConverterService.ConvertChapterMetadataResultToDbEntity(newOrUpdatedChapter, metadata.Id);
				await dbContext.ChapterMetadata.AddAsync(newChapter, cancellationToken);
				metadata.ChapterMetadata.Add(newChapter);
				newOrUpdatedChapters.Add(newChapter);
			}
			else
			{
				existingChapter.Title = newOrUpdatedChapter.Title ?? existingChapter.Title;
				existingChapter.VolumeNumber = newOrUpdatedChapter.VolumeNumber;
				newOrUpdatedChapters.Add(existingChapter);
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);

		return newOrUpdatedChapters;
	}

	private async Task<MangaMetadata> UpdateMangaMetadataAsync(string mangaDexId, MangaMetadata metadata, CancellationToken cancellationToken)
    {
        var mangaMetadataResult = await metadataFetcher.FetchMangaMetadataAsync(mangaDexId, cancellationToken);

        logger.LogTrace("Manga metadata: {@MangaMetadataResult}", mangaMetadataResult);

        metadata.DisplayTitle = mangaMetadataResult.DisplayTitle;
        metadata.Aliases = mangaMetadataResult.Aliases;
        metadata.Status = mangaMetadataResult.Status;
        metadata.Year = mangaMetadataResult.Year;
        metadata.Author = mangaMetadataResult.Author;
        metadata.Artist = mangaMetadataResult.Artist;
        metadata.Description = mangaMetadataResult.Description;
        metadata.Genres = mangaMetadataResult.Genres;
        metadata.Tags = mangaMetadataResult.Tags;
        metadata.UserScore = mangaMetadataResult.UserScore;
        metadata.CoverImageUrl = mangaMetadataResult.CoverImageUrl;
        metadata.AnilistId = mangaMetadataResult.AnilistId;
        metadata.MangaDexId = mangaMetadataResult.MangaDexId;
        metadata.MangaUpdatesId = mangaMetadataResult.MangaUpdatesId;
        metadata.MyAnimeListId = mangaMetadataResult.MyAnimeListId;

        await dbContext.SaveChangesAsync(cancellationToken);

        return metadata;
    }
}
