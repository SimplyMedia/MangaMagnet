using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Chapter;

public record MangaDexAltName(
	[property: JsonPropertyName("en")] string En
);
