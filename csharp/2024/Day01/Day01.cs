namespace AdventOfCode2024.Day01;

public class Day01
{
    // https://adventofcode.com/2024/day/1
    private readonly AdventSession _session = new(2024, 01, "Historian Hysteria");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 11)]
    [TestCase("Puzzle Input.txt", 936_063)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (left, right) = ParseInput(input);

        var answer = left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 31)]
    [TestCase("Puzzle Input.txt", 23_150_395)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (left, right) = ParseInput(input);

        var rightCount = right.CountBy(x => x).ToDictionary();

        var answer = left.Select(l => l * rightCount.GetValueOrDefault(l, 0)).Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (List<int> Left, List<int> Right) ParseInput(string input)
    {
        var sections = input.Split("\n")
            .Select(x => x.Split("   "))
            .Select(x => (Left: int.Parse(x[0]), Right: int.Parse(x[1])))
            .ToList();

        var left = sections.Select(x => x.Left).Order().ToList();
        var right = sections.Select(x => x.Right).Order().ToList();
        return (left, right);
    }
}
