namespace AdventOfCode2022;

public class Day12
{
    [TestCase("data/12 - Sample.txt", 31, TestName = "Day 12 - Part 1 - Sample")]
    [TestCase("data/12 - Puzzle Input.txt", 425, TestName = "Day 12 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var (start, goal, map) = ParseInput(File.ReadAllText(inputFile));

        var (answer, path) =
            PathFinding.AStar(start, goal,
                (n) => GetNeighbours(n, map),
                (_, _) => 1,
                DistanceToGoal);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");

        answer.ShouldBe(expected);
    }

    [TestCase("data/12 - Sample.txt", 29, TestName = "Day 12 - Part 2 - Sample")]
    [TestCase("data/12 - Puzzle Input.txt", 418, TestName = "Day 12 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var (start, goal, map) = ParseInput(File.ReadAllText(inputFile));

        var answer = map.Where(x => x.Value == 0)
            .Select(x =>
                PathFinding.AStar(x.Key, goal,
                    (n) => GetNeighbours(n, map),
                    (_, _) => 1,
                    DistanceToGoal))
            .Min(x => x.Item1);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
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

    private int DistanceToGoal(Coordinate current, Coordinate goal)
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