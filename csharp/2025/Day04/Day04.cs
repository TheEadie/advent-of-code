namespace AdventOfCode2025.Day04;

public class Day04
{
    // https://adventofcode.com/2025/day/4
    private readonly AdventSession _session = new(2025, 04, "Printing Department");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 13)]
    [TestCase("Puzzle Input.txt", 1578)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var initialRolls = ParseInput(input);
        var finalRolls = RunRemovalProcess(initialRolls).First();

        var answer = initialRolls.Count - finalRolls.Count;

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 43)]
    [TestCase("Puzzle Input.txt", 10132)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var initialRolls = ParseInput(input);
        var finalRolls = RunRemovalProcess(initialRolls).Last();

        var answer = initialRolls.Count - finalRolls.Count;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<HashSet<Coordinate>> RunRemovalProcess(HashSet<Coordinate> rolls)
    {
        var currentRolls = rolls;
        while (true)
        {
            var newRolls = RemoveRolls(currentRolls);
            yield return newRolls;

            if (newRolls.SetEquals(currentRolls))
            {
                yield break;
            }

            currentRolls = newRolls;
        }
    }

    private static HashSet<Coordinate> RemoveRolls(HashSet<Coordinate> rolls) =>
        rolls.Where(x => Vector.EightDirections().Count(v => rolls.Contains(x + v)) >= 4).ToHashSet();

    private static HashSet<Coordinate> ParseInput(string input)
    {
        var rolls = new HashSet<Coordinate>();

        var lines = input.Split("\n");
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '@')
                {
                    rolls.Add(new Coordinate(x, y));
                }
            }
        }

        return rolls;
    }
}
