namespace AdventOfCode2022.Day12;

public class Day12
{
    private readonly AdventSession _session = new(2022, 12, "Hill Climbing Algorithm");

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", 31)]
    [TestCase("Puzzle Input.txt", 425)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (start, goal, map) = ParseInput(input);

        var (answer, _) =
            PathFinding.AStar(start,
                n => n == goal,
                n => GetNeighbours(n, map),
                (_, _) => 1,
                n => DistanceToGoal(n, goal));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 29)]
    [TestCase("Puzzle Input.txt", 418)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (_, goal, map) = ParseInput(input);

        var answer = map.Where(x => x.Value == 0)
            .Select(x =>
                PathFinding.AStar(x.Key,
                    n => n == goal,
                    n => GetNeighbours(n, map),
                    (_, _) => 1,
                    n => DistanceToGoal(n, goal))
            )
            .Min(x => x.Item1);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (Coordinate, Coordinate, Dictionary<Coordinate, int>) ParseInput(string input)
    {
        var map = new Dictionary<Coordinate, int>();
        var lines = input.Split("\n");
        var rows = lines
            .Select(line => line.ToCharArray()
                .Select(x => x - 'a').ToList())
            .ToList();

        var sizeX = rows.Count;
        var sizeY = rows[0].Count;

        Coordinate start = null!;
        Coordinate goal = null!;

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                if (rows[x][y] == -14)
                {
                    start = new Coordinate(x, y);
                    rows[x][y] = 0;
                }

                if (rows[x][y] == -28)
                {
                    goal = new Coordinate(x, y);
                    rows[x][y] = 25;
                }
                map.Add(new Coordinate(x, y), rows[x][y]);
            }
        }

        return (start, goal, map);
    }

    private static int DistanceToGoal(Coordinate current, Coordinate goal)
    {
        return (goal.X - current.X) + (goal.Y - current.Y);
    }

    private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, IReadOnlyDictionary<Coordinate, int> map)
    {
        var testCoordinates = new List<Coordinate>
        {
            new(input.X - 1, input.Y),
            new(input.X, input.Y - 1),
            new(input.X, input.Y + 1),
            new(input.X + 1, input.Y)
        };

        return testCoordinates
            .Where(map.ContainsKey)
            .Where(x => map[x] - map[input] <= 1);
    }
}