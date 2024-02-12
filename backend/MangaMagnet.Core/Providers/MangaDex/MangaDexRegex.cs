using System.Text.RegularExpressions;

namespace MangaMagnet.Core.Providers.MangaDex;

public static partial class MangaDexRegex
{
	[GeneratedRegex(@"(?<pageNumber>\d{1,4})-", RegexOptions.IgnoreCase)]
	public static partial Regex MangaDexPageNumberRegex();
}
