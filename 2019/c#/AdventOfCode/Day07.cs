using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day07
    {
        [Test]
        public void Part1()
        {
            var program = File.ReadAllLines("day07.txt")[0]
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var inputs = new List<int> {0, 1, 2, 3, 4};
            var possible = inputs.Permutate();

            var answer = possible.Select(x => GetOutput(program, x)).Max();
            Console.WriteLine(answer);
            answer.ShouldBe(212460);
        }

        private static int GetOutput(int[] program, int[] inputs)
        {
            var ampA = new IntCode.IntCode(program);
            var ampB = new IntCode.IntCode(program);
            var ampC = new IntCode.IntCode(program);
            var ampD = new IntCode.IntCode(program);
            var ampE = new IntCode.IntCode(program);
            
            ampA.Inputs.Enqueue(inputs[0]);
            ampB.Inputs.Enqueue(inputs[1]);
            ampC.Inputs.Enqueue(inputs[2]);
            ampD.Inputs.Enqueue(inputs[3]);
            ampE.Inputs.Enqueue(inputs[4]);
            
            ampA.Inputs.Enqueue(0);
            ampA.Run();
            ampB.Inputs.Enqueue(ampA.Output.First());
            ampB.Run();
            ampC.Inputs.Enqueue(ampB.Output.First());
            ampC.Run();
            ampD.Inputs.Enqueue(ampC.Output.First());
            ampD.Run();
            ampE.Inputs.Enqueue(ampD.Output.First());
            ampE.Run();

            return ampE.Output.First();
        }

        [Test]
        public void Part2()
        {
        }
    }

    public static class Extensions
    {
        public static IEnumerable<T[]> Permutate<T>(this IEnumerable<T> source)
        {
            return permutate(source, Enumerable.Empty<T>());

            IEnumerable<T[]> permutate(IEnumerable<T> reminder, IEnumerable<T> prefix) =>
                !reminder.Any()
                    ? new[] {prefix.ToArray()}
                    : reminder.SelectMany((c, i) => permutate(
                        reminder.Take(i).Concat(reminder.Skip(i + 1)).ToArray(),
                        prefix.Append(c)));
        }
    }
}
