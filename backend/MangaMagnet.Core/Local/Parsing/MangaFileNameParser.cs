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
			var match = TryMatch(fileName, ParsedReleaseType.VOLUME, regex);
			if (match is not null) return match;
		}

		foreach (var regex in chapterRegexes)
		{
			var match = TryMatch(fileName, ParsedReleaseType.CHAPTER, regex);
			if (match is not null) return match;
		}

		throw new FileNameNotParsableException("File Name could not be parsed.");
	}

	private FileParserResult? TryMatch(string fileName, ParsedReleaseType type, Regex regex)
	{
		var match = regex.Match(fileName);
		if (!match.Success) return null;

		logger.LogDebug("Matched {FileName} as a {Type} release with Regex {Regex}", fileName, type.ToString().ToLower(), regex);

		var title = match.Groups["Title"].Value;
		var year = match.Groups["Year"].ValueSpan;
		var releaseGroup = match.Groups["ReleaseGroup"].Value;
		var digital = match.Groups["Digital"].ValueSpan;

		int? yearNumber = year.IsEmpty ? null : int.Parse(year);

		var fixedMatch = RegexConstants.FixedRegex().Match(fileName);
		if (fixedMatch.Success && fixedMatch.Groups["Number"].Value.Length > 0)
			logger.LogDebug("Matched {FileName} as a fixed release with Regex {Regex}", fileName, RegexConstants.FixedRegex());

		var containsFixedNumber = fixedMatch.Success && fixedMatch.Groups["Number"].ValueSpan.Length > 0;
		int? fixedNumber = containsFixedNumber  ? int.Parse(fixedMatch.Groups["Number"].ValueSpan) : null;

		int? volumeNumber = null;
		double? chapterNumber = null;

		switch (type)
		{
			case ParsedReleaseType.CHAPTER:
			{
				var chapter = match.Groups["Chapter"].ValueSpan;
				chapterNumber = double.Parse(chapter);
				break;
			}
			case ParsedReleaseType.VOLUME:
			{
				var volume = match.Groups["Volume"].Value;
				volumeNumber = int.Parse(volume);
				break;
			}
			case ParsedReleaseType.NONE:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
		}

		return new FileParserResult(title, type, yearNumber, volumeNumber, chapterNumber, releaseGroup, !digital.IsEmpty, fixedMatch.Success, fixedNumber);
	}
}
