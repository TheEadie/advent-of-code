namespace AdventOfCode2023.Day11;

public class Day11
{
    // https://adventofcode.com/2023/day/11
    private readonly AdventSession _session = new(2023, 11, "Cosmic Expansion");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 374)]
    [TestCase("Puzzle Input.txt", 9_312_968)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var galaxies = GetGalaxies(input);
        var xWithNoGalaxies = Enumerable.Range(0, galaxies.Max(c => c.X) + 1)
            .Where(x => galaxies.All(c => c.X != x))
            .ToHashSet();
        var yWithNoGalaxies = Enumerable.Range(0, galaxies.Max(c => c.Y) + 1)
            .Where(y => galaxies.All(c => c.Y != y))
            .ToHashSet();

        var answer = TotalDistances(2, galaxies, xWithNoGalaxies, yWithNoGalaxies);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 1030, 10)]
    [TestCase("Sample.txt", 8410, 100)]
    [TestCase("Puzzle Input.txt", 597_714_117_556, 1_000_000)]
    public async Task Part2(string inputFile, long expected, int scale)
    {
        var input = await _session.Start(inputFile);

        var galaxies = GetGalaxies(input);
        var xWithNoGalaxies = Enumerable.Range(0, galaxies.Max(c => c.X) + 1)
            .Where(x => galaxies.All(c => c.X != x))
            .ToHashSet();
        var yWithNoGalaxies = Enumerable.Range(0, galaxies.Max(c => c.Y) + 1)
            .Where(y => galaxies.All(c => c.Y != y))
            .ToHashSet();

        var answer = TotalDistances(scale, galaxies, xWithNoGalaxies, yWithNoGalaxies);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static long
        TotalDistances(
            int scale,
            HashSet<Coordinate> galaxies,
            HashSet<int> xWithNoGalaxies,
            HashSet<int> yWithNoGalaxies) =>
        GetAllPairs(galaxies)
            .Select(x => new Line(x.Item1, x.Item2))
            .Select(
                pair => (Pair: pair,
                    ExpandX: xWithNoGalaxies.Count(
                        x => Math.Min(pair.Start.X, pair.End.X) < x && x < Math.Max(pair.Start.X, pair.End.X)),
                    ExpandY: yWithNoGalaxies.Count(
                        y => Math.Min(pair.Start.Y, pair.End.Y) < y && y < Math.Max(pair.Start.Y, pair.End.Y))))
            .Sum(
                x => (long) Math.Abs(x.Pair.Start.X - x.Pair.End.X)
                    + Math.Abs(x.Pair.Start.Y - x.Pair.End.Y)
                    + x.ExpandX * (scale - 1)
                    + x.ExpandY * (scale - 1));

    private static HashSet<Coordinate> GetGalaxies(string input) =>
        input.Split("\n")
            .SelectMany((line, y) => line.Select((c, x) => (Text: c, Coordinate: new Coordinate(x, y))))
            .Where(x => x.Text == '#')
            .Select(x => x.Coordinate)
            .ToHashSet();

    private static List<(T, T)> GetAllPairs<T>(ICollection<T> list) =>
        list.Select((item1, i) => list.Skip(i + 1).Select(item2 => (item1, item2))).SelectMany(x => x).ToList();
}
