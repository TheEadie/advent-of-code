namespace AdventOfCode2019.Day19;

public class Day19
{
    private readonly AdventSession _session = new(2019, 19, "");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 197)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var grid = Enumerable.Range(0, 50)
            .SelectMany(x => Enumerable.Range(0, 50).Select(y => new Coordinate(x, y)))
            .Select(
                c =>
                    {
                        var emulator = new IntCode.IntCode(program);
                        emulator.Run(c.X);
                        var (_, outputs) = emulator.Run(c.Y);
                        return (Location: c, Beam: outputs.Last() == 1);
                    });

        var answer = grid.Count(x => x.Beam);
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 0)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = 0;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }
}
