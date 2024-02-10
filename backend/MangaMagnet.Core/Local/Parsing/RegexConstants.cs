using System.Text.RegularExpressions;

namespace MangaMagnet.Core.Local.Parsing;

public static partial class RegexConstants
{
	[GeneratedRegex(@"(?<Title>.*)?[ _.-]v(?<Volume>[0-9]+)[ _.-](\((?<Year>\d{4})\))?[ _.-]?(?<Digital>\(digital\))?[ _.-]?(\((?<ReleaseGroup>.*)\))", RegexOptions.IgnoreCase)]
	public static partial Regex VolumeReleaseRegex1();

	[GeneratedRegex(@"\[(?<ReleaseGroup>.*)\][ _.-](?<Title>.*)((vol|v|volume)\.?)[ _.-](?<Volume>\d+)", RegexOptions.IgnoreCase)]
	public static partial Regex VolumeReleaseRegex2();

	[GeneratedRegex(@".*[ _.-]c?(?<Chapter>[0-9]+)[ _.-](\((?<Year>\d{4})\))?[ _.-](?<Digital>\(digital\))?[ _.-]?(\((?<ReleaseGroup>.*)\))", RegexOptions.IgnoreCase)]
	public static partial Regex ChapterReleaseRegex1();

	[GeneratedRegex(@".*[ _.-]c?(?<Chapter>[0-9]+)",  RegexOptions.IgnoreCase)]
	public static partial Regex ChapterReleaseRegex2();

	[GeneratedRegex(@"[\[(](f(?<Number>\d+)?)[\])] ?", RegexOptions.IgnoreCase)]
	public static partial Regex FixedRegex();
}
