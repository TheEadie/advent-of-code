namespace AdventOfCode2024.Day10;

public class Day10
{
    // https://adventofcode.com/2024/day/10
    private readonly AdventSession _session = new(2024, 10, "Hoof It");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 36)]
    [TestCase("Puzzle Input.txt", 617)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = Parse(input);

        var startPoints = map.Where(x => x.Value == 0).Select(x => x.Key).ToList();
        var routes = startPoints.Select(x => FindPaths(x, map)).ToList();

        var answer = routes.Sum(x => x.Distinct().Count());

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 81)]
    [TestCase("Puzzle Input.txt", 1_477)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = Parse(input);

        var startPoints = map.Where(x => x.Value == 0).Select(x => x.Key).ToList();
        var routes = startPoints.Select(x => FindPaths(x, map)).ToList();

        var answer = routes.Sum(x => x.Count());

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static Dictionary<Coordinate, int> Parse(string input)
    {
        var map = new Dictionary<Coordinate, int>();

        var lines = input.Split("\n");
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                map.Add(new Coordinate(x, y), int.Parse(c.ToString()));
            }
        }

        return map;
    }

    private static IEnumerable<Coordinate> FindPaths(Coordinate start, Dictionary<Coordinate, int> map)
    {
        var queue = new Queue<Coordinate>();
        queue.Enqueue(start);

        while (queue.Count != 0)
        {
            var current = queue.Dequeue();

            if (map[current] == 9)
            {
                yield return current;
            }

            var neighbours = Vector.FourDirections()
                .Select(v => current + v)
                .Where(x => map.ContainsKey(x) && map[x] == map[current] + 1);
            foreach (var neighbour in neighbours)
            {
                queue.Enqueue(neighbour);
            }
        }
    }
}
