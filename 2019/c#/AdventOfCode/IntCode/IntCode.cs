using System;
using System.Collections.Generic;
using System.Globalization;

namespace AdventOfCode.IntCode;

public class IntCode
{
    private int _pc;
    
    public int[] Memory { get; }
    public Queue<int> Inputs { get; }
    public List<int> Output { get; }

    public IntCode(int[] program)
    {
        Inputs = new Queue<int>();
        Output = new List<int>();
        Memory = (int[]) program.Clone();
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
                default:
                    throw new Exception($"Unknown op code {opCode}");
            }
        }
    }

    private enum ParamMode
    {
        Position,
        Immediate
    }

    private int GetValue(ParamMode mode, int location)
    {
        return mode == ParamMode.Position ? Memory[location] : location;
    }

    private void Add(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = Memory[_pc + 3];
        Memory[c] = a + b;
        _pc += 4;
    }

    private void Multiply(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = Memory[_pc + 3];
        Memory[c] = a * b;
        _pc += 4;
    }

    private void ReadInput(ParamMode modeA)
    {
        var a = Memory[_pc + 1];
        Memory[a] = Inputs.Dequeue();
        _pc += 2;
    }

    private void WriteOutput(ParamMode modeA)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        Output.Add(a);
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
        var c = Memory[_pc + 3];

        Memory[c] = a < b ? 1 : 0;
        _pc += 4;
    }
    
    private void Equals(ParamMode modeA, ParamMode modeB, ParamMode modeC)
    {
        var a = GetValue(modeA, Memory[_pc + 1]);
        var b = GetValue(modeB, Memory[_pc + 2]);
        var c = Memory[_pc + 3];

        Memory[c] = a == b ? 1 : 0;
        _pc += 4;
    }
}