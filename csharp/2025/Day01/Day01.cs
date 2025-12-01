namespace AdventOfCode2025.Day01;

public class Day01
{
    // https://adventofcode.com/2025/day/1
    private readonly AdventSession _session = new(2025, 01, "Secret Entrance");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 3)]
    [TestCase("Puzzle Input.txt", 1_123)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var rotations = ParseInput(input);

        var answer = Run(50, rotations).Count(x => x.Position == 0);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 6)]
    [TestCase("Puzzle Input.txt", 6_695)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var rotations = ParseInput(input);

        var result = Run(50, rotations).ToList();
        var answer = result.Sum(x => x.Zeros) + result.Count(x => x.Position == 0);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static List<Rotation> ParseInput(string input) =>
        input.Split("\n")
            .Select(line =>
                {
                    var direction = line[0] == 'R';
                    var steps = int.Parse(line[1..]);
                    return new Rotation(direction, steps);
                })
            .ToList();

    private static IEnumerable<(int Position, int Zeros)> Run(int startPosition, List<Rotation> rotations)
    {
        var previous = startPosition;
        foreach (var rotation in rotations)
        {
            var newPosition = rotation.Right ? previous + rotation.Steps : previous - rotation.Steps;
            var crosses = (Math.Abs(newPosition) - 1) / 100;
            if (previous < 0 && newPosition > 0 || previous > 0 && newPosition < 0)
            {
                crosses++;
            }

            newPosition = (newPosition + 100) % 100;
            yield return (newPosition, crosses);

            previous = newPosition;
        }
    }

    private record Rotation(bool Right, int Steps);
}
