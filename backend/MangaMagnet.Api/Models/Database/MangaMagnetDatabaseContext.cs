using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.PostgreSQL;
using MangaMagnet.Core.Database;
using Microsoft.EntityFrameworkCore;

namespace MangaMagnet.Api.Models.Database;

public class MangaMagnetDatabaseContext : BaseDatabaseContext
{
    /// <inheritdoc />
    public MangaMagnetDatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<MangaStatus>();

        // Prefix and schema can be passed as parameters
        // Adds Quartz.NET PostgreSQL schema to EntityFrameworkCore
        builder.AddQuartz(b => b.UsePostgreSql());

        base.OnModelCreating(builder);
    }
}