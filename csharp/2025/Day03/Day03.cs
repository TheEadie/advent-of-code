namespace AdventOfCode2025.Day03;

public class Day03
{
    // https://adventofcode.com/2025/day/3
    private readonly AdventSession _session = new(2025, 03, "Lobby");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 357)]
    [TestCase("Puzzle Input.txt", 17_408)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input).Select(x => GetJoltage(x, 2)).Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 3_121_910_778_619)]
    [TestCase("Puzzle Input.txt", 172_740_584_266_849)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input).Select(x => GetJoltage(x, 12)).Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static long GetJoltage(int[] bank, int batteriesToSwitchOn)
    {
        var total = "";
        var startIndex = 0;

        for (var b = 0; b < batteriesToSwitchOn; b++)
        {
            var (digit, position) = bank[startIndex..^(batteriesToSwitchOn - b - 1)]
                .Select((x, i) => (x, i))
                .MaxBy(x => x.x);
            total += digit;
            startIndex += position + 1;
        }

        return long.Parse(total);
    }

    private static IReadOnlyList<int[]> ParseInput(string input) =>
        input.Split("\n").Select(line => line.Select(c => c - '0').ToArray()).ToList();
}
