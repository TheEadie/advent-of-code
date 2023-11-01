using AdventOfCode2019.IntCode;

namespace AdventOfCode2019.Day07;

public class Day07
{
    private readonly AdventSession _session = new(2019, 7, "Amplification Circuit");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var inputs = new List<int>
        {
            0,
            1,
            2,
            3,
            4
        };
        var possible = inputs.Permutate();

        var answer = (await Task.WhenAll(possible.Select(x => GetOutput(program, x)))).Max();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(212460);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var inputs = new List<int>
        {
            5,
            6,
            7,
            8,
            9
        };
        var possible = inputs.Permutate();

        var answer = (await Task.WhenAll(possible.Select(x => GetOutput(program, x)))).Max();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(21844737);
    }

    private static Task<long> GetOutput(long[] program, int[] inputs)
    {
        var ampA = new IntCode.IntCode(program);
        var ampB = new IntCode.IntCode(program);
        var ampC = new IntCode.IntCode(program);
        var ampD = new IntCode.IntCode(program);
        var ampE = new IntCode.IntCode(program);

        _ = ampA.Run(inputs[0]);
        _ = ampB.Run(inputs[1]);
        _ = ampC.Run(inputs[2]);
        _ = ampD.Run(inputs[3]);
        var ampEResult = ampE.Run(inputs[4]);
        var ampAResult = ampA.Run(0);

        while (ampAResult.Status != IntCodeStatus.Halted)
        {
            var ampBResult = ampB.Run(ampAResult.Output);
            var ampCResult = ampC.Run(ampBResult.Output);
            var ampDResult = ampD.Run(ampCResult.Output);
            ampEResult = ampE.Run(ampDResult.Output);
            ampAResult = ampA.Run(ampEResult.Output);
        }

        return Task.FromResult(ampEResult.Output!.Value);
    }
}
