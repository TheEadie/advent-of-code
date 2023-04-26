namespace AdventOfCode2019.Day02;

public class Day02
{
    private readonly AdventSession _session = new(2019, 2, "1202 Program Alarm");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        program[1] = 12;
        program[2] = 2;

        var emulator = new IntCode.IntCode(program);
        await emulator.RunAsync(CancellationToken.None);
        var answer = emulator.Memory[0];

        Console.WriteLine(answer);
        answer.ShouldBe(3790645);
    }

    [Test]
    public async Task Part2()
    {
        static async Task<(int noun, int verb)> FindResult(long[] program, int result)
        {
            for (var n = 0; n < 100; n++)
            {
                for (var v = 0; v < 100; v++)
                {
                    program[1] = n;
                    program[2] = v;
                    var emulator = new IntCode.IntCode(program);
                    await emulator.RunAsync(CancellationToken.None);
                    var output = emulator.Memory[0];

                    if (output == result)
                    {
                        return (n, v);
                    }
                }
            }

            throw new Exception($"No inputs found that result in {result}");
        }

        var input = await _session.Start("Puzzle Input.txt");
        var program = input
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        var (noun, verb) = await FindResult(program, 19690720);

        var answer = 100 * noun + verb;
        Console.WriteLine(answer);
        answer.ShouldBe(6577);
    }
}
