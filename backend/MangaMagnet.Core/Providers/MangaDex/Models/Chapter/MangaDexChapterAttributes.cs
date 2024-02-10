using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Chapter;

public record MangaDexChapterAttributes(
	[property: JsonPropertyName("volume")] string Volume,
	[property: JsonPropertyName("chapter")] string Chapter,
	[property: JsonPropertyName("title")] string Title,
	[property: JsonPropertyName("translatedLanguage")] string TranslatedLanguage,
	[property: JsonPropertyName("externalUrl")] object ExternalUrl,
	[property: JsonPropertyName("publishAt")] DateTime? PublishAt,
	[property: JsonPropertyName("readableAt")] DateTime? ReadableAt,
	[property: JsonPropertyName("createdAt")] DateTime? CreatedAt,
	[property: JsonPropertyName("updatedAt")] DateTime? UpdatedAt,
	[property: JsonPropertyName("pages")] int? Pages,
	[property: JsonPropertyName("version")] int? Version,
	[property: JsonPropertyName("username")] string Username,
	[property: JsonPropertyName("roles")] IReadOnlyList<string> Roles,
	[property: JsonPropertyName("name")] string Name,
	[property: JsonPropertyName("altNames")] IReadOnlyList<MangaDexAltName> AltNames,
	[property: JsonPropertyName("locked")] bool? Locked,
	[property: JsonPropertyName("website")] string Website,
	[property: JsonPropertyName("ircServer")] object IrcServer,
	[property: JsonPropertyName("ircChannel")] object IrcChannel,
	[property: JsonPropertyName("discord")] string Discord,
	[property: JsonPropertyName("contactEmail")] string ContactEmail,
	[property: JsonPropertyName("description")] string Description,
	[property: JsonPropertyName("twitter")] string Twitter,
	[property: JsonPropertyName("mangaUpdates")] string MangaUpdates,
	[property: JsonPropertyName("focusedLanguages")] IReadOnlyList<string> FocusedLanguages,
	[property: JsonPropertyName("official")] bool? Official,
	[property: JsonPropertyName("verified")] bool? Verified,
	[property: JsonPropertyName("inactive")] bool? Inactive,
	[property: JsonPropertyName("publishDelay")] object PublishDelay,
	[property: JsonPropertyName("exLicensed")] bool? ExLicensed
);
