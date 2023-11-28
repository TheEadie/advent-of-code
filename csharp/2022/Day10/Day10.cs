using System.Text;

namespace AdventOfCode2022.Day10;

public class Day10
{
    private readonly AdventSession _session = new(2022, 10, "Cathode-Ray Tube");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 13140)]
    [TestCase("Puzzle Input.txt", 12460)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var opCodes = ParseInput(input).ToList();
        var cpu = new Cpu(opCodes);

        var signalStrengths = new List<double>();

        for (var i = 1; i < opCodes.Sum(x => x.Steps.Length); i++)
        {
            if (i == 20 || (i - 20) % 40 == 0)
            {
                signalStrengths.Add(i * cpu.Registers.X);
            }
            cpu.Step();
        }

        var answer = signalStrengths.Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", "-")]
    [TestCase("Puzzle Input.txt", "EZFPRAKL")]
    public async Task Part2(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);

        var opCodes = ParseInput(input).ToList();
        var cpu = new Cpu(opCodes);
        var gpu = new Gpu(cpu.Registers);

        for (var i = 1; i < opCodes.Sum(x => x.Steps.Length); i++)
        {
            gpu.Step();
            cpu.Step();
        }

        _session.PrintAnswer(2, expected);
        //gpu.Draw();
    }

    private static IEnumerable<OpCode> ParseInput(string input)
    {
        return input.Split("\n")
                .Select(x =>
                    x.Split(" ")[0] switch
                    {
                        "noop" => new OpCode([_ => true]),
                        "addx" => new OpCode([_ => false,
                            r => {r.X += int.Parse(x.Split(" ")[1]); return true;}
                        ]),
                        _ => throw new ArgumentOutOfRangeException()
                    });
    }

    private class Cpu
    {
        public Registers Registers { get; }
        private readonly OpCode[] _instructions;
        private int _pc;
        private OpCode _currentInstruction;
        private int _subPc;

        public Cpu(IEnumerable<OpCode> program)
        {
            Registers = new Registers();
            _instructions = program.ToArray();
            _currentInstruction = _instructions[0];
        }

        public void Step()
        {
            var complete = _currentInstruction.Steps[_subPc](Registers);
            _subPc++;
            if (!complete)
            {
                return;
            }

            _subPc = 0;
            _pc++;
            _currentInstruction = _instructions[_pc];
        }
    }

    private class Gpu(Registers registers)
    {
        private readonly bool[,] _pixels = new bool[6, 40];
        private int _cycle;

        public void Step()
        {
            var x = _cycle % 40;
            var y = _cycle / 40;

            _pixels[y, x] = x == registers.X
                || x == registers.X + 1
                || x == registers.X - 1;

            _cycle++;
        }

        public void Draw()
        {
            for (var y = 0; y < _pixels.GetLength(0); y++)
            {
                var row = new StringBuilder();
                for (var x = 0; x < _pixels.GetLength(1); x++)
                {
                    row = row.Append(_pixels[y, x] ? "#" : ".");
                }
                Console.Error.WriteLine(row.ToString());
            }
        }
    }

    private record OpCode(Func<Registers, bool>[] Steps);

    private class Registers
    {
        public int X { get; set; } = 1;
    }
}
