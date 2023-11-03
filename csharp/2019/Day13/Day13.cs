namespace AdventOfCode2019.Day13;

public class Day13
{
    private readonly AdventSession _session = new(2019, 13, "Care Package");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var emulator = new IntCode.IntCode(program);
        var (screen, _) = RunProgram(emulator);

        var answer = screen.Count(x => x.Value == 2);
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(329);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();
        var emulator = new IntCode.IntCode(program) { Memory = { [0] = 2 } };

        var (_, answer) = RunProgram(emulator);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(15973);
    }

    private static (IDictionary<Coordinate, long>, long) RunProgram(IntCode.IntCode emulator)
    {
        var screen = new Dictionary<Coordinate, long>();
        var segmentDisplay = 0L;

        emulator.Run(
            outputs =>
                {
                    foreach (var output in outputs.Chunk(3))
                    {
                        var x = output.ElementAt(0);
                        var y = output.ElementAt(1);
                        var value = output.ElementAt(2);
                        var coordinate = new Coordinate((int) x, (int) y);
                        if (x == -1 && y == 0)
                        {
                            segmentDisplay = value;
                        }
                        else
                        {
                            screen[coordinate] = value;
                        }
                    }
                },
            () =>
                {
                    var ballXPosition = screen.Single(x => x.Value == 4).Key.X;
                    var paddleXPosition = screen.Single(x => x.Value == 3).Key.X;
                    var distance = ballXPosition - paddleXPosition;
                    return distance > 0 ? 1 : distance < 0 ? -1 : 0;
                });

        return (screen, segmentDisplay);
    }
}
