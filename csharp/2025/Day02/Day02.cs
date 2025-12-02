namespace AdventOfCode2025.Day02;

public class Day02
{
    // https://adventofcode.com/2025/day/2
    private readonly AdventSession _session = new(2025, 02, "Gift Shop");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 1_227_775_554)]
    [TestCase("Puzzle Input.txt", 31_000_881_061)]
    public async Task Part1(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input)
            .SelectMany(r => LongEnumerable.Range(r.Item1, r.Item2 - r.Item1 + 1))
            .Where(x => NumberRepeats(x, 2))
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 4_174_379_265)]
    [TestCase("Puzzle Input.txt", 46_769_308_485)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input)
            .SelectMany(r => LongEnumerable.Range(r.Item1, r.Item2 - r.Item1 + 1))
            .Where(number => Enumerable.Range(2, number.ToString().Length - 1).Any(x => NumberRepeats(number, x)))
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static bool NumberRepeats(long number, int times)
    {
        var str = number.ToString();
        if (str.Length % times != 0)
        {
            return false;
        }

        var partLength = str.Length / times;
        var firstPart = str[..partLength];

        return Enumerable.Range(1, times - 1).All(x => str[(x * partLength)..((x + 1) * partLength)] == firstPart);
    }

    private static List<(long, long)> ParseInput(string input) =>
        input.Split(",")
            .Select(s =>
                {
                    var parts = s.Split("-");
                    return (long.Parse(parts[0]), long.Parse(parts[1]));
                })
            .ToList();
}
