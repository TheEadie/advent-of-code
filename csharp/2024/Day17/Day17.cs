namespace AdventOfCode2024.Day17;

public class Day17
{
    // https://adventofcode.com/2024/day/17
    private readonly AdventSession _session = new(2024, 17, "Chronospatial Computer");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", "4,6,3,5,6,3,5,2,1,0")]
    [TestCase("Puzzle Input.txt", "1,6,7,4,3,0,5,0,6")]
    public async Task Part1(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);
        var (a, b, c, program) = Parse(input);
        var machine = new ChronospatialComputer(a, b, c, program);

        var output = machine.Run();
        var answer = string.Join(",", output);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample2.txt", (ulong) 117_440)]
    [TestCase("Puzzle Input.txt", (ulong) 216_148_338_630_253)]
    public async Task Part2(string inputFile, ulong expected)
    {
        var input = await _session.Start(inputFile);
        var (a, b, c, program) = Parse(input);
        var machine = new ChronospatialComputer(a, b, c, program);

        var possibleAs = new List<ulong> { 0 };
        foreach (var programItem in program.Reverse())
        {
            var newAs = new List<ulong>();
            foreach (var possibleA in possibleAs)
            {
                for (var j = 0; j < 8; j++)
                {
                    var candidate = (possibleA << 3) + (uint) j;
                    var result = machine.Run(candidate).First();
                    if (result == programItem)
                    {
                        newAs.Add(candidate);
                    }
                }
            }

            possibleAs = newAs;
        }

        var answer = possibleAs.Min();
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (ulong a, ulong b, ulong c, ulong[] program) Parse(string input)
    {
        var sections = input.Split("\n");
        var a = uint.Parse(sections[0].Split(" ")[2]);
        var b = uint.Parse(sections[1].Split(" ")[2]);
        var c = uint.Parse(sections[2].Split(" ")[2]);
        var program = sections[4].Split(" ")[1].Split(",").Select(ulong.Parse).ToArray();
        return (a, b, c, program);
    }

    private class ChronospatialComputer(ulong a, ulong b, ulong c, ulong[] program)
    {
        private ulong _pc;
        private ulong _a = a;
        private ulong _b = b;
        private ulong _c = c;

        private readonly List<uint> _output = [];

        public List<uint> Run(ulong? a = null)
        {
            _a = a ?? _a;
            _pc = 0;
            _b = 0;
            _c = 0;
            _output.Clear();

            while ((int) _pc < program.Length)
            {
                Step();
            }

            return _output;
        }

        private void Step()
        {
            var opCode = program[_pc];
            var operand = program[_pc + 1];
            _pc += 2;

            switch (opCode)
            {
                case 0:
                    _a = (ulong) Math.Floor(_a / Math.Pow(2, ConvertToCombo(operand)));
                    break;
                case 1:
                    _b ^= operand;
                    break;
                case 2:
                    _b = ConvertToCombo(operand) % 8;
                    break;
                case 3:
                    _pc = _a != 0 ? operand : _pc;
                    break;
                case 4:
                    _b ^= _c;
                    break;
                case 5:
                    _output.Add((uint) ConvertToCombo(operand) % 8);
                    break;
                case 6:
                    _b = (ulong) Math.Floor(_a / Math.Pow(2, ConvertToCombo(operand)));
                    break;
                case 7:
                    _c = (ulong) Math.Floor(_a / Math.Pow(2, ConvertToCombo(operand)));
                    break;
            }
        }

        private ulong ConvertToCombo(ulong i) =>
            i switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => _a,
                5 => _b,
                6 => _c,
                _ => throw new InvalidOperationException()
            };
    };
}
