namespace AdventOfCode2024.Day11;

public class Day11
{
    // https://adventofcode.com/2024/day/11
    private readonly AdventSession _session = new(2024, 11, "Plutonian Pebbles");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 55_312)]
    [TestCase("Puzzle Input.txt", 216_042)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var stones = Parse(input).GroupBy(x => x).ToDictionary(x => x.Key, x => (long) x.Count());

        var result = Blink(stones).Take(25);
        var answer = result.Last().Sum(x => x.Value);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 255_758_646_442_399)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var stones = Parse(input).GroupBy(x => x).ToDictionary(x => x.Key, x => (long) x.Count());

        var result = Blink(stones).Take(75);
        var answer = result.Last().Sum(x => x.Value);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Dictionary<long, long>> Blink(Dictionary<long, long> stones)
    {
        while (true)
        {
            stones = RunStep(stones);
            yield return stones;
        }
    }

    private static Dictionary<long, long> RunStep(Dictionary<long, long> previous)
    {
        var next = new Dictionary<long, long>();
        foreach (var n in previous.Keys)
        {
            if (n == 0)
            {
                next[1] = next.GetValueOrDefault(1) + previous[0];
            }
            else if (n.ToString().Length % 2 == 0)
            {
                var s = n.ToString();
                var first = long.Parse(s[..(s.Length / 2)]);
                var second = long.Parse(s[(s.Length / 2)..]);
                next[first] = next.GetValueOrDefault(first) + previous[n];
                next[second] = next.GetValueOrDefault(second) + previous[n];
            }
            else
            {
                next[n * 2024] = next.GetValueOrDefault(n * 2024) + previous[n];
            }
        }

        return next;
    }

    private static IEnumerable<long> Parse(string input) => input.Split(' ').Select(long.Parse);
}
