namespace AdventOfCode2015.Day02;

public class Day02
{
    private readonly AdventSession _session = new(2015, 2, "I Was Told There Would Be No Math");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var presents = ParseInput(input).ToList();

        var presentArea = presents.Sum(x => 2 * x.Length * x.Width + 2 * x.Width * x.Height + 2 * x.Height * x.Length);
        var extra = presents.Sum(x => new List<int> { x.Length * x.Width, x.Width * x.Height, x.Height * x.Length }.Min());

        var answer = presentArea + extra;
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(1588178);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var presents = ParseInput(input).ToList();

        var ribbon = presents.Sum(RibbonForBox);
        var bows = presents.Sum(x => x.Height * x.Length * x.Width);

        var answer = ribbon + bows;
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(3783758);
    }

    private static int RibbonForBox(Box box)
    {
        var smallestSides = new List<int> { box.Length, box.Width, box.Height }.OrderBy(x => x).ToList();
        return 2 * (smallestSides[0] + smallestSides[1]);
    }

    private static IEnumerable<Box> ParseInput(string input)
    {
        return input.Split("\n")
            .Select(x => x.Split('x'))
            .Select(x => new Box(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])));
    }

    private record Box(int Length, int Width, int Height);
}
