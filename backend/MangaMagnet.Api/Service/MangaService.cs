using MangaMagnet.Api.Exceptions;
using MangaMagnet.Api.Models.Response;
using MangaMagnet.Core.Database;
using MangaMagnet.Core.Download;
using Microsoft.EntityFrameworkCore;

namespace MangaMagnet.Api.Service;

public class MangaService(ILogger<MangaService> logger, BaseDatabaseContext dbContext, EntityConverterService entityConverterService, MetadataService metadataService, DownloadService downloadService)
{
    public async Task<MangaResponse> CreateAsync(string mangaDexId, string path, CancellationToken cancellationToken = default)
    {
        if (await dbContext.LocalMangas.AnyAsync(m => m.Metadata.MangaDexId == mangaDexId, cancellationToken))
            throw new AlreadyExistException($"Manga with id {mangaDexId} already exists in database");

        logger.LogDebug("Creating manga with id {MangaDexId} in database", mangaDexId);

        var mangaMetadata = await metadataService.CreateOrUpdateMangaMetadata(mangaDexId, cancellationToken);

        // Create empty local manga
        var localManga = new LocalManga
        {
            Metadata = mangaMetadata,
            Path = path,
            Chapters = [],
            Volumes = []
        };

        await dbContext.LocalMangas.AddAsync(localManga, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogDebug("Created manga with id {MangaDexId} in database", mangaDexId);

        return entityConverterService.ConvertLocalMangaToResponse(localManga);
    }

    public async Task<List<MangaResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var localManga = await dbContext.LocalMangas
            .Include(l => l.Metadata)
            .Include(l => l.Metadata.ChapterMetadata)
            .Include(l => l.Chapters)
            .Include(l => l.Volumes)
            .ToListAsync(cancellationToken);

        return localManga.Select(entityConverterService.ConvertLocalMangaToResponse).ToList();
    }

    public async Task<MangaResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var localManga = await dbContext.LocalMangas
	        .Include(m => m.Chapters)
	        .Include(m => m.Volumes)
	        .Include(m => m.Metadata)
	        .Include(m => m.Metadata.ChapterMetadata)
	        .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (localManga is null)
            throw new NotFoundException($"Manga with id {id} does not exist in database");

        return entityConverterService.ConvertLocalMangaToResponse(localManga);
    }

    public async Task<MangaResponse> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var localManga = await dbContext.LocalMangas.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (localManga is null)
            throw new NotFoundException($"Manga with id {id} does not exist in database");

        dbContext.LocalMangas.Remove(localManga);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entityConverterService.ConvertLocalMangaToResponse(localManga);
    }

    public async Task DownloadChapterAsync(Guid id, double chapterNumber)
    {
	    var localManga = await dbContext.LocalMangas
		    .Include(m => m.Metadata)
		    .FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Manga not found");

	    await downloadService.DownloadChapterAsCBZAsync(id, chapterNumber, localManga.Path);
    }
}
