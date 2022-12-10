namespace AdventOfCode2022;

public class Day10
{
    [TestCase("data/10 - Sample.txt", 13140, TestName = "Sample")]
    [TestCase("data/10 - Puzzle Input.txt", 12460, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));
        var cpu = new CPU(input);

        var signalStrengths = new List<double>();

        for (var i = 1; i < input.Sum(x => x.Steps.Length); i++)
        {
            if (i == 20 || ((i - 20) % 40) == 0)
            {
                signalStrengths.Add(i * cpu.Registers.X);
            }
            cpu.Step();
        }

        var answer = signalStrengths.Sum();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/10 - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/10 - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int _)
    {
        var input = ParseInput(File.ReadAllText(inputFile));
        var cpu = new CPU(input);
        var gpu = new GPU(cpu.Registers);

        for (var i = 1; i < input.Sum(x => x.Steps.Length); i++)
        {
            gpu.Step();
            cpu.Step();
        }

        gpu.Draw();
    }

    private static IEnumerable<OpCode> ParseInput(string input)
    {
        return input.Split("\n")
                .Select(x =>
                    x.Split(" ")[0] switch
                    {
                        "noop" => new OpCode("Noop", new Func<Registers, bool>[] {
                            _ => true
                        }),
                        "addx" => new OpCode("Add x", new Func<Registers, bool>[]{
                            _ => false,
                            r => {r.X += int.Parse(x.Split(" ")[1]); return true;}
                    }),
                        _ => throw new ArgumentOutOfRangeException()
                    });
    }

    private class CPU
    {
        public Registers Registers { get; }
        private readonly OpCode[] _instructions;
        private int _pc;
        private OpCode _currentInstruction;
        private int _subPC;

        public CPU(IEnumerable<OpCode> program)
        {
            Registers = new Registers();
            _instructions = program.ToArray();
            _currentInstruction = _instructions[0];
        }

        public void Step()
        {
            var complete = _currentInstruction.Steps[_subPC](Registers);
            _subPC++;
            if (complete)
            {
                _subPC = 0;
                _pc++;
                _currentInstruction = _instructions[_pc];
            }
        }
    }

    private class GPU
    {
        private readonly Registers _registers;
        private readonly bool[,] _pixels;
        private int _cycle;

        public GPU(Registers registers)
        {
            _registers = registers;
            _pixels = new bool[6, 40];
        }

        public void Step()
        {
            var x = _cycle % 40;
            var y = _cycle / 40;

            _pixels[y, x] = x == _registers.X
                || x == (_registers.X + 1)
                || x == (_registers.X - 1);

            _cycle++;
        }

        public void Draw()
        {
            for (var y = 0; y < _pixels.GetLength(0); y++)
            {
                for (var x = 0; x < _pixels.GetLength(1); x++)
                {
                    Console.Write(_pixels[y, x] ? "#" : ".");
                }
                Console.WriteLine();
            }
        }
    }

    private record OpCode(string Description, Func<Registers, bool>[] Steps);

    private class Registers
    {
        public int X { get; set; } = 1;
    }
}