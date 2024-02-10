using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Chapter;

public record MangaDexAltName(
	[property: JsonPropertyName("en")] string En
);
