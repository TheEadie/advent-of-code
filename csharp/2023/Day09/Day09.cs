namespace AdventOfCode2023.Day09;

public class Day09
{
    // https://adventofcode.com/2023/day/9
    private readonly AdventSession _session = new(2023, 09, "Mirage Maintenance");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 114)]
    [TestCase("Puzzle Input.txt", 1_725_987_467)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = Parse(input)
            .Select(GetTree)
            .Select(x => x.Select(d => d.Last()))
            .Select(x => x.Sum())
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 2)]
    [TestCase("Puzzle Input.txt", 971)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = Parse(input)
            .Select(GetTree)
            .Select(x => x.Select(d => d.First()))
            .Select(x => x.Reverse().Aggregate((acc, d) => d - acc))
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<IList<int>> Parse(string input) =>
        input.Split("\n").Select(x => x.Split(" ").Select(int.Parse).ToList());

    private static IEnumerable<IEnumerable<int>> GetTree(IList<int> sequence)
    {
        while (sequence.Any(x => x != 0))
        {
            yield return sequence;
            sequence = sequence.Skip(1).Select((x, i) => x - sequence.ElementAt(i)).ToList();
        }
    }
}
