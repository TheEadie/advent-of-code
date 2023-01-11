using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day14
    {
        [Test]
        public void Part1()
        {
            var (template, rules) = ParseInput();

            var elementCounts = CountElements(template, rules, 10);

            var mostCommon = elementCounts.MaxBy(x => x.Value).Value;
            var leastCommon = elementCounts.MinBy(x => x.Value).Value;

            var answer = mostCommon - leastCommon;

            Console.WriteLine(answer);
            answer.ShouldBe(2345);
        }

        [Test]
        public void Part2()
        {
            var (template, rules) = ParseInput();

            var elementCounts = CountElements(template, rules, 40);

            var mostCommon = elementCounts.MaxBy(x => x.Value).Value;
            var leastCommon = elementCounts.MinBy(x => x.Value).Value;

            var answer = mostCommon - leastCommon;

            Console.WriteLine(answer);
            answer.ShouldBe(2432786807053);
        }

        private static Dictionary<char, long> CountElements(string template, IDictionary<string, char> rules, int iterations)
        {
            var initialPairCount = template
                .Zip(template.Skip(1), (a,b) => new string($"{a}{b}"))
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => (long)x.Count());

            var sequence = Run(initialPairCount, rules).Take(iterations).Last();

            var elementCounts = sequence
                .Select(x => new { element = x.Key[0], count = x.Value })
                .GroupBy(x => x.element)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.count));

            elementCounts[template.Last()] = elementCounts.GetValueOrDefault(template.Last()) + 1;
            return elementCounts;
        }

        private static IEnumerable<IDictionary<string, long>> Run(IDictionary<string, long> pairCounts, IDictionary<string, char> rules)
        {
            while (true)
            {
                var newPairCounts = new Dictionary<string, long>();

                foreach (var pairCount in pairCounts)
                {
                    var a = pairCount.Key[0];
                    var b = pairCount.Key[1];
                    var n = rules[pairCount.Key];

                    newPairCounts[$"{a}{n}"] = newPairCounts.GetValueOrDefault($"{a}{n}") + pairCount.Value;
                    newPairCounts[$"{n}{b}"] = newPairCounts.GetValueOrDefault($"{n}{b}") + pairCount.Value;
                }

                pairCounts = newPairCounts;

                yield return newPairCounts;
            }
        }


        private static (string, IDictionary<string, char>) ParseInput()
        {

            var lines = File.ReadAllLines("Day14.txt");

            var template = lines[0];

            var rules = lines.Skip(2)
                .Select(x => x.Split(" -> "))
                .Select(x => new KeyValuePair<string, string>(x[0], x[1]))
                .ToDictionary(rule => rule.Key, rule => rule.Value[0]);

            return (template, rules);
        }
    }
}
