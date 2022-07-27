using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day02
    {
        [Test]
        public async Task Part1()
        {
            var program = (await File.ReadAllLinesAsync("day02.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            program[1] = 12;
            program[2] = 2;
            
            var emulator = new IntCode.IntCode(program);
            await emulator.RunAsync(CancellationToken.None);
            var answer = emulator.Memory[0];

            Console.WriteLine(answer);
            answer.ShouldBe(3790645);
        }

        [Test]
        public async Task Part2()
        {
            async Task<(int noun, int verb)> FindResult(long[] program, int result)
            {
                for (var n = 0; n < 100; n++)
                {
                    for (var v = 0; v < 100; v++)
                    {
                        program[1] = n;
                        program[2] = v;
                        var emulator = new IntCode.IntCode(program);
                        await emulator.RunAsync(CancellationToken.None);
                        var output = emulator.Memory[0];

                        if (output == result)
                            return (n, v);
                    }
                }

                throw new Exception($"No inputs found that result in {result}");
            }

            var program = (await File.ReadAllLinesAsync("day02.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();
            
            var (noun, verb) = await FindResult(program, 19690720);

            var answer = 100 * noun + verb;
            Console.WriteLine(answer);
            answer.ShouldBe(6577);
        }
    }
}
