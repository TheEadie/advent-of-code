using AdventOfCode2019.IntCode;

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

        var result = emulator.Run();
        while (result.Status != IntCodeStatus.Halted)
        {
            switch (result.Status)
            {
                case IntCodeStatus.OutputAvailable:
                    {
                        var x = result.Output!.Value;
                        var y = emulator.Run().Output!.Value;
                        var output = emulator.Run();
                        var value = output.Output!.Value;
                        var coordinate = new Coordinate((int) x, (int) y);
                        if (x == -1 && y == 0)
                        {
                            segmentDisplay = value;
                        }
                        else
                        {
                            screen[coordinate] = value;
                        }

                        result = emulator.Run();
                        continue;
                    }
                case IntCodeStatus.AwaitingInput:
                    {
                        var ballXPosition = screen.Single(x => x.Value == 4).Key.X;
                        var paddleXPosition = screen.Single(x => x.Value == 3).Key.X;
                        var distance = ballXPosition - paddleXPosition;
                        var move = distance > 0 ? 1 : distance < 0 ? -1 : 0;

                        result = emulator.Run(move);
                        continue;
                    }
                default:
                    throw new ApplicationException("No idea how we got here!");
            }
        }

        return (screen, segmentDisplay);
    }
}
