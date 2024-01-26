using System.Text.RegularExpressions;
using MangaMagnet.Core.Local.Parsing.Exceptions;
using Microsoft.Extensions.Logging;

namespace MangaMagnet.Core.Local.Parsing;

/// <summary>
/// Parses file names for manga.
/// </summary>
public class MangaFileNameParser(ILogger<MangaFileNameParser> logger) : IFileNameParser
{
	/// <inheritdoc />
	public FileParserResult Parse(string fileName)
	{
		ReadOnlySpan<Regex> volumeRegexes = [RegexConstants.VolumeReleaseRegex1(), RegexConstants.VolumeReleaseRegex2()];
		ReadOnlySpan<Regex> chapterRegexes = [RegexConstants.ChapterReleaseRegex1(), RegexConstants.ChapterReleaseRegex2()];

		foreach (var regex in volumeRegexes)
		{
			var match = regex.Match(fileName);
			if (!match.Success) continue;
			logger.LogDebug("Matched {FileName} as a volume release with Regex {Regex}", fileName, regex);

			var volume = match.Groups["Volume"].Value;

			var title = match.Groups["Title"].Value;
			var year = match.Groups["Year"].ValueSpan;
			var releaseGroup = match.Groups["ReleaseGroup"].Value;
			var digital = match.Groups["Digital"].ValueSpan;

			int? yearNumber = year.IsEmpty ? null : int.Parse(year);

			var volumeNumber = int.Parse(volume);

			return new FileParserResult(title, ParsedReleaseType.VOLUME, yearNumber, volumeNumber, null, releaseGroup, !digital.IsEmpty);
		}

		foreach (var regex in chapterRegexes)
		{
			var match = regex.Match(fileName);
			if (!match.Success) continue;
			logger.LogDebug("Matched {FileName} as a chapter release with Regex {Regex}", fileName, regex);

			var chapter = match.Groups["Chapter"].ValueSpan;

			var title = match.Groups["Title"].Value;
			var year = match.Groups["Year"].ValueSpan;
			var releaseGroup = match.Groups["ReleaseGroup"].Value;
			var digital = match.Groups["Digital"].ValueSpan;

			int? yearNumber = year.IsEmpty ? null : int.Parse(year);

			var chapterNumber = double.Parse(chapter);

			return new FileParserResult(title, ParsedReleaseType.CHAPTER, yearNumber, null, chapterNumber, releaseGroup, !digital.IsEmpty);
		}

		throw new FileNameNotParsableException("File Name could not be parsed.");
	}
}
