namespace AdventOfCode2019.Day09;

public class Day09
{
    private readonly AdventSession _session = new(2019, 9, "Sensor Boost");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var emulator = new IntCode.IntCode(program);
        var (_, outputs) = emulator.Run(1);

        var answer = outputs.Last();
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(2941952859);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var emulator = new IntCode.IntCode(program);
        var (_, outputs) = emulator.Run(2);

        var answer = outputs.Last();
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(66113);
    }
}
