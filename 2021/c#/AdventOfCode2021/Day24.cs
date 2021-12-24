using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2021
{
    public class Day24
    {
        [Test]
        public void Part1()
        {
            var commands = ParseInput();

            var modelNumber = "99999999999999";

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

            foreach (var register in registers)
            {
                Console.WriteLine($"{register.Key} : {register.Value}");
            }
        }

        [Test]
        public void Part2()
        {
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
