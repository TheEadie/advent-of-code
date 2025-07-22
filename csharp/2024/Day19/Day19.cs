namespace AdventOfCode2024.Day19;

public class Day19
{
    // https://adventofcode.com/2024/day/19
    private readonly AdventSession _session = new(2024, 19, "Linen Layout");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 6)]
    [TestCase("Puzzle Input.txt", 293)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (patterns, designs) = ParseInput(input);

        var answer = designs.Count(design => CanMakeDesign(design, patterns));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 16)]
    [TestCase("Puzzle Input.txt", 623924810770264)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var (patterns, designs) = ParseInput(input);

        var answer = designs.Sum(design => CountWaysToMakeDesign(design, patterns));

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (List<string> patterns, List<string> designs) ParseInput(string input)
    {
        var lines = input.Split('\n');
        var patterns = lines[0].Split(", ").ToList();
        var designs = lines.Skip(2).ToList();
        return (patterns, designs);
    }

    private static bool CanMakeDesign(string design, List<string> patterns, Dictionary<string, bool>? memo = null)
    {
        memo ??= new Dictionary<string, bool>();

        if (design.Length == 0)
        {
            return true;
        }

        if (memo.TryGetValue(design, out var canMake))
        {
            return canMake;
        }

        var possible = patterns.Where(design.StartsWith)
            .Any(pattern => CanMakeDesign(design[pattern.Length..], patterns, memo));

        memo[design] = possible;
        return possible;
    }

    private static long CountWaysToMakeDesign(string design, List<string> patterns, Dictionary<string, long>? memo = null)
    {
        memo ??= new Dictionary<string, long>();

        if (design.Length == 0)
        {
            return 1;
        }

        if (memo.TryGetValue(design, out var countWays))
        {
            return countWays;
        }

        var ways = patterns.Where(design.StartsWith)
            .Sum(pattern => CountWaysToMakeDesign(design[pattern.Length..], patterns, memo));

        memo[design] = ways;
        return ways;
    }
}
