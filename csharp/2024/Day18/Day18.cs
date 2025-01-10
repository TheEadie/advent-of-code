namespace AdventOfCode2024.Day18;

public class Day18
{
    // https://adventofcode.com/2024/day/18
    private readonly AdventSession _session = new(2024, 18, "RAM Run");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 22, 6, 12)]
    [TestCase("Puzzle Input.txt", 356, 70, 1024)]
    public async Task Part1(string inputFile, int expected, int gridSize, int bytes)
    {
        var input = await _session.Start(inputFile);
        var positions = Parse(input).Take(bytes).ToHashSet();

        var (answer, _) = PathFinding.AStar(
            new Coordinate(0, 0),
            x => x == new Coordinate(gridSize, gridSize),
            x => Vector.FourDirections()
                .Select(v => x + v)
                .Where(n => !positions.Contains(n) && n.X >= 0 && n.Y >= 0 && n.X <= gridSize && n.Y <= gridSize)
                .ToList(),
            (_, _) => 1,
            _ => 0);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Coordinate> Parse(string input) =>
        input.Split("\n").Select(x => x.Split(",")).Select(x => new Coordinate(int.Parse(x[0]), int.Parse(x[1])));

    [TestCase("Sample.txt", 6, 1, 6)]
    [TestCase("Puzzle Input.txt", 22, 33, 70)]
    public async Task Part2(string inputFile, int expectedX, int expectedY, int gridSize)
    {
        var input = await _session.Start(inputFile);
        var allBytes = Parse(input).ToList();
        var answer = new Coordinate(0, 0);
        var goal = new Coordinate(gridSize, gridSize);
        var currentPath = new HashSet<Coordinate> { allBytes[0] };

        foreach (var current in allBytes)
        {
            if (!currentPath.Contains(current))
            {
                continue;
            }

            var positions = allBytes.TakeUntil(x => x == current).ToHashSet();
            var (_, path) = PathFinding.AStar(
                new Coordinate(0, 0),
                x => x == goal,
                x => Vector.FourDirections()
                    .Select(v => x + v)
                    .Where(n => !positions.Contains(n) && n.X >= 0 && n.Y >= 0 && n.X <= gridSize && n.Y <= gridSize)
                    .ToList(),
                (_, _) => 1,
                x => Coordinate.ManhattanDistance(x, goal));
            currentPath = path.ToHashSet();

            if (currentPath.Count == 0)
            {
                answer = current;
                break;
            }
        }

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(new Coordinate(expectedX, expectedY));
    }
}
