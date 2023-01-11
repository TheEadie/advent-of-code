using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2019
{
    public class Day05
    {
        [Test]
        public async Task Part1()
        {
            var program = (await File.ReadAllLinesAsync("day05.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new AdventOfCode.IntCode.IntCode(program);
            
            emulator.Inputs.Enqueue(1);

            await emulator.RunAsync(CancellationToken.None);

            foreach (var value in emulator.Output)
            {
                Console.WriteLine(value);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(16225258);
        }

        [Test]
        public async Task Part2()
        {
            var program = (await File.ReadAllLinesAsync("day05.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new AdventOfCode.IntCode.IntCode(program);
            
            emulator.Inputs.Enqueue(5);

            await emulator.RunAsync(CancellationToken.None);

            foreach (var value in emulator.Output)
            {
                Console.WriteLine(value);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(2808771);
        }
    }
}
