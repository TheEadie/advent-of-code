using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2019
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
            var ampA = new AdventOfCode.IntCode.IntCode(program);
            var ampB = new AdventOfCode.IntCode.IntCode(program);
            var ampC = new AdventOfCode.IntCode.IntCode(program);
            var ampD = new AdventOfCode.IntCode.IntCode(program);
            var ampE = new AdventOfCode.IntCode.IntCode(program);
            
            ampA.Inputs.Enqueue(inputs[0]);
            ampB.Inputs.Enqueue(inputs[1]);
            ampC.Inputs.Enqueue(inputs[2]);
            ampD.Inputs.Enqueue(inputs[3]);
            ampE.Inputs.Enqueue(inputs[4]);
            
            ampA.Inputs.Enqueue(0);
            
            var cancellationSource = new CancellationTokenSource();
            var token = cancellationSource.Token;
            
            var ampATask = ampA.RunAsync(token);
            var ampBTask = ampB.RunAsync(token);
            var ampCTask = ampC.RunAsync(token);
            var ampDTask = ampD.RunAsync(token);
            var ampETask = ampE.RunAsync(token);
            
            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
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
            }, token);

            Task.WaitAll(ampATask, ampBTask, ampCTask, ampDTask, ampETask);
            cancellationSource.Cancel();
            return Task.FromResult(ampE.WaitForOutput());
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
            var ampA = new AdventOfCode.IntCode.IntCode(program);
            var ampB = new AdventOfCode.IntCode.IntCode(program);
            var ampC = new AdventOfCode.IntCode.IntCode(program);
            var ampD = new AdventOfCode.IntCode.IntCode(program);
            var ampE = new AdventOfCode.IntCode.IntCode(program);
            
            ampA.Inputs.Enqueue(inputs[0]);
            ampB.Inputs.Enqueue(inputs[1]);
            ampC.Inputs.Enqueue(inputs[2]);
            ampD.Inputs.Enqueue(inputs[3]);
            ampE.Inputs.Enqueue(inputs[4]);
            
            ampA.Inputs.Enqueue(0);
            
            var cancellationSource = new CancellationTokenSource();
            var token = cancellationSource.Token;
            
            var ampATask = ampA.RunAsync(token);
            var ampBTask = ampB.RunAsync(token);
            var ampCTask = ampC.RunAsync(token);
            var ampDTask = ampD.RunAsync(token);
            var ampETask = ampE.RunAsync(token);
            
            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (!ampA.Output.IsEmpty)
                        ampB.Inputs.Enqueue(ampA.WaitForOutput());
                    if (!ampB.Output.IsEmpty)
                        ampC.Inputs.Enqueue(ampB.WaitForOutput());
                    if (!ampC.Output.IsEmpty)
                        ampD.Inputs.Enqueue(ampC.WaitForOutput());
                    if (!ampD.Output.IsEmpty)
                        ampE.Inputs.Enqueue(ampD.WaitForOutput());
                    if (!ampD.Output.IsEmpty)
                        ampE.Inputs.Enqueue(ampD.WaitForOutput());
                    if (!ampE.Output.IsEmpty)
                        ampA.Inputs.Enqueue(ampE.WaitForOutput());
                }
            }, token);

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
