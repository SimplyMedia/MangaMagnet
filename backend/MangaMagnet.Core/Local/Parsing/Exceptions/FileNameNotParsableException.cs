namespace MangaMagnet.Core.Local.Parsing.Exceptions;

/// <summary>
///     Exception thrown when a FileName is not parsable
/// </summary>
public class FileNameNotParsableException : Exception
{
	/// <summary>
	///     Creates a new instance of <see cref="FileNameNotParsableException" />
	/// </summary>
	/// <param name="reason">The reason why this Release is not parsable</param>
	public FileNameNotParsableException(string reason) : base(reason)
	{
	}
}
