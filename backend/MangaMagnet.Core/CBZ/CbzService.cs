using System.IO.Compression;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.CBZ;

public class CbzService(ILogger<CbzService> logger)
{
	public async Task CreateAsync(string imagePath, string outputDirectory, string fileName, CancellationToken cancellationToken = default)
	{
		if (!Directory.Exists(outputDirectory))
		{
			Directory.CreateDirectory(outputDirectory);
		}

		var outputPathZip = Path.Combine(outputDirectory, $"{fileName}.zip");

		await Task.Run(() => ZipFile.CreateFromDirectory(imagePath, outputPathZip, CompressionLevel.SmallestSize, false), cancellationToken);

		logger.LogDebug("Zipped folder {OriginalFolder} and wrote to {Path}", imagePath, outputPathZip);

		var outputPath = Path.Combine(outputDirectory, $"{fileName}.cbz");

		await Task.Run(() => File.Move(outputPathZip, outputPath), cancellationToken);

		logger.LogDebug("Renamed file {Path} to {NewPath}", outputPathZip, outputPath);
	}
}
