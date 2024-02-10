using MangaMagnet.Core.Database.Abstraction;

namespace MangaMagnet.Core.Database;

public class MangaMetadata : ICreatable, IUpdatable
{
    public Guid Id { get; set; }

    public string DisplayTitle { get; set; } = default!;

    public List<string> Aliases { get; set; } = default!;

    public MangaStatus Status { get; set; } = MangaStatus.OnGoing;

    public int? Year { get; set; }

    public string Author { get; set; } = default!;

    public string Artist { get; set; } = default!;

    public string Description { get; set; } = default!;

    public List<string> Genres { get; set; } = default!;

    public List<string> Tags { get; set; } = default!;

    public double UserScore { get; set; }

    public string? CoverImageUrl { get; set; } = default!;

    public long? AnilistId { get; set; }

    public string MangaDexId { get; set; } = default!;

    public string? MangaUpdatesId { get; set; } = default!;

    public long? MyAnimeListId { get; set; }

    public List<ChapterMetadata> ChapterMetadata { get; set; } = default!;

    /// <inheritdoc/>
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset UpdatedAt { get; set; }
}
