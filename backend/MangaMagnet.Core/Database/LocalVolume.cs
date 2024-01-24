using MangaMagnet.Core.Database.Abstraction;

namespace MangaMagnet.Core.Database;

public class LocalVolume : ICreatable, IUpdatable
{
    public Guid Id { get; set; }

    public int VolumeNumber { get; set; }

    public string Path { get; set; } = default!;

    public long SizeInBytes { get; set; }

    public LocalManga LocalManga { get; set; } = default!;

    public List<ChapterMetadata> ChapterMetadata { get; set; } = default!;

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt { get; set; }
}