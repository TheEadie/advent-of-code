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
        var (ampEResult, _) = ampE.Run(inputs[4]);

        var result = 0L;

        while (ampEResult != IntCodeStatus.Halted)
        {
            var (_, ampAOutputs) = ampA.Run(result);
            var (_, ampBOutputs) = ampB.Run(ampAOutputs.Last());
            var (_, ampCOutputs) = ampC.Run(ampBOutputs.Last());
            var (_, ampDOutputs) = ampD.Run(ampCOutputs.Last());
            (ampEResult, var ampEOutputs) = ampE.Run(ampDOutputs.Last());
            result = ampEOutputs.Last();
        }

        return Task.FromResult(result);
    }
}
