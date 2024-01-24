using System;
using System.Collections.Generic;
using MangaMagnet.Core.Database;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaMagnet.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:manga_status", "on_going,completed,cancelled,on_hold");

            migrationBuilder.CreateTable(
                name: "manga_metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_title = table.Column<string>(type: "text", nullable: false),
                    aliases = table.Column<List<string>>(type: "text[]", nullable: false),
                    status = table.Column<MangaStatus>(type: "manga_status", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: true),
                    author = table.Column<string>(type: "text", nullable: false),
                    artist = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    genres = table.Column<List<string>>(type: "text[]", nullable: false),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false),
                    user_score = table.Column<double>(type: "double precision", nullable: false),
                    cover_image_url = table.Column<string>(type: "text", nullable: true),
                    anilist_id = table.Column<long>(type: "bigint", nullable: true),
                    manga_dex_id = table.Column<string>(type: "text", nullable: false),
                    manga_updates_id = table.Column<string>(type: "text", nullable: true),
                    my_anime_list_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manga_metadata", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "local_mangas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    metadata_id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_local_mangas", x => x.id);
                    table.ForeignKey(
                        name: "fk_local_mangas_manga_metadata_metadata_id",
                        column: x => x.metadata_id,
                        principalTable: "manga_metadata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "local_volumes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    volume_number = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    local_manga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_local_volumes", x => x.id);
                    table.ForeignKey(
                        name: "fk_local_volumes_local_mangas_local_manga_id",
                        column: x => x.local_manga_id,
                        principalTable: "local_mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chapter_metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    manga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    chapter_number = table.Column<double>(type: "double precision", nullable: false),
                    volume_number = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    local_volume_id = table.Column<Guid>(type: "uuid", nullable: true),
                    manga_metadata_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chapter_metadata", x => x.id);
                    table.ForeignKey(
                        name: "fk_chapter_metadata_local_volumes_local_volume_id",
                        column: x => x.local_volume_id,
                        principalTable: "local_volumes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_chapter_metadata_manga_metadata_manga_metadata_id",
                        column: x => x.manga_metadata_id,
                        principalTable: "manga_metadata",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "local_chapters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    chapter_number = table.Column<float>(type: "real", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    local_manga_id = table.Column<Guid>(type: "uuid", nullable: false),
                    metadata_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_local_chapters", x => x.id);
                    table.ForeignKey(
                        name: "fk_local_chapters_chapter_metadata_metadata_id",
                        column: x => x.metadata_id,
                        principalTable: "chapter_metadata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_local_chapters_local_mangas_local_manga_id",
                        column: x => x.local_manga_id,
                        principalTable: "local_mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chapter_metadata_local_volume_id",
                table: "chapter_metadata",
                column: "local_volume_id");

            migrationBuilder.CreateIndex(
                name: "ix_chapter_metadata_manga_metadata_id",
                table: "chapter_metadata",
                column: "manga_metadata_id");

            migrationBuilder.CreateIndex(
                name: "ix_local_chapters_local_manga_id",
                table: "local_chapters",
                column: "local_manga_id");

            migrationBuilder.CreateIndex(
                name: "ix_local_chapters_metadata_id",
                table: "local_chapters",
                column: "metadata_id");

            migrationBuilder.CreateIndex(
                name: "ix_local_mangas_metadata_id",
                table: "local_mangas",
                column: "metadata_id");

            migrationBuilder.CreateIndex(
                name: "ix_local_volumes_local_manga_id",
                table: "local_volumes",
                column: "local_manga_id");

            migrationBuilder.CreateIndex(
                name: "ix_manga_metadata_manga_dex_id",
                table: "manga_metadata",
                column: "manga_dex_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "local_chapters");

            migrationBuilder.DropTable(
                name: "chapter_metadata");

            migrationBuilder.DropTable(
                name: "local_volumes");

            migrationBuilder.DropTable(
                name: "local_mangas");

            migrationBuilder.DropTable(
                name: "manga_metadata");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:manga_status", "on_going,completed,cancelled,on_hold");
        }
    }
}
