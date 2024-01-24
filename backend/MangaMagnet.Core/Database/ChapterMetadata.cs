using MangaMagnet.Core.Database.Abstraction;

namespace MangaMagnet.Core.Database;

public class ChapterMetadata : ICreatable, IUpdatable
{
    public Guid Id { get; set; }

    public Guid MangaId { get; set; }

    public string? Title { get; set; }

    public double ChapterNumber { get; set; }

    public int? VolumeNumber { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt { get; set; }
}
