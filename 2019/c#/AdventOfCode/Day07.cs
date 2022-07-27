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
    public class Day07
    {
        [Test]
        public async Task Part1()
        {
            var program = (await File.ReadAllLinesAsync("day07.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var inputs = new List<int> {0, 1, 2, 3, 4};
            var possible = inputs.Permutate();

            var answer = (await Task.WhenAll(possible.Select(x => GetOutputPart1(program, x)))).Max();

            Console.WriteLine(answer);
            answer.ShouldBe(212460);
        }
        
        private static Task<long> GetOutputPart1(long[] program, int[] inputs)
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
            var ampATask = Task.Run(() => ampA.Run());
            var ampBTask = Task.Run(() => ampB.Run());
            var ampCTask = Task.Run(() => ampC.Run());
            var ampDTask = Task.Run(() => ampD.Run());
            var ampETask = Task.Run(() => ampE.Run());


            var cancellationSource = new CancellationTokenSource();
            var wiresTask = Task.Run(() => WireUp(cancellationSource.Token), cancellationSource.Token);

            void WireUp(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!ampA.Output.IsEmpty)
                        ampB.Inputs.Enqueue(ampA.WaitForOutput());
                    if (!ampB.Output.IsEmpty)
                        ampC.Inputs.Enqueue(ampB.WaitForOutput());
                    if (!ampC.Output.IsEmpty)
                        ampD.Inputs.Enqueue(ampC.WaitForOutput());
                    if (!ampD.Output.IsEmpty)
                        ampE.Inputs.Enqueue(ampD.WaitForOutput());
                }
            }

            Task.WaitAll(ampATask, ampBTask, ampCTask, ampDTask, ampETask);
            cancellationSource.Cancel();
            return Task.FromResult(ampE.Output.First());
        }

        [Test]
        public async Task Part2()
        {
            var program = (await File.ReadAllLinesAsync("day07.txt"))[0]
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var inputs = new List<int> {5, 6, 7, 8, 9};
            var possible = inputs.Permutate();

            var answer = (await Task.WhenAll(possible.Select(x => GetOutputPart2(program, x)))).Max();

            Console.WriteLine(answer);
            answer.ShouldBe(21844737);
        }
        
        private static Task<long> GetOutputPart2(long[] program, int[] inputs)
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
            var ampATask = Task.Run(() => ampA.Run());
            var ampBTask = Task.Run(() => ampB.Run());
            var ampCTask = Task.Run(() => ampC.Run());
            var ampDTask = Task.Run(() => ampD.Run());
            var ampETask = Task.Run(() => ampE.Run());


            var cancellationSource = new CancellationTokenSource();
            var wiresTask = Task.Run(() => WireUp(cancellationSource.Token), cancellationSource.Token);

            void WireUp(CancellationToken cancellationToken)
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!ampA.Output.IsEmpty)
                        ampB.Inputs.Enqueue(ampA.WaitForOutput());
                    if (!ampB.Output.IsEmpty)
                        ampC.Inputs.Enqueue(ampB.WaitForOutput());
                    if (!ampC.Output.IsEmpty)
                        ampD.Inputs.Enqueue(ampC.WaitForOutput());
                    if (!ampD.Output.IsEmpty)
                        ampE.Inputs.Enqueue(ampD.WaitForOutput());
                    if (!ampE.Output.IsEmpty)
                        ampA.Inputs.Enqueue(ampE.WaitForOutput());
                }
            }

            Task.WaitAll(ampATask, ampBTask, ampCTask, ampDTask, ampETask);
            cancellationSource.Cancel();
            return Task.FromResult(ampA.Inputs.First());
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
