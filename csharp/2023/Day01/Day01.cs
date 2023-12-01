using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day01;

public class Day01
{
    // https://adventofcode.com/2023/day/1
    private readonly AdventSession _session = new(2023, 01, "Trebuchet?!");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 142)]
    [TestCase("Puzzle Input.txt", 55_090)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = GetAnswer(input, @"\d");

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample-2.txt", 281)]
    [TestCase("Puzzle Input.txt", 54_845)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = GetAnswer(input, @"\d|one|two|three|four|five|six|seven|eight|nine");

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static int GetAnswer(string input, string regex) =>
        input.Split("\n")
            .Select(x =>
                {
                    var first = GetNumber(Regex.Match(x, regex).Value);
                    var last = GetNumber(Regex.Match(x, regex, RegexOptions.RightToLeft).Value);
                    return $"{first}{last}";
                })
            .Select(int.Parse)
            .Sum();

    private static int GetNumber(string input) =>
        input switch
        {
            "one" or "1" => 1,
            "two" or "2" => 2,
            "three" or "3" => 3,
            "four" or "4" => 4,
            "five" or "5" => 5,
            "six" or "6" => 6,
            "seven" or "7" => 7,
            "eight" or "8" => 8,
            "nine" or "9" => 9,
            _ => throw new ArgumentOutOfRangeException(nameof(input))
        };
}
