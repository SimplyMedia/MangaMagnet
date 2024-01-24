using MangaMagnet.Core.Database.Abstraction;

namespace MangaMagnet.Core.Database;

public class LocalManga : ICreatable, IUpdatable
{
    public Guid Id { get; init; }

    public MangaMetadata Metadata { get; init; } = default!;

    public string Path { get; init; } = default!;

    public List<LocalChapter> Chapters { get; set; } = default!;

    public List<LocalVolume> Volumes { get; set; } = default!;

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt { get; set; }
}