﻿namespace MangaMagnet.Core.Database.Abstraction;

/// <summary>
///     Interface which abstracts UpdatedAt for Database entities
/// </summary>
public interface IUpdatable
{
    /// <summary>
    ///     When this entity was last updated in the Database
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}