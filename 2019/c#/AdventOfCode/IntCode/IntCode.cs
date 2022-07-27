using System;
using System.Collections.Concurrent;

namespace AdventOfCode.IntCode;

public class IntCode
{
    private long _pc;
    private long _relativeBase;
    
    public long[] Memory { get; }
    public ConcurrentQueue<long> Inputs { get; }
    public ConcurrentQueue<long> Output { get; }

    public IntCode(long[] program)
    {
        Inputs = new ConcurrentQueue<long>();
        Output = new ConcurrentQueue<long>();
        Memory = new long[4096];
        program.CopyTo(Memory, 0);
    }

    public void Run()
    {
        _pc = 0;

        while (Memory[_pc] != 99)
        {
            var opCode = Memory[_pc] % 100;
            var modeA = (ParamMode)((Memory[_pc] / 100) % 10);
            var modeB = (ParamMode)((Memory[_pc] / 1000) % 10);
            var modeC = (ParamMode)((Memory[_pc] / 10000) % 10);

            switch (opCode)
            {
                case 1:
                    Add(modeA, modeB, modeC);
                    break;
                case 2:
                    Multiply(modeA, modeB, modeC);
                    break;
                case 3:
                    ReadInput(modeA);
                    break;
                case 4:
                    WriteOutput(modeA);
                    break;
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
        }
    }

    public long WaitForOutput()
    {
        while(Output.Count == 0)
        {
            // Wait for output
        }

        Output.TryDequeue(out var output);
        return output;
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
            ParamMode.Immediate => location,
            ParamMode.Relative => location + _relativeBase,
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
        while(Inputs.Count == 0)
        {
            // Wait for input
        }

        Inputs.TryDequeue(out var input);
        Memory[a] = input;

        _pc += 2;
    }

    private void WriteOutput(ParamMode modeA)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        Output.Enqueue(a);
        _pc += 2;
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