using System.Text.RegularExpressions;

namespace AdventOfCode2015.Day08;

public class Day08
{
    private readonly AdventSession _session = new(2015, 8, "Matchsticks");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 12)]
    [TestCase("Puzzle Input.txt", 1371)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var strings = input.Split("\n")
            .Select(x => new { Original = x, Evaluated = EvalString(x) });

        var answer = strings.Sum(x => x.Original.Length - x.Evaluated.Length);
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 19)]
    [TestCase("Puzzle Input.txt", 2117)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var strings = input.Split("\n")
            .Select(x => new { Original = x, Escaped = EscapeString(x) });

        var answer = strings.Sum(x => x.Escaped.Length - x.Original.Length);
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private string EvalString(string input) =>
        Regex.Replace(input, @"\\x[0-f]{2}", "X")
            .TrimStart('"')
            .TrimEnd('"')
            .Replace(@"\\", @"\")
            .Replace(@"\""", @"""");

    private string EscapeString(string input) =>
        $"\"{input.Replace(@"\", @"\\").Replace(@"""", @"\""")}\"";
}
