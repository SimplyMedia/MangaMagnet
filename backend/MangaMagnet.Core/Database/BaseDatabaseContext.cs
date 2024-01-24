using MangaMagnet.Core.Database.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace MangaMagnet.Core.Database;

public class BaseDatabaseContext : DbContext
{
    public DbSet<MangaMetadata> MangaMetadata { get; set; } = default!;
    public DbSet<ChapterMetadata> ChapterMetadata { get; set; } = default!;
    public DbSet<LocalManga> LocalMangas { get; set; } = default!;
    public DbSet<LocalChapter> LocalChapters { get; set; } = default!;
    public DbSet<LocalVolume> LocalVolumes { get; set; } = default!;

    /// <inheritdoc />
    public BaseDatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        #region MangaMetadata

        builder.Entity<MangaMetadata>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<MangaMetadata>()
            .HasIndex(i => i.MangaDexId)
            .IsUnique();

        #endregion

        #region ChapterMetadata

        builder.Entity<ChapterMetadata>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        #endregion

        #region LocalManga

        builder.Entity<LocalManga>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        #endregion

        #region LocalChapter

        builder.Entity<LocalChapter>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        #endregion

        #region LocalVolume

        builder.Entity<LocalVolume>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        #endregion

        base.OnModelCreating(builder);
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTimestamps();

        return base.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override int SaveChanges()
    {
        UpdateTimestamps();

        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var changedEntries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .ToList();

        var now = DateTimeOffset.UtcNow;

        foreach (var entry in changedEntries)
        {
            if (entry is { State: EntityState.Added, Entity: ICreatable creatable })
                creatable.CreatedAt = now;

            if (entry.Entity is IUpdatable updatable)
                updatable.UpdatedAt = now;
        }
    }
}