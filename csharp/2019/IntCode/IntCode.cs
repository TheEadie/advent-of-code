namespace AdventOfCode2019.IntCode;

public class IntCode
{
    private long _pc;
    private long _relativeBase;
    private readonly Queue<long> _inputs;

    public long[] Memory { get; }

    public IntCode(long[] program, int memorySpace = 4096)
    {
        _inputs = new Queue<long>();
        Memory = new long[memorySpace];
        program.CopyTo(Memory, 0);
        _pc = 0;
    }

    public void Run(Action<IEnumerable<long>> processOutputs, Func<long> getInput)
    {
        var status = IntCodeStatus.Running;
        long? input = null;

        while (status != IntCodeStatus.Halted)
        {
            (status, var outputs) = Run(input);
            outputs = outputs.ToArray();

            if (outputs.Any())
            {
                processOutputs(outputs);
            }

            if (status == IntCodeStatus.AwaitingInput)
            {
                input = getInput();
            }
        }
    }

    public IntCodeReturn Run(long? input = null)
    {
        var outputs = new List<long>(0);

        var result = RunUntilInputOutputOrHalt(input);
        if (result.Status == IntCodeStatus.OutputAvailable)
        {
            outputs.AddRange(result.Outputs);
        }

        while (result.Status != IntCodeStatus.Halted && result.Status != IntCodeStatus.AwaitingInput)
        {
            result = RunUntilInputOutputOrHalt();
            if (result.Status == IntCodeStatus.OutputAvailable)
            {
                outputs.AddRange(result.Outputs);
            }
        }

        return result with { Outputs = outputs };
    }

    private IntCodeReturn RunUntilInputOutputOrHalt(long? input = null)
    {
        if (input.HasValue)
        {
            _inputs.Enqueue(input.Value);
        }

        while (Memory[_pc] != 99)
        {
            var output = Step();
            if (output.Status != IntCodeStatus.Running)
            {
                return output;
            }
        }

        return new IntCodeReturn(IntCodeStatus.Halted, Array.Empty<long>());
    }

    private IntCodeReturn Step()
    {
        var opCode = Memory[_pc] % 100;
        var modeA = (ParamMode) (Memory[_pc] / 100 % 10);
        var modeB = (ParamMode) (Memory[_pc] / 1000 % 10);
        var modeC = (ParamMode) (Memory[_pc] / 10000 % 10);

        switch (opCode)
        {
            case 1:
                Add(modeA, modeB, modeC);
                break;
            case 2:
                Multiply(modeA, modeB, modeC);
                break;
            case 3:
                if (!_inputs.Any())
                {
                    return new IntCodeReturn(IntCodeStatus.AwaitingInput, Array.Empty<long>());
                }

                ReadInput(modeA);
                break;
            case 4:
                return new IntCodeReturn(IntCodeStatus.OutputAvailable, WriteOutput(modeA));
            case 5:
                JumpIfTrue(modeA, modeB);
                break;
            case 6:
                JumpIfFalse(modeA, modeB);
                break;
            case 7:
                LessThan(modeA, modeB, modeC);
                break;
            case 8:
                Equals(modeA, modeB, modeC);
                break;
            case 9:
                AdjustRelativeBase(modeA);
                break;
            default:
                throw new Exception($"Unknown op code {opCode}");
        }

        return new IntCodeReturn(IntCodeStatus.Running, Array.Empty<long>());
    }

    private enum ParamMode
    {
        Position,
        Immediate,
        Relative
    }

    private long GetValue(ParamMode mode, long location)
    {
        return mode switch
        {
            ParamMode.Position => Memory[location],
            ParamMode.Immediate => location,
            ParamMode.Relative => Memory[location + _relativeBase],
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private long GetWriteAddress(ParamMode mode, long location)
    {
        return mode switch
        {
            ParamMode.Position => location,
            ParamMode.Relative => location + _relativeBase,
            ParamMode.Immediate => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private void Add(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = GetWriteAddress(modeC, Memory[_pc + 3]);
        Memory[c] = a + b;
        _pc += 4;
    }

    private void Multiply(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = GetWriteAddress(modeC, Memory[_pc + 3]);
        Memory[c] = a * b;
        _pc += 4;
    }

    private void ReadInput(ParamMode modeA)
    {
        var a = GetWriteAddress(modeA, Memory[_pc + 1]);
        Memory[a] = _inputs.Dequeue();

        _pc += 2;
    }

    private long[] WriteOutput(ParamMode modeA)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        _pc += 2;
        return new[] { a };
    }

    private void JumpIfTrue(ParamMode modeA, ParamMode modeB)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);

        if (a != 0)
        {
            _pc = b;
        }
        else
        {
            _pc += 3;
        }
    }

    private void JumpIfFalse(ParamMode modeA, ParamMode modeB)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);

        if (a == 0)
        {
            _pc = b;
        }
        else
        {
            _pc += 3;
        }
    }

    private void LessThan(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = GetWriteAddress(modeC, Memory[_pc + 3]);

        Memory[c] = a < b ? 1 : 0;
        _pc += 4;
    }

    private void Equals(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = GetWriteAddress(modeC, Memory[_pc + 3]);

        Memory[c] = a == b ? 1 : 0;
        _pc += 4;
    }

    private void AdjustRelativeBase(ParamMode modeA)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        _relativeBase += a;
        _pc += 2;
    }
}
