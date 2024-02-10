using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Manga;

public record MangaDexTagAttributes(
	[property: JsonPropertyName("name")] MangaDexName Name,
	[property: JsonPropertyName("group")] string Group,
	[property: JsonPropertyName("version")] int Version
);
