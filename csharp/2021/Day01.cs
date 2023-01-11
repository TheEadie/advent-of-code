using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day01
    {
        [Test]
        public void Part1()
        {
            var depths = ParseInput();

            var count = depths
                .Zip(depths.Skip(1), (prev, current) => current > prev)
                .Count(x => x);
            
            Console.WriteLine(count);
            count.ShouldBe(1292);
        }

        [Test]
        public void Part2()
        {
            var depths = ParseInput();

            var slidingWindows = depths
                .Zip(depths.Skip(1), (first, second) => first + second)
                .Zip(depths.Skip(2), (sum, third) => sum + third)
                .ToArray();

            var count = slidingWindows
                .Zip(slidingWindows.Skip(1), (prev, current) => current > prev)
                .Count(x => x);

            Console.WriteLine(count);
            count.ShouldBe(1262);
        }

        private static int[] ParseInput()
        {
            var lines = File.ReadAllLines("Day01.txt");
            var depths = lines.Select(int.Parse).ToArray();
            return depths;
        }
    }
}