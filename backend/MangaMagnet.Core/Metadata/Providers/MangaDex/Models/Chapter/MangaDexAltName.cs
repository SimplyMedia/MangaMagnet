using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Metadata.Providers.MangaDex.Models.Chapter;

public record MangaDexAltName(
	[property: JsonPropertyName("en")] string En
);
