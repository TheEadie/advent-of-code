using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day05
    {
        [Test]
        public void Part1()
        {
            var program = File.ReadAllLines("day05.txt")[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new IntCode.IntCode(program);
            
            emulator.Inputs.Enqueue(1);

            emulator.Run();

            foreach (var value in emulator.Output)
            {
                Console.WriteLine(value);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(16225258);
        }

        [Test]
        public void Part2()
        {
            var program = File.ReadAllLines("day05.txt")[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new IntCode.IntCode(program);
            
            emulator.Inputs.Enqueue(5);

            emulator.Run();

            foreach (var value in emulator.Output)
            {
                Console.WriteLine(value);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(2808771);
        }
    }
}
