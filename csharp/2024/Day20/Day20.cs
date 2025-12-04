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

        var enumeratedPath = orderedPath.ToList();
        var pathWithIndex = enumeratedPath.Index().ToDictionary(k => k.Item, v => v.Index);

        var shortcuts = pathWithIndex.SelectMany(x =>
            Vector.FourDirections()
                .Select(v => (Start: x, End: x.Key + v + v))
                .Where(n => pathWithIndex.ContainsKey(n.End)));
        var timeSaved = shortcuts.Select(n => (n.Start.Key, n.End, pathWithIndex[n.End] - n.Start.Value - 2))
            .Where(n => n.Item3 >= saveAtLeast);

        var answer = timeSaved.Count();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
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
