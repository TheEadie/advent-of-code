namespace AdventOfCode2021.Day24;

public class Day24
{
    private readonly AdventSession _session = new(2021, 24, "Arithmetic Logic Unit");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var stack = new Stack<Block>();
        var max = new int[14];

        var blocks = ParseBlocks(input);

        foreach (var block in blocks)
        {
            if (block.Pop)
            {
                var prev = stack.Pop();
                var expression = prev.PushAdd + block.ConditionAdd;
                max[block.Number] = expression > 0 ? 9 : 9 + expression;
                max[prev.Number] = expression > 0 ? 9 - expression : 9;
            }
            else
            {
                stack.Push(block);
            }
        }

        var answer = long.Parse(string.Join("", max.Select(x => x.ToString())));

        var commands = ParseInput(input);
        var result = Run(answer, commands);
        result.ShouldBe(0);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(92967699949891);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var stack = new Stack<Block>();
        var max = new int[14];

        var blocks = ParseBlocks(input);

        foreach (var block in blocks)
        {
            if (block.Pop)
            {
                var prev = stack.Pop();
                var expression = prev.PushAdd + block.ConditionAdd;
                max[block.Number] = expression > 0 ? 1 + expression : 1;
                max[prev.Number] = expression > 0 ? 1 : 1 - expression;
            }
            else
            {
                stack.Push(block);
            }
        }

        var answer = long.Parse(string.Join("", max.Select(x => x.ToString())));

        var commands = ParseInput(input);
        var result = Run(answer, commands);
        result.ShouldBe(0);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(91411143612181);
    }

    private static int Run(long digit, List<ICommand> commands)
    {
        var modelNumber = digit.ToString();

        var registers = new Dictionary<RegisterLetter, int>
        {
            { RegisterLetter.w, 0 },
            { RegisterLetter.x, 0 },
            { RegisterLetter.y, 0 },
            { RegisterLetter.z, 0 }
        };
        var input = new Queue<int>();
        foreach (var letter in modelNumber)
        {
            input.Enqueue(int.Parse(letter.ToString()));
        }

        foreach (var command in commands)
        {
            command.Run(registers, input);
        }

        return registers[RegisterLetter.z];
    }

    private IEnumerable<Block> ParseBlocks(string input)
    {
        var lines = input.Split("\n");

        for (var i = 0; i < 14; i++)
        {
            var blockLines = lines.Skip(i * 18).Take(18).ToList();
            var pop = blockLines[4].Split(' ')[2];
            var conditionAdd = blockLines[5].Split(' ')[2];
            var pushAdd = blockLines[15].Split(' ')[2];
            yield return new Block(i, pop != "1", int.Parse(conditionAdd), int.Parse(pushAdd));
        }
    }

    private List<ICommand> ParseInput(string input) => input.Split("\n").Select(ParseCommand).ToList();

    private record Block(int Number, bool Pop, int ConditionAdd, int PushAdd);

    private ICommand ParseCommand(string input)
    {
        var sections = input.Split(' ');

        var register = ParseRegisterOrNumber(sections[1]).Register;

        return sections[0] switch
        {
            "inp" => new Inp(register),
            "add" => new Add(register, ParseRegisterOrNumber(sections[2])),
            "mul" => new Mul(register, ParseRegisterOrNumber(sections[2])),
            "div" => new Div(register, ParseRegisterOrNumber(sections[2])),
            "mod" => new Mod(register, ParseRegisterOrNumber(sections[2])),
            "eql" => new Eql(register, ParseRegisterOrNumber(sections[2])),
            _ => throw new ArgumentException("Input was not a known instruction")
        };
    }

    private RegisterOrNumber ParseRegisterOrNumber(string input)
    {
        _ = Enum.TryParse<RegisterLetter>(input, out var register);
        var isNumber = int.TryParse(input, out var number);

        return new RegisterOrNumber(!isNumber, register, number);
    }

    private record RegisterOrNumber(bool IsRegister, RegisterLetter Register, int Number);

    private enum RegisterLetter { w, x, y, z }

    private interface ICommand
    {
        void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input);
    }

    private class Inp : ICommand
    {
        private readonly RegisterLetter _a;

        public Inp(RegisterLetter a) => _a = a;

        public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input) => registers[_a] = input.Dequeue();
    }

    private class Mul : ICommand
    {
        private readonly RegisterLetter _a;
        private readonly RegisterOrNumber _b;


        public Mul(RegisterLetter a, RegisterOrNumber b)
        {
            _a = a;
            _b = b;
        }

        public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input) => registers[_a] *= _b.IsRegister ? registers[_b.Register] : _b.Number;
    }

    private class Add : ICommand
    {
        private readonly RegisterLetter _a;
        private readonly RegisterOrNumber _b;


        public Add(RegisterLetter a, RegisterOrNumber b)
        {
            _a = a;
            _b = b;
        }

        public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input) => registers[_a] += _b.IsRegister ? registers[_b.Register] : _b.Number;
    }

    private class Div : ICommand
    {
        private readonly RegisterLetter _a;
        private readonly RegisterOrNumber _b;


        public Div(RegisterLetter a, RegisterOrNumber b)
        {
            _a = a;
            _b = b;
        }

        public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input) => registers[_a] /= _b.IsRegister ? registers[_b.Register] : _b.Number;
    }

    private class Mod : ICommand
    {
        private readonly RegisterLetter _a;
        private readonly RegisterOrNumber _b;

        public Mod(RegisterLetter a, RegisterOrNumber b)
        {
            _a = a;
            _b = b;
        }

        public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input) => registers[_a] %= _b.IsRegister ? registers[_b.Register] : _b.Number;
    }

    private class Eql : ICommand
    {
        private readonly RegisterLetter _a;
        private readonly RegisterOrNumber _b;


        public Eql(RegisterLetter a, RegisterOrNumber b)
        {
            _a = a;
            _b = b;
        }

        public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input) => registers[_a] = registers[_a] == (_b.IsRegister ? registers[_b.Register] : _b.Number) ? 1 : 0;
    }
}
