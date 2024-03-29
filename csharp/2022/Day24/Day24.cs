namespace AdventOfCode2022.Day24;

public class Day24
{
    private readonly AdventSession _session = new(2022, 24, "Blizzard Basin");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 18)]
    [TestCase("Puzzle Input.txt", 251)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (start, goal, map, storms) = ParseInput(input);

        var (answer, _) = PathFinding.AStar(
            new State(0, start),
            (n) => n.Position == goal,
            (n) => GetMoves(n, map, storms),
            (_, _) => 1,
            (n) => Math.Abs(n.Position.X - goal.X) + Math.Abs(n.Position.Y - goal.Y));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 54)]
    [TestCase("Puzzle Input.txt", 758)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (start, goal, map, storms) = ParseInput(input);

        var (one, _) = PathFinding.AStar(
            new State(0, start),
            (n) => n.Position == goal,
            (n) => GetMoves(n, map, storms),
            (_, _) => 1,
            (n) => Math.Abs(n.Position.X - goal.X) + Math.Abs(n.Position.Y - goal.Y));

        await TestContext.Progress.WriteLineAsync($"There: {one}");

        var (two, _) = PathFinding.AStar(
            new State(one, goal),
            (n) => n.Position == start,
            (n) => GetMoves(n, map, storms),
            (_, _) => 1,
            (n) => Math.Abs(n.Position.X - start.X) + Math.Abs(n.Position.Y - start.Y));

        await TestContext.Progress.WriteLineAsync($"Back: {two}");

        var (three, _) = PathFinding.AStar(
            new State(one + two, start),
            (n) => n.Position == goal,
            (n) => GetMoves(n, map, storms),
            (_, _) => 1,
            (n) => Math.Abs(n.Position.X - goal.X) + Math.Abs(n.Position.Y - goal.Y));

        await TestContext.Progress.WriteLineAsync($"Back again: {three}");

        var answer = one + two + three;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<State> GetMoves(State input,
                        IReadOnlySet<Coordinate> map,
                        IReadOnlyList<HashSet<Coordinate>> storms)
    {
        var (time, current) = input;

        var testCoordinates = new HashSet<Coordinate>
        {
            new(current.X - 1, current.Y),
            new(current.X, current.Y - 1),
            new(current.X, current.Y + 1),
            new(current.X + 1, current.Y),
            current
        };

        var stormsAtTime = storms[time];

        return testCoordinates.Where(map.Contains).Where(x => !stormsAtTime.Contains(x)).Select(x => new State(time + 1, x));
    }

    private record State(int Time, Coordinate Position);

    private static (Coordinate, Coordinate, HashSet<Coordinate>, HashSet<Coordinate>[]) ParseInput(string input)
    {
        var storms = new Dictionary<Coordinate, Vector>();
        var lines = input.Split("\n");
        var rows = lines
            .Select(line => line.ToCharArray().ToList())
            .ToList();

        var sizeY = rows.Count;
        var sizeX = rows[0].Count;

        for (var y = 0; y < sizeY; y++)
        {
            for (var x = 0; x < sizeX; x++)
            {
                switch (rows[y][x])
                {
                    case '>':
                        storms.Add(new Coordinate(x - 1, y - 1), Vector.Right);
                        break;
                    case '<':
                        storms.Add(new Coordinate(x - 1, y - 1), Vector.Left);
                        break;
                    case '^':
                        storms.Add(new Coordinate(x - 1, y - 1), Vector.Up);
                        break;
                    case 'v':
                        storms.Add(new Coordinate(x - 1, y - 1), Vector.Down);
                        break;
                    case '.':
                    case '#':
                        break;
                    default:
                        break;
                }
            }
        }

        var width = sizeX - 2;
        var height = sizeY - 2;

        var map = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).Select(x => new Coordinate(x, y)))
            .Append(new Coordinate(0, -1))
            .Append(new Coordinate(width - 1, height))
            .ToHashSet();

        var stormsOverTime = new HashSet<Coordinate>[1000];

        for (var i = 0; i < 1000; i++)
        {
            stormsOverTime[i] = storms
                .Select(x => new Coordinate(
                    (int) Mod(x.Key.X + x.Value.X * (i + 1), width),
                    (int) Mod(x.Key.Y + x.Value.Y * (i + 1), height)))
                    .ToHashSet();
        }

        var start = new Coordinate(0, -1);
        var goal = new Coordinate(width - 1, height);


        return (start, goal, map, stormsOverTime);
    }

    private static double Mod(double x, double m) => x - m * Math.Floor(x / m);
}
