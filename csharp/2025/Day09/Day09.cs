namespace AdventOfCode2025.Day09;

public class Day09
{
    // https://adventofcode.com/2025/day/9
    private readonly AdventSession _session = new(2025, 09, "Movie Theater");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 50)]
    [TestCase("Puzzle Input.txt", 4_755_278_336)]
    public async Task Part1(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var coordinates = ParseInput(input).ToList();
        var areas = GetAreas(coordinates);

        var answer = areas.Last().Value;

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

    private record Area(Coordinate A, Coordinate B, long Value);

    private static List<Area> GetAreas(List<Coordinate> coordinates)
    {
        var areas = new List<Area>();
        foreach (var x in coordinates)
        {
            areas.AddRange(
                coordinates.TakeWhile(y => x != y)
                    .Select(y => new Area(x, y, (Math.Abs(x.X - y.X) + 1L) * (Math.Abs(x.Y - y.Y) + 1L))));
        }

        return areas.OrderBy(x => x.Value).ToList();
    }

    private static IEnumerable<Coordinate> ParseInput(string input) =>
        input.Split("\n")
            .Select(x =>
                {
                    var parts = x.Split(",");
                    return new Coordinate(int.Parse(parts[0]), int.Parse(parts[1]));
                });
}
