using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day09
    {
        [Test]
        public async Task Part1()
        {
            var program = (await File.ReadAllLinesAsync("day09.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new IntCode.IntCode(program);
            emulator.Inputs.Enqueue(1);

            emulator.Run();

            foreach (var output in emulator.Output)
            {
                Console.WriteLine(output);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(2941952859);
        }
        
        [Test]
        public async Task Part2()
        {
            var program = (await File.ReadAllLinesAsync("day09.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new IntCode.IntCode(program);
            emulator.Inputs.Enqueue(2);

            emulator.Run();

            foreach (var output in emulator.Output)
            {
                Console.WriteLine(output);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(66113);
        }
    }
}
