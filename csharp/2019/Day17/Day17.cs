namespace AdventOfCode2019.Day17;

public class Day17
{
    private readonly AdventSession _session = new(2019, 17, "Set and Forget");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 8928)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();
        var emulator = new IntCode.IntCode(program);
        var map = ParseMap(emulator);

        var points = map.Keys.Where(x => GetNeighbours(x).All(n => map.ContainsKey(n) && map[n] == '#'));

        var answer = points.Sum(x => x.X * x.Y);
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 880360)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();
        var emulator = new IntCode.IntCode(program) { Memory = { [0] = 2 } };
        var map = ParseMap(emulator);
        var robot = map.Single(x => x.Value == '^').Key;

        var path = GetPath(robot, map);

        //Console.WriteLine(path);

        var route = new[]
        {
            "A,B,A,B,A,C,B,C,A,C",
            "L,6,R,12,L,6",
            "R,12,L,10,L,4,L,6",
            "L,10,L,10,L,4,L,6",
            "n"
        };

        IEnumerable<long> outputs = new List<long>();

        foreach (var line in route)
        {
            foreach (var c in line)
            {
                _ = emulator.Run(c);
            }

            (_, outputs) = emulator.Run(10);
        }

        var answer = outputs.Last();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static string GetPath(Coordinate robot, IReadOnlyDictionary<Coordinate, char> map)
    {
        var path = "";
        var current = robot;
        var direction = Vector.Up;
        var steps = 0;

        while (true)
        {
            var next = current + direction;
            if (map.ContainsKey(next) && map[next] == '#')
            {
                steps++;
                current = next;
                continue;
            }

            if (steps > 0)
            {
                path += steps.ToString() + ",";
                steps = 0;
            }

            var left = direction.TurnLeft();
            var right = direction.TurnRight();

            if (map.ContainsKey(current + left) && map[current + left] == '#')
            {
                path += "L" + ",";
                direction = left;
                continue;
            }

            if (map.ContainsKey(current + right) && map[current + right] == '#')
            {
                path += "R" + ",";
                direction = right;
                continue;
            }

            break;
        }

        return path;
    }

    private static IEnumerable<Coordinate> GetNeighbours(Coordinate input)
    {
        yield return input;
        yield return input with { Y = input.Y - 1 };
        yield return input with { Y = input.Y + 1 };
        yield return input with { X = input.X - 1 };
        yield return input with { X = input.X + 1 };
    }

    private static IReadOnlyDictionary<Coordinate, char> ParseMap(IntCode.IntCode emulator)
    {
        var (_, outputs) = emulator.Run();

        var map = new Dictionary<Coordinate, char>();
        var x = 0;
        var y = 0;
        foreach (var output in outputs)
        {
            if (output == 10)
            {
                y++;
                x = 0;
                continue;
            }

            map[new Coordinate(x, y)] = (char) output;
            x++;
        }

        return map;
    }
}
