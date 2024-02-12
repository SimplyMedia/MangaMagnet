namespace MangaMagnet.Core.Providers.MangaDex.Models.Download;

public record MangaDexDownloadResult(List<string> LocalImagePaths, string TempImageBasePath, MangaDexDownloadMetadata Metadata);
