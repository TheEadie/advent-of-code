namespace AdventOfCode2023.Day10;

public class Day10
{
    // https://adventofcode.com/2023/day/10
    private readonly AdventSession _session = new(2023, 10, "Pipe Maze");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 8)]
    [TestCase("Puzzle Input.txt", 7102)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (start, map) = Parse(input);
        var (_, maxDistance) = GetLoop(start, map);

        var answer = maxDistance / 3;

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 1)]
    [TestCase("Puzzle Input.txt", 363)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (start, map) = Parse(input);
        var bottomRight = new Coordinate(map.Max(x => x.X) + 1, map.Max(x => x.Y) + 1);

        var (loop, _) = GetLoop(start, map);
        var outside = FloodFill(loop, new Coordinate(0,0), bottomRight);

        var answer = 0;

        for (var y = 0; y < bottomRight.Y / 3; y++)
        {
            for (var x = 0; x < bottomRight.X / 3; x++)
            {
                var middle = new Coordinate(x * 3 + 1, y * 3 + 1);
                if (!outside.Contains(middle) &&
                    !loop.Contains(middle))
                {
                    answer++;
                }
            }
        }

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IReadOnlySet<Coordinate> FloodFill(IReadOnlySet<Coordinate> map, Coordinate startPoint, Coordinate bottomRight)
    {
        var queue = new Queue<Coordinate>();
        var visited = new HashSet<Coordinate>();

        queue.Enqueue(startPoint);
        while(queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current))
            {
                continue;
            }
            _ = visited.Add(current);

            var neighbours = Vector.FourDirections()
                .Select(x => current + x)
                .Where(x => x.X >= 0 &&
                    x.Y >= 0 &&
                    x.X <= bottomRight.X &&
                    x.Y <= bottomRight.Y &&
                    !map.Contains(x));

            foreach (var neighbour in neighbours)
            {
                if (!visited.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        return visited;
    }

    private static (IReadOnlySet<Coordinate>, int) GetLoop(Coordinate start, IReadOnlySet<Coordinate> map)
    {
        var queue = new Queue<Coordinate>();
        var visited = new HashSet<Coordinate>();
        var distances = new Dictionary<Coordinate, int>{ { start, 0 } };

        queue.Enqueue(start);
        while(queue.Count != 0)
        {
            var current = queue.Dequeue();
            _ = visited.Add(current);

            var neighbours = Vector.FourDirections()
                .Select(x => current + x)
                .Where(map.Contains);

            foreach (var neighbour in neighbours)
            {
                if (visited.Contains(neighbour))
                {
                    continue;
                }

                distances[neighbour] = distances[current] + 1;
                queue.Enqueue(neighbour);
            }
        }

        return (visited, distances.Max(x => x.Value));
    }


    private static (Coordinate, IReadOnlySet<Coordinate>) Parse(string input)
    {
        var map = new HashSet<Coordinate>();
        var start = new Coordinate(0, 0);
        var lines = input.Split("\n");

        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                var c = lines[y][x];
                var middle = new Coordinate(x * 3 + 1, y * 3 + 1);
                map.Add(middle);
                foreach (var vector in GetVectors(c))
                {
                    map.Add(middle + vector);
                }

                if (c == 'S')
                {
                    start = middle;
                }
            }
        }

        return (start, map);
    }

    private static IEnumerable<Vector> GetVectors(char c) =>
        c switch
        {
            '|' => [Vector.Up, Vector.Down],
            '-' => [Vector.Left, Vector.Right],
            'L' => [Vector.Up, Vector.Right],
            'J' => [Vector.Up, Vector.Left],
            '7' => [Vector.Down, Vector.Left],
            'F' => [Vector.Down, Vector.Right],
            '.' => [],
            'S' => [Vector.Up, Vector.Down, Vector.Left, Vector.Right],
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
}
