using MangaMagnet.Core.Database;
using MangaMagnet.Core.Local.Parsing;
using MangaMagnet.Core.Local.Parsing.Exceptions;
using MangaMagnet.Core.Progress;
using MangaMagnet.Core.Progress.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Local;

public class LocalFileService(ILogger<LocalFileService> logger, BaseDatabaseContext dbContext, IFileNameParser fileNameParser, ProgressService progressService)
{
	public async Task VerifyAllLocalVolumeAndChapters(CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Verifying Local Volume and Chapters for all Mangas");

		using var progressTask = progressService.CreateTask("Verify Local Files");

		var localMangas = await dbContext.LocalMangas
			.Include(m => m.Metadata)
			.Include(m => m.Volumes)
			.Include(m => m.Chapters)
			.ToListAsync(cancellationToken: cancellationToken);

		progressTask.Total = localMangas.Count;

		foreach (var manga in localMangas)
		{
			await VerifyLocalVolumeAndChapters(manga, progressTask, cancellationToken);
			progressTask.Increment();
		}

		logger.LogDebug("Finished Verifying Local Volume and Chapters for all Mangas");
	}

	public async Task VerifyLocalVolumeAndChapters(LocalManga manga, ProgressTask progressTask, CancellationToken cancellationToken = default)
	{
		logger.LogDebug("Verifying Local Volume and Chapters for {Manga}", manga.Metadata.DisplayTitle);

		progressTask.Description = manga.Metadata.DisplayTitle;

		foreach (var localVolume in manga.Volumes)
			await VerifyLocalVolume(manga, localVolume, cancellationToken);

		foreach (var localChapter in manga.Chapters)
			await VerifyLocalChapter(manga, localChapter, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);

		logger.LogDebug("Finished Verifying Local Volume and Chapters for {Manga}", manga.Metadata.DisplayTitle);

		var filePaths = await Task.Run(() => Directory.EnumerateFiles(manga.Path), cancellationToken);

		foreach (var path in filePaths)
			await VerifyLocalFile(manga, path, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private async Task VerifyLocalFile(LocalManga manga, string path, CancellationToken cancellationToken = default)
	{
		var fileName = Path.GetFileName(path);

		if (manga.Volumes.Any(x => x.Path == path) || manga.Chapters.Any(x => x.Path == path)) return;

		logger.LogDebug("Found new file {FileName} in {Manga}", fileName, manga.Metadata.DisplayTitle);

		if (!TryParseFileName(fileName, out var result)) return;
		var parsedResult = result!;

		logger.LogDebug("Parsed {FileName} as a {Type} release", fileName, parsedResult.ParsedType.ToString());

		var fileSize = await Task.Run(() => new FileInfo(path).Length, cancellationToken);

		switch (parsedResult.ParsedType)
		{
			case ParsedReleaseType.VOLUME:
			{
				var volume = new LocalVolume
				{
					Path = path,
					VolumeNumber = (int)parsedResult.VolumeNumber!,
					LocalManga = manga,
					ChapterMetadata = await dbContext.ChapterMetadata
						.Where(x => x.VolumeNumber != null && x.VolumeNumber == parsedResult.VolumeNumber)
						.ToListAsync(cancellationToken: cancellationToken),
					SizeInBytes = fileSize
				};

				manga.Volumes.Add(volume);
				dbContext.LocalVolumes.Add(volume);
				break;
			}
			case ParsedReleaseType.CHAPTER:
			{
				var chapterNumber = (double) parsedResult.ChapterNumber!;

				var chapter = new LocalChapter
				{
					Path = path,
					ChapterNumber = chapterNumber!,
					LocalManga = manga,
					Metadata = await dbContext.ChapterMetadata.FirstAsync(x => Math.Abs(chapterNumber - x.ChapterNumber) < 0.1, cancellationToken: cancellationToken),
					SizeInBytes = fileSize
				};

				manga.Chapters.Add(chapter);
				dbContext.LocalChapters.Add(chapter);
				break;
			}
			case ParsedReleaseType.NONE:
				throw new NotSupportedException("Non Volume or Chapters aren't supported yet.");
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private async Task VerifyLocalChapter(LocalManga manga, LocalChapter localChapter, CancellationToken cancellationToken = default)
	{
		var exists = await Task.Run(() => File.Exists(localChapter.Path), cancellationToken);

		if (exists) return;

		logger.LogWarning("Chapter {Chapter} from Manga {Manga} was not found anymore, removing from database", localChapter.ChapterNumber, manga.Metadata.DisplayTitle);
		dbContext.LocalChapters.Remove(localChapter);
	}

	private async Task VerifyLocalVolume(LocalManga manga, LocalVolume localVolume, CancellationToken cancellationToken = default)
	{
		var exists = await Task.Run(() => File.Exists(localVolume.Path), cancellationToken);

		if (exists)
		{
			var fileSize = await Task.Run(() => new FileInfo(localVolume.Path).Length, cancellationToken);

			if (fileSize != localVolume.SizeInBytes)
				localVolume.SizeInBytes = fileSize;

			return;
		}

		logger.LogWarning("Volume {Volume} from Manga {Manga} was not found anymore, removing from database", localVolume.VolumeNumber, manga.Metadata.DisplayTitle);
		dbContext.LocalVolumes.Remove(localVolume);
	}

	private bool TryParseFileName(string fileName, out FileParserResult? result)
	{
      		try
	        {
		        result = fileNameParser.Parse(fileName);
		        return true;
	        }
	        catch (FileNameNotParsableException)
	        {
		        result = null;
		        return false;
	        }
	}
}
