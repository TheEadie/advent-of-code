using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day14
    {
        [Test]
        public void Part1()
        {
            var (template, rules) = ParseInput();

            var sequence = Run(template, rules).Take(10).Last();

            var group = sequence.GroupBy(x => x);
            var mostCommon = group.OrderBy(x => x.Count()).Last();
            var leastCommon = group.OrderBy(x => x.Count()).First();

            var answer = mostCommon.Count() - leastCommon.Count();

            Console.WriteLine(answer);
            answer.ShouldBe(2345);
        }

        private static IEnumerable<string> Run(string template, IDictionary<string, string> rules)
        {
            while (true)
            {
                var result = template
                    .Zip(template.Skip(1), (curr, next) =>
                    {
                        var key = $"{curr}{next}";
                        return (rules.ContainsKey(key) ? $"{curr}{rules[key]}" : $"{curr}");
                    });

                template = string.Join("", result) + template.Last();

                yield return template;
            }
        }

        [Test]
        public void Part2()
        {
            var (template, rules) = ParseInput();

            var sequence = Run(template, rules).Take(40).Last();

            var group = sequence.GroupBy(x => x);
            var mostCommon = group.OrderBy(x => x.Count()).Last();
            var leastCommon = group.OrderBy(x => x.Count()).First();

            long answer = mostCommon.Count() - leastCommon.Count();

            Console.WriteLine(answer);
            answer.ShouldBe(2345);
        }



        private static (string, IDictionary<string, string>) ParseInput()
        {

            var lines = File.ReadAllLines("Day14.txt");

            var template = lines[0];

            var rules = lines.Skip(2)
                .Select(x => x.Split(" -> "))
                .Select(x => new KeyValuePair<string, string>(x[0], x[1]))
                .ToDictionary(rule => rule.Key, rule => rule.Value);

            return (template, rules);
        }
    }
}
