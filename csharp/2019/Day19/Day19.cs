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

        var firstCoordinate = FindSquare(program, 100);

        var answer = 10_000 * firstCoordinate.X + firstCoordinate.Y;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static Coordinate FindSquare(long[] program, int size)
    {
        var endOfRow = new Coordinate(0, 100);
        var startOfRow = new Coordinate(0, 100);

        while (endOfRow.Y < 1_000_000)
        {
            while(!IsInBeam(program, startOfRow))
            {
                startOfRow += Vector.Right;
            }

            endOfRow = startOfRow;

            while (IsInBeam(program, endOfRow))
            {
                if (FoundSquare(program, endOfRow, size))
                {
                    return endOfRow;
                }

                endOfRow += Vector.Right;
            }

            startOfRow += Vector.Down;
        }

        return new Coordinate(0, 0);
    }

    private static bool FoundSquare(long[] program, Coordinate coordinate, int size) =>
        IsInBeam(program, coordinate with { X = coordinate.X + size - 1 }) &&
        IsInBeam(program, coordinate with { Y = coordinate.Y + size - 1 });

    private static bool IsInBeam(long[] program, Coordinate c)
    {
        var emulator = new IntCode.IntCode(program);
        _ = emulator.Run(c.X);
        var (_, outputs) = emulator.Run(c.Y);
        return outputs.Last() == 1;
    }
}
