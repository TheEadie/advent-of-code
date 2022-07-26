using System;
using System.Collections.Generic;

namespace AdventOfCode.IntCode;

public class IntCode
{
    private int _pc;
    
    public int[] Memory { get; }
    public Stack<int> Inputs { get; }
    public List<int> Output { get; }

    public IntCode(int[] program)
    {
        Inputs = new Stack<int>();
        Output = new List<int>();
        Memory = (int[]) program.Clone();
    }

    public void Run()
    {
        _pc = 0;

        while (Memory[_pc] != 99)
        {
            var opCode = Memory[_pc];

            switch (opCode)
            {
                case 1:
                    Add();
                    break;
                case 2:
                    Multiply();
                    break;
                default:
                    throw new Exception("Unknown op code");
            }
        }
    }

    private void Add()
    {
        var a = Memory[Memory[_pc + 1]];
        var b = Memory[Memory[_pc + 2]];
        var c = Memory[_pc + 3];
        Memory[c] = a + b;
        _pc += 4;
    }

    private void Multiply()
    {
        var a = Memory[Memory[_pc + 1]];
        var b = Memory[Memory[_pc + 2]];
        var c = Memory[_pc + 3];
        Memory[c] = a * b;
        _pc += 4;
    }
}