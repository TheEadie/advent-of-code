using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day07
    {
        [Test]
        public void Part1()
        {
            var numbers = ParseInput();

            var median = numbers.OrderBy(x => x).ElementAt(numbers.Count / 2);
            var fuelUsed = numbers.Sum(number => Math.Abs(number - median));

            Console.WriteLine(fuelUsed);
            fuelUsed.ShouldBe(323647);
        }

        [Test]
        public void Part2()
        {
            var numbers = ParseInput();

            var mean = numbers.Sum() / numbers.Count;
            var total = numbers.Sum(number => GetFuelUse(Math.Abs(number - mean)));

            Console.WriteLine(total);
            total.ShouldBe(87640209);
        }

        private static int GetFuelUse(int distance)
        {
            var total = 0;
            for (var i = 1; i <= distance; i++)
            {
                total += i;
            }

            return total;
        }

        private static List<int> ParseInput()
        {
            var lines = File.ReadAllLines("Day07.txt");
            return lines[0].Split(",").Select(int.Parse).ToList();
        }
    }
}
