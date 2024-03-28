namespace AdventOfCode2023.Day10;

public class Day10
{
    // https://adventofcode.com/2023/day/10
    private readonly AdventSession _session = new(2023, 10, "Pipe Maze");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 8)]
    [TestCase("Puzzle Input.txt", 0)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = Parse(input);

        var start = map.Single(x => x.Value.Intersect(Vector.FourDirections()).Count() == 4).Key;

        var queue = new Queue<Coordinate>();
        var visited = new HashSet<Coordinate>();
        var distances = new Dictionary<Coordinate, int>{ { start, 0 } };

        queue.Enqueue(start);
        while(queue.Count != 0)
        {
            var current = queue.Dequeue();
            _ = visited.Add(current);

            var neighbours = GetNeighbours(current, map);
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

        var answer = distances.Values.Max();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Coordinate> GetNeighbours(
        Coordinate position,
        IDictionary<Coordinate, IEnumerable<Vector>> map)
    {
        var directions = map[position];

        foreach (var direction in directions)
        {
            var next = position + direction;

            if (!map.ContainsKey(next))
            {
                continue;
            }

            if (direction == Vector.Up && map[next].Contains(Vector.Down) ||
                direction == Vector.Down && map[next].Contains(Vector.Up) ||
                direction == Vector.Left && map[next].Contains(Vector.Right) ||
                direction == Vector.Right && map[next].Contains(Vector.Left))
            {
                yield return next;
            }
        }
    }


    [TestCase("Sample.txt", 0)]
    [TestCase("Puzzle Input.txt", 0)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = 0;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IDictionary<Coordinate, IEnumerable<Vector>> Parse(string input)
    {
        var lines = input.Split("\n");

        var map = Enumerable.Range(0, lines.Length)
            .SelectMany(
                y => Enumerable.Range(0, lines[0].Length)
                    .Select(x => (Coordinate: new Coordinate(x, y), Value: lines[y][x]))
                    .Select(x => (x.Coordinate, Vectors: GetVectors(x.Value), Symbol: x.Value)))
            .ToDictionary(key => key.Coordinate, value => value.Vectors);
        return map;
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
