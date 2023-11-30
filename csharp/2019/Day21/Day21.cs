namespace AdventOfCode2019.Day21;

public class Day21
{
    // https://adventofcode.com/2019/day/21
    private readonly AdventSession _session = new(2019, 21, "Springdroid Adventure");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 19_355_862)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var asciiProgram = new[]
        {
            "NOT B J",
            "NOT C T",
            "OR T J",
            "AND D J",
            "NOT A T",
            "OR T J",
            "WALK"
        };

        var answer = RunProgram(asciiProgram, program);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 1_140_470_745)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var asciiProgram = new[]
        {
            "NOT B J",
            "NOT C T",
            "OR T J",
            "AND D J",
            "AND H J",
            "NOT A T",
            "OR T J",
            "RUN"
        };

        var answer = RunProgram(asciiProgram, program);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static long RunProgram(IEnumerable<string> asciiProgram, long[] program)
    {
        var emulator = new IntCode.IntCode(program);
        IEnumerable<long> outputs = new List<long>();

        foreach (var line in asciiProgram)
        {
            foreach (var c in line)
            {
                _ = emulator.Run(c);
            }

            (_, outputs) = emulator.Run(10);
        }

        //Console.Error.WriteLine(new string(outputs.Select(x => (char)x).ToArray()));

        return outputs.Last();
    }
}
