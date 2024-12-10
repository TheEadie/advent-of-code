namespace AdventOfCode2024.Day08;

public class Day08
{
    // https://adventofcode.com/2024/day/8
    private readonly AdventSession _session = new(2024, 08, "Resonant Collinearity");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 14)]
    [TestCase("Puzzle Input.txt", 244)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (map, size) = Parse(input);

        var answer = map.Select(x => (A: x.Key, Others: map.Where(o => o.Value == x.Value && o.Key != x.Key)))
            .SelectMany(g => g.Others.SelectMany(x => GetAntiNodes(g.A, x.Key).Skip(1).Take(1)))
            .Where(a => InBounds(a, size))
            .Distinct()
            .Count();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 34)]
    [TestCase("Puzzle Input.txt", 912)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (map, size) = Parse(input);

        var answer = map.Select(x => (A: x.Key, Others: map.Where(o => o.Value == x.Value && o.Key != x.Key)))
            .SelectMany(g => g.Others.SelectMany(x => GetAntiNodes(g.A, x.Key).TakeWhile(a => InBounds(a, size))))
            .Distinct()
            .Count();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Coordinate> GetAntiNodes(Coordinate a, Coordinate b)
    {
        while (true)
        {
            var aToB = new Vector(a, b);
            a += aToB;
            b += aToB;
            yield return a;
        }
    }

    private static (Dictionary<Coordinate, char> map, Coordinate size) Parse(string input)
    {
        var lines = input.Split("\n");
        var map = lines
            .SelectMany((str, y) => str.ToCharArray().Select((c, x) => (Position: new Coordinate(x, y), Value: c)))
            .Where(x => x.Value != '.')
            .ToDictionary(x => x.Position, x => x.Value);
        var size = new Coordinate(lines[0].Length - 1, lines.Length - 1);
        return (map, size);
    }

    private static bool InBounds(Coordinate c, Coordinate bottomRight) =>
        c.X >= 0 && c.X <= bottomRight.X && c.Y >= 0 && c.Y <= bottomRight.Y;
}
