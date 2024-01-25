using JetBrains.Annotations;
using MangaMagnet.Core.Local.Parsing;
using Xunit;
using Xunit.Abstractions;

namespace MangaMagnet.Core.Test.Local.Parsing;

[TestSubject(typeof(MangaFileNameParser))]
public class MangaFileNameParserTest(ITestOutputHelper output)
{
	private readonly MangaFileNameParser _parser = new(new XunitLogger<MangaFileNameParser>(output));

	[Theory]
	[InlineData("Title v08 (CM)")]
	[InlineData("Title - A Longer Title v01 (2014) (Digital) (danke-Empire)")]
	[InlineData("[PZG] Title!, Vol. 01 (Audiobook) [Podium Audio]")]
	public void Parse_ShouldReturnParseReleaseTypeVolume_WhenReleaseTypeIsVolume(string input)
		=> AssertParseType(input, ParsedReleaseType.VOLUME);

	[Theory]
	[InlineData("A Very Long Title 408 (2023) (Digital) (LuCaZ)")]
	[InlineData("Title 001")]
	[InlineData("Title c000 (2019) (Digital) (Dalte).cbz")]
	public void Parse_ShouldReturnParseReleaseTypeChapter_WhenReleaseTypeIsChapter(string input)
		=> AssertParseType(input, ParsedReleaseType.CHAPTER);

	[Theory]
	[InlineData("Title - A Longer Title v09 (2024) (Digital) (Ushi)", 2024)]
	[InlineData("Title v01 (2024) (Digital) (1r0n)", 2024)]
	[InlineData("[0v3r] Title v15 (2023) (Digital) (0v3r)", 2023)]
	public void Parse_ShouldReturnYear_WhenReleaseContainsYear(string input, int year)
		=> AssertYear(input, year);

	[Theory]
	[InlineData("Title v08 (CM)", "CM")]
	[InlineData("Title - A Longer Title v01 (2014) (Digital) (danke-Empire)", "danke-Empire")]
	[InlineData("[PZG] Title!, Vol. 01 (Audiobook) [Podium Audio]", "PZG")]
	public void Parse_ShouldReturnReleaseGroup_WhenReleaseGroupIsPresent(string input, string expected)
		=> AssertReleaseGroup(input, expected);

	[Theory]
	[InlineData("Title - A Longer Title v01 (2014) (Digital) (danke-Empire)")]
	[InlineData("Title v01 (2024) (Digital) (1r0n)")]
	[InlineData("Title (Alias) c000 (2013) (Digital) (Dalte).cbz")]
	[InlineData("A Very Long Title c001 (2017) (Digital) (Dalte)")]
	public void Parse_ShouldReturnDigital_WhenReleaseIsDigital(string input)
		=> AssertDigital(input, true);

	private void AssertDigital(string input, bool expected)
	{
		var result = _parser.Parse(input);

		Assert.Equal(expected, result.IsDigital);
	}

	private void AssertChapterNumber(string input, double expected)
	{
		var result = _parser.Parse(input);

		Assert.Equal(expected, result.ChapterNumber);
	}

	private void AssertVolumeNumber(string input, int? expected)
	{
		var result = _parser.Parse(input);

		Assert.Equal(expected, result.VolumeNumber);
	}

	private void AssertYear(string input, int? expected)
	{
		var result = _parser.Parse(input);

		Assert.Equal(expected, result.Year);
	}

	private void AssertParseType(string input, ParsedReleaseType expected)
	{
		var result = _parser.Parse(input);

		Assert.Equal(expected, result.ParsedType);
	}

	private void AssertReleaseGroup(string input, string expected)
	{
		var result = _parser.Parse(input);

		Assert.Equal(expected, result.ReleaseGroup);
	}
}
