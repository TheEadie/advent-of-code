namespace AdventOfCode2024.Day16;

public class Day16
{
    // https://adventofcode.com/2024/day/16
    private readonly AdventSession _session = new(2024, 16, "Reindeer Maze");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 7_036)]
    [TestCase("Sample2.txt", 11_048)]
    [TestCase("Puzzle Input.txt", 90_440)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (path, start, finish) = Parse(input);

        var (answer, _) = PathFinding.AStar(
            start,
            x => x.Position == finish,
            x => GetNeighbours(x, path),
            (a, b) => a.Position != b.Position ? 1 : 1000,
            c => Coordinate.ManhattanDistance(c.Position, finish));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 45, 7_036)]
    [TestCase("Sample2.txt", 64, 11_048)]
    [TestCase("Puzzle Input.txt", 479, 90_440)]
    public async Task Part2(string inputFile, int expected, int bestCost)
    {
        var input = await _session.Start(inputFile);
        var (path, start, finish) = Parse(input);

        var answer = PathFinding.AStarMultiplePaths(
                start,
                x => x.Position == finish,
                x => GetNeighbours(x, path),
                (a, b) => a.Position != b.Position ? 1 : 1000,
                c => Coordinate.ManhattanDistance(c.Position, finish))
            .TakeWhile(x => x.cost <= bestCost)
            .SelectMany(x => x.path)
            .Select(x => x.Position)
            .Distinct()
            .Count();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static void PrintPath(List<Coordinate> path)
    {
        var maxX = path.Max(x => x.X);
        var maxY = path.Max(x => x.Y);

        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var c = path.FirstOrDefault(p => p.X == x && p.Y == y);
                Console.Write(c == null ? '#' : '.');
            }

            Console.WriteLine();
        }
    }


    private static IEnumerable<Reindeer> GetNeighbours(Reindeer reindeer, HashSet<Coordinate> path)
    {
        var newPosition = reindeer.Position + reindeer.Direction;
        if (path.Contains(newPosition))
        {
            yield return reindeer with { Position = newPosition };
        }

        yield return reindeer with { Direction = reindeer.Direction.TurnLeft() };
        yield return reindeer with { Direction = reindeer.Direction.TurnRight() };
    }

    private static (HashSet<Coordinate> path, Reindeer start, Coordinate finish) Parse(string input)
    {
        var lines = input.Split("\n");
        var map = lines.SelectMany(
                (str, y) => str.ToCharArray().Select((c, x) => (Position: new Coordinate(x, y), Value: c)))
            .ToDictionary(x => x.Position, x => x.Value);

        var path = map.Where(x => x.Value != '#').Select(x => x.Key).ToHashSet();
        var start = map.First(x => x.Value == 'S').Key;
        var finish = map.First(x => x.Value == 'E').Key;

        return (path, new Reindeer(start, Vector.Right), finish);
    }

    private record Reindeer(Coordinate Position, Vector Direction);
}
