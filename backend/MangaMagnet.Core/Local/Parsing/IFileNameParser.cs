namespace MangaMagnet.Core.Local.Parsing;

/// <summary>
///    Interface for parsing file names.
/// </summary>
public interface IFileNameParser
{
	/// <summary>
	///   Parses the file name and returns the result.
	/// </summary>
	/// <param name="fileName">The File name to parse</param>
	/// <returns></returns>
	FileParserResult Parse(string fileName);
}
