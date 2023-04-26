namespace AdventOfCode2021.Day05;

public class Day05
{
    private readonly AdventSession _session = new(2021, 5, "Hydrothermal Venture");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var answer = ParseInput(input)
            .Where(x => x.Start.X == x.End.X || x.Start.Y == x.End.Y)
            .SelectMany(x => x.GetCoordinatesOnLine())
            .GroupBy(x => x)
            .Count(x => x.Count() > 1);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(5145);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var answer = ParseInput(input)
            .SelectMany(x => x.GetCoordinatesOnLine())
            .GroupBy(x => x)
            .Count(x => x.Count() > 1);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(16518);
    }

    private static IEnumerable<Line> ParseInput(string input)
    {
        var lines = input.Split("\n");
        var parseOne = lines.Select(x => x.Split(" -> ")).Select(ParseVentLine);
        return parseOne.ToList();
    }

    private static Line ParseVentLine(string[] values)
    {
        var firstCoordinates = values[0].Split(',');
        var firstX = int.Parse(firstCoordinates[0]);
        var firstY = int.Parse(firstCoordinates[1]);
        var secondCoordinates = values[1].Split(',');
        var secondX = int.Parse(secondCoordinates[0]);
        var secondY = int.Parse(secondCoordinates[1]);

        return new Line(new Coordinate(firstX, firstY), new Coordinate(secondX, secondY));
    }


}
