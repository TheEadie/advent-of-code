namespace AdventOfCode2024.Day02;

public class Day02
{
    // https://adventofcode.com/2024/day/2
    private readonly AdventSession _session = new(2024, 02, "Red-Nosed Reports");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 2)]
    [TestCase("Puzzle Input.txt", 218)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input).Count(ValidReport);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 4)]
    [TestCase("Puzzle Input.txt", 290)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input).Select(GetAllPossibleReports).Count(x => x.Any(ValidReport));

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Report> ParseInput(string input) =>
        input.Split("\n").Select(x => new Report(x.Split(" ").Select(int.Parse).ToList()));

    private static bool ValidReport(Report report)
    {
        var diffs = report.Zip(report.Skip(1)).Select(z => z.First - z.Second).ToList();
        return diffs.All(n => n is > 0 and <= 3) || diffs.All(n => n is < 0 and >= -3);
    }

    private static IEnumerable<Report> GetAllPossibleReports(Report report) =>
        report.Select((_, ind) => new Report(report.Where((_, i) => i != ind).ToList()));

    private class Report(IEnumerable<int> collection) : List<int>(collection);
}
