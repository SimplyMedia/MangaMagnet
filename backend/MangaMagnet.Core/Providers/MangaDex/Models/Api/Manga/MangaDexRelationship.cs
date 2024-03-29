﻿using System.Text.Json.Serialization;

namespace MangaMagnet.Core.Providers.MangaDex.Models.Api.Manga;

public record MangaDexRelationship(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("attributes")] MangaDexRelationshipAttribute Attributes,
    [property: JsonPropertyName("related")] string Related
);
