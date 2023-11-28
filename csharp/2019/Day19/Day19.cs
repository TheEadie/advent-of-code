namespace AdventOfCode2019.Day19;

public class Day19
{
    private readonly AdventSession _session = new(2019, 19, "Tractor Beam");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 197)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var answer = Enumerable.Range(0, 50)
            .SelectMany(x => Enumerable.Range(0, 50).Select(y => new Coordinate(x, y)))
            .Select(c => IsInBeam(program, c))
            .Count(x => x);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 9_181_022)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var beam = Enumerable.Range(900, 1000)
            .SelectMany(x => Enumerable.Range(1000, 1000).Select(y => new Coordinate(x, y)))
            .Where(c => IsInBeam(program, c))
            .ToList();

        var found = FindSquare(beam);

        var answer = 10_000 * found.X + found.Y;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static Coordinate FindSquare(List<Coordinate> beam)
    {
        foreach (var (x, y) in beam)
        {
            var coordinateY = new Coordinate(x, y + 99);
            var coordinateX = new Coordinate(x + 99, y);
            if (beam.Contains(coordinateY) && beam.Contains(coordinateX))
            {
                var coordinate = new Coordinate(x, y);
                Console.Error.WriteLine($"Found it! {coordinate}");
                return coordinate;
            }
        }

        return new Coordinate(0, 0);
    }

    private static bool IsInBeam(long[] program, Coordinate c)
    {
        var emulator = new IntCode.IntCode(program);
        _ = emulator.Run(c.X);
        var (_, outputs) = emulator.Run(c.Y);
        return outputs.Last() == 1;
    }
}
