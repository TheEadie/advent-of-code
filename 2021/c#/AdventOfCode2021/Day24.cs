using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day24
    {
        [Test]
        public void Part1()
        {
            const long input = 92967699949891;
            var commands = ParseInput();

            var result = Run(input, commands);
            Console.WriteLine(result);
            result.ShouldBe(0);
        }

        [Test]
        public void Part2()
        {
            const long input = 91411143612181;
            var commands = ParseInput();

            var result = Run(input, commands);
            Console.WriteLine(result);
            result.ShouldBe(0);
        }


        // Block 1
        // w = Read();
        // if (w != stack.Peak() + 11)
        //     stack.Push(w + 3)
        // Block 2
        // w = Read();
        // if (w != stack.Peak() + 14)
        //     stack.Push(w + 7)
        // Block 3
        // w = Read();
        // if (w != stack.Peak() + 13)
        //     stack.Push(w + 6)
        // Block 4
        // w = Read();
        // if (w != stack.Pop() - 4)
        //     stack.Push(w + 6)
        // Block 5
        // w = Read();
        // if (w != stack.Peak() + 11)
        //     stack.Push(w + 14)
        // Block 6
        // w = Read();
        // if (w != stack.Peak() + 10)
        //     stack.Push(w + 7)
        // Block 7
        // w = Read();
        // if (w != stack.Pop() - 4)
        //     stack.Push(w + 9)
        // Block 8
        // w = Read();
        // if (w != stack.Pop() - 12)
        //     stack.Push(w + 9)
        // Block 9
        // w = Read();
        // if (w != stack.Peak() + 10)
        //     stack.Push(w + 6)
        // Block 10
        // w = Read();
        // if (w != stack.Pop() - 11)
        //     stack.Push(w + 4)
        // Block 11
        // w = Read();
        // if (w != stack.Peak() + 12)
        //     stack.Push(w)
        // Block 12
        // w = Read();
        // if (w != stack.Pop() - 1)
        //     stack.Push(w + 7)
        // Block 13
        // w = Read();
        // if (w != stack.Pop())
        //     stack.Push(w + 12)
        // Block 14
        // w = Read();
        // if (w != stack.Pop() - 11)
        //     stack.Push(w + 11)

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

        private List<ICommand> ParseInput()
        {
            return File.ReadAllLines("Day24.txt").Select(ParseCommand).ToList();
        }

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
            var isRegister = Enum.TryParse<RegisterLetter>(input, out var register);
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

            public Inp(RegisterLetter a)
            {
                _a = a;
            }

            public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input)
            {
                registers[_a] = input.Dequeue();
            }
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

            public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input)
            {
                registers[_a] *= _b.IsRegister ? registers[_b.Register] : _b.Number;
            }
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

            public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input)
            {
                registers[_a] += _b.IsRegister ? registers[_b.Register] : _b.Number;
            }
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

            public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input)
            {
                registers[_a] /= _b.IsRegister ? registers[_b.Register] : _b.Number;
            }
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

            public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input)
            {
                registers[_a] %= _b.IsRegister ? registers[_b.Register] : _b.Number;
            }
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

            public void Run(Dictionary<RegisterLetter, int> registers, Queue<int> input)
            {
                registers[_a] = registers[_a] == ( _b.IsRegister ? registers[_b.Register] : _b.Number) ? 1 : 0;
            }
        }
    }

}
