namespace AdventOfCode2024.Day15;

public class Day15
{
    // https://adventofcode.com/2024/day/15
    private readonly AdventSession _session = new(2024, 15, "Warehouse Woes");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 10_092)]
    [TestCase("Sample2.txt", 2_028)]
    [TestCase("Puzzle Input.txt", 1_505_963)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (map, moves) = Parse(input);

        var answer = GetGpsTotal(Run(map, moves).Last());

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 9_021)]
    [TestCase("Sample3.txt", 618)]
    [TestCase("Puzzle Input.txt", 1_543_141)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var inputScaled = ScaleUp(input);
        var (map, moves) = Parse(inputScaled);

        var answer = GetGpsTotal(Run(map, moves).Last());

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Map> Run(Map map, List<Vector> moves)
    {
        foreach (var vector in moves)
        {
            var robot = map.First(c => c.Value == ObjectType.Robot).Key;
            yield return CanMove(robot, vector, map) ? Move(robot, vector, map) : map;
        }
    }

    private static int GetGpsTotal(Map map) =>
        map.Where(x => x.Value is ObjectType.Box or ObjectType.BoxLeft).Sum(x => x.Key.Y * 100 + x.Key.X);

    private static bool CanMove(Coordinate current, Vector vector, Map map)
    {
        var newPos = current + vector;

        return map[newPos] switch
        {
            ObjectType.Wall => false,
            ObjectType.Box => CanMove(newPos, vector, map),
            ObjectType.BoxLeft => CanMove(newPos, vector, map)
                && (vector == Vector.Left || CanMove(newPos + Vector.Right, vector, map)),
            ObjectType.BoxRight => CanMove(newPos, vector, map)
                && (vector == Vector.Right || CanMove(newPos + Vector.Left, vector, map)),
            ObjectType.Empty => true,
            ObjectType.Robot => throw new InvalidOperationException("Robot cannot be in two places at once"),
            _ => throw new NotSupportedException($"Unknown object: {map[newPos]}")
        };
    }

    private static Map Move(Coordinate current, Vector vector, Map map)
    {
        var type = map[current];
        var newPos = current + vector;

        switch (map[newPos])
        {
            case ObjectType.Wall:
                return map;
            case ObjectType.Box:
                map = Move(newPos, vector, map);
                map[current] = ObjectType.Empty;
                map[newPos] = type;
                return map;
            case ObjectType.BoxLeft:
                map = Move(newPos, vector, map);
                map = vector != Vector.Right ? Move(newPos + Vector.Right, vector, map) : map;
                map[current] = ObjectType.Empty;
                map[newPos] = type;
                return map;
            case ObjectType.BoxRight:
                map = Move(newPos, vector, map);
                map = vector != Vector.Left ? Move(newPos + Vector.Left, vector, map) : map;
                map[current] = ObjectType.Empty;
                map[newPos] = type;
                return map;
            case ObjectType.Empty:
                map[current] = ObjectType.Empty;
                map[newPos] = type;
                return map;
            case ObjectType.Robot:
                throw new InvalidOperationException("Robot cannot be in two places at once");
            default:
                throw new NotSupportedException($"Unknown object: {map[newPos]}");
        }
    }

    private static string ScaleUp(string input) =>
        input.Replace(".", "..").Replace("@", "@.").Replace("O", "[]").Replace("#", "##");

    private static (Map map, List<Vector> moves) Parse(string input)
    {
        var sections = input.Split("\n\n");

        var lines = sections[0].Split("\n");
        var map = new Map();

        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                var pos = new Coordinate(x, y);
                map.Add(
                    pos,
                    c switch
                    {
                        '#' => ObjectType.Wall,
                        'O' => ObjectType.Box,
                        '[' => ObjectType.BoxLeft,
                        ']' => ObjectType.BoxRight,
                        '@' => ObjectType.Robot,
                        '.' => ObjectType.Empty,
                        _ => throw new NotSupportedException($"Unknown object: {c}")
                    });
            }
        }

        var moves = sections[1]
            .ReplaceLineEndings("")
            .Select(
                x => x switch
                {
                    '^' => Vector.Up,
                    'v' => Vector.Down,
                    '<' => Vector.Left,
                    '>' => Vector.Right,
                    _ => throw new NotSupportedException($"Unknown move: {x}")
                });

        return (map, moves.ToList());
    }

    private class Map : Dictionary<Coordinate, ObjectType>;

    private enum ObjectType
    {
        Wall,
        Box,
        Robot,
        Empty,
        BoxLeft,
        BoxRight
    }
}
