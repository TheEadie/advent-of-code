namespace AdventOfCode2025.Day07;

public class Day07
{
    // https://adventofcode.com/2025/day/7
    private readonly AdventSession _session = new(2025, 07, "Laboratories");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 21)]
    [TestCase("Puzzle Input.txt", 1546)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (start, nodes) = ParseInput(input);

        var answer = RunSimulation(start, nodes).Count(x => x.Node.Y <= nodes.Max(n => n.Y));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 40)]
    [TestCase("Puzzle Input.txt", 13_883_459_503_480)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var (start, nodes) = ParseInput(input);

        var answer = RunSimulation(start, nodes).Where(x => x.Node.Y > nodes.Max(n => n.Y)).Select(x => x.Ways).Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<(Coordinate Node, long Ways)> RunSimulation(Coordinate start, HashSet<Coordinate> nodes)
    {
        var maxDepth = nodes.Max(n => n.Y);
        var queue = new PriorityQueue<Coordinate, int>();
        var visited = new HashSet<Coordinate>();
        var ways = new Dictionary<Coordinate, long>();
        var startDown = MoveDown(start, nodes, maxDepth)!;
        ways[startDown] = 1;
        queue.Enqueue(startDown, startDown.Y);

        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current))
            {
                continue;
            }

            yield return (current, ways[current]);

            var left = MoveDown(current + Vector.Left, nodes, maxDepth);
            if (left is not null && IsInBounds(left, maxDepth))
            {
                ways[left] = ways.GetValueOrDefault(left) + ways[current];
                queue.Enqueue(left, left.Y);
            }

            var right = MoveDown(current + Vector.Right, nodes, maxDepth);
            if (right is not null && IsInBounds(right, maxDepth))
            {
                ways[right] = ways.GetValueOrDefault(right) + ways[current];
                queue.Enqueue(right, right.Y);
            }
        }
    }

    private static Coordinate? MoveDown(Coordinate current, HashSet<Coordinate> nodes, int maxDepth) =>
        MoveAlongVector(current, Vector.Down).TakeUntil(x => nodes.Contains(x) || x.Y > maxDepth).LastOrDefault();

    private static bool IsInBounds(Coordinate coordinate, int maxY) => coordinate.Y >= 0 && coordinate.Y <= maxY + 1;

    private static IEnumerable<Coordinate> MoveAlongVector(Coordinate coordinate, Vector step)
    {
        var next = coordinate + step;
        while (true)
        {
            yield return next;

            next += step;
        }
    }

    private static (Coordinate Start, HashSet<Coordinate> Nodes) ParseInput(string input)
    {
        var start = new Coordinate(0, 0);
        var nodes = new HashSet<Coordinate>();

        var lines = input.Split("\n");
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                switch (line[x])
                {
                    case '^':
                        nodes.Add(new Coordinate(x, y));
                        break;
                    case 'S':
                        start = new Coordinate(x, y);
                        break;
                }
            }
        }

        return (start, nodes);
    }
}
