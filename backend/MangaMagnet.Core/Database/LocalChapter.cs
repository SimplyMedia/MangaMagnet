using MangaMagnet.Core.Database.Abstraction;

namespace MangaMagnet.Core.Database;

public class LocalChapter : ICreatable, IUpdatable
{
    public Guid Id { get; set; }

    public float ChapterNumber { get; set; }

    public string Path { get; set; } = default!;

    public long SizeInBytes { get; set; }

    public LocalManga LocalManga { get; set; } = default!;

    public ChapterMetadata Metadata { get; set; } = default!;

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt { get; set; }
}