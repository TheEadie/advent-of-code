using System;
using System.Collections.Generic;
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
            var list = ParseInput();

            var nice = list
                .Where(x => x.Count(l => l is 'a' or 'e' or 'i' or 'o' or 'u') >= 3)
                .Where(x => x.Zip(x.Skip(1), (curr, next) => curr == next).Any(d => d))
                .Where(x => !(x.Contains("ab") || x.Contains("cd") || x.Contains("pq") || x.Contains("xy")));


            var answer = nice.Count();

            Console.WriteLine(answer);
            answer.ShouldBe(258);
        }

        [Test]
        public void Part2()
        {
            var list = ParseInput();

            var nice = list
                .Where(x => Enumerable.Range(0, x.Length - 1)
                    .Any(i => x.IndexOf(x.Substring(i, 2), i + 2, StringComparison.Ordinal) >= 0))
                .Where(x => x.Zip(x.Skip(2), (curr, next) => curr == next).Count(x => x) >= 1);


            var answer = nice.Count();

            Console.WriteLine(answer);
            answer.ShouldBe(53);
        }

        private static IEnumerable<string> ParseInput()
        {
            return File.ReadAllLines("Day05.txt").ToList();

        }
    }


}
