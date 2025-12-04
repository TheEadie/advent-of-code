namespace AdventOfCode2024.Day20;

public class Day20
{
    // https://adventofcode.com/2024/day/20
    private readonly AdventSession _session = new(2024, 20, "Race Condition");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 44, 1)]
    [TestCase("Puzzle Input.txt", 1321, 100)]
    public async Task Part1(string inputFile, int expected, int saveAtLeast)
    {
        var input = await _session.Start(inputFile);
        var (start, finish, path) = Parse(input);

        var (_, orderedPath) = PathFinding.AStar(
            start,
            x => x == finish,
            (x) => Vector.FourDirections().Select(v => x + v).Where(n => path.Contains(n)),
            (_, _) => 1,
            _ => 1);

        var answer = GetCheats(orderedPath.ToList(), 2).Count(n => n >= saveAtLeast);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 285, 50)]
    [TestCase("Puzzle Input.txt", 971_737, 100)]
    public async Task Part2(string inputFile, int expected, int saveAtLeast)
    {
        var input = await _session.Start(inputFile);
        var (start, finish, path) = Parse(input);

        var (_, orderedPath) = PathFinding.AStar(
            start,
            x => x == finish,
            (x) => Vector.FourDirections().Select(v => x + v).Where(n => path.Contains(n)),
            (_, _) => 1,
            _ => 1);

        var answer = GetCheats(orderedPath.ToList(), 20).Count(n => n >= saveAtLeast);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<int> GetCheats(List<Coordinate> path, int maxLength)
    {
        var pathWithIndex = path.Index().ToDictionary(k => k.Item, v => v.Index);
        return path.SelectMany(x => path.Select(y => (Start: x, End: y, Distance: Coordinate.ManhattanDistance(x, y))))
            .Where(x => x.Distance <= maxLength && pathWithIndex.ContainsKey(x.End))
            .Select(x => pathWithIndex[x.End] - pathWithIndex[x.Start] - x.Distance);
    }

    private static (Coordinate Start, Coordinate Finish, HashSet<Coordinate> Path) Parse(string input)
    {
        Coordinate start = null!;
        Coordinate finish = null!;
        var path = new HashSet<Coordinate>();

        var lines = input.Split("\n");
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                var coord = new Coordinate(x, y);
                switch (c)
                {
                    case 'S':
                        start = coord;
                        path.Add(coord);
                        break;
                    case 'E':
                        finish = coord;
                        path.Add(coord);
                        break;
                    case '.':
                        path.Add(coord);
                        break;
                }
            }
        }

        return (start, finish, path);
    }
}
