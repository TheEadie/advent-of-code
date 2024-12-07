namespace AdventOfCode2024.Day06;

public class Day06
{
    // https://adventofcode.com/2024/day/6
    private readonly AdventSession _session = new(2024, 06, "Guard Gallivant");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 41)]
    [TestCase("Puzzle Input.txt", 4_778)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (obstacles, start, bottomCorner) = Parse(input);
        var (visited, _) = GetPath(start, bottomCorner, Vector.Up, obstacles);

        var answer = visited.Count;

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 6)]
    [TestCase("Puzzle Input.txt", 1_618)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (obstacles, start, bottomCorner) = Parse(input);
        var (visited, _) = GetPath(start, bottomCorner, Vector.Up, obstacles);

        var answer = visited.Count(
            x => GetPath(start, bottomCorner, Vector.Up, obstacles.Append(x).ToHashSet()).IsLoop);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (HashSet<Coordinate> Positions, bool IsLoop) GetPath(Coordinate position, Coordinate bottomCorner, Vector direction,
        HashSet<Coordinate> obstacles)
    {
        var visited = new HashSet<(Coordinate, Vector)>();
        while(IsInBounds(position, bottomCorner))
        {
            if(!visited.Add((position, direction)))
            {
                return (visited.Select(x => x.Item1).ToHashSet(), true);
            }

            var testPosition = position + direction;

            if (obstacles.Contains(testPosition))
            {
                direction = direction.TurnRight();
                continue;
            }
            position += direction;
        }

        return (visited.Select(x => x.Item1).ToHashSet(), false);
    }

    private static bool IsInBounds(Coordinate coordinate, Coordinate bottomCorner) =>
        coordinate is { X: >= 0, Y: >= 0 } &&
        coordinate.X <= bottomCorner.X &&
        coordinate.Y <= bottomCorner.Y;

    private static (HashSet<Coordinate> obstacles, Coordinate start, Coordinate bottomCorner) Parse(string input)
    {
        var map = input.Split("\n")
            .SelectMany((str, y) => str.ToCharArray().Select((c, x) => (Position: new Coordinate(x, y), Value: c)))
            .ToList();
        var boxes = map
            .Where(x => x.Value == '#')
            .Select(x => x.Position)
            .ToHashSet();
        var start = map.Where(x => x.Value == '^').Select(x => x.Position).First();
        var bottomCorner = new Coordinate(map.Max(x => x.Position.X), map.Max(x => x.Position.Y));
        return (boxes, start, bottomCorner);
    }
}
