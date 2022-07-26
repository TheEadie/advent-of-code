using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day02
    {
        [Test]
        public void Part1()
        {
            var program = File.ReadAllLines("day02.txt")[0]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            program[1] = 12;
            program[2] = 2;
            
            var emulator = new IntCode.IntCode(program);
            emulator.Run();
            var answer = emulator.Memory[0];

            Console.WriteLine(answer);
            answer.ShouldBe(3790645);
        }

        [Test]
        public void Part2()
        {
            (int noun, int verb) FindResult(int[] program, int result)
            {
                for (var n = 0; n < 100; n++)
                {
                    for (var v = 0; v < 100; v++)
                    {
                        program[1] = n;
                        program[2] = v;
                        var emulator = new IntCode.IntCode(program);
                        emulator.Run();
                        var output = emulator.Memory[0];

                        if (output == result)
                            return (n, v);
                    }
                }

                throw new Exception($"No inputs found that result in {result}");
            }

            var program = File.ReadAllLines("day02.txt")[0]
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            
            var (noun, verb) = FindResult(program, 19690720);

            var answer = 100 * noun + verb;
            Console.WriteLine(answer);
            answer.ShouldBe(6577);
        }
    }
}
