namespace MangaMagnet.Core.Database.Abstraction;

/// <summary>
///     Interface which abstracts CreatedAt for Database entities
/// </summary>
public interface ICreatable
{
    /// <summary>
    ///     When this entity was created in the database
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }
}