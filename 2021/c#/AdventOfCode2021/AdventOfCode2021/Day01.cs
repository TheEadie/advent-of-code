using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AdventOfCode2021
{
    public class Day01
    {
        [Test]
        public void Part1()
        {
            var lines = File.ReadAllLines("Day01.txt");
            var depths = lines.Select(int.Parse).ToArray();

            var count = 0;
            var previous = int.MaxValue;

            foreach (var depth in depths)
            {
                if (depth > previous)
                    count++;
                previous = depth;
            }

            Console.WriteLine(count);
        }
        
        [Test]
        public void Part2()
        {
            var lines = File.ReadAllLines("Day01.txt");
            var depths = lines.Select(int.Parse).ToArray();

            var slidingWindows = new List<int>();
            
            for (var i = 0; i < depths.Length - 2; i++)
            {
                var sum = depths[i] + depths[i + 1] + depths[i + 2];
                slidingWindows.Add(sum);
            }

            var count = 0;
            var previous = int.MaxValue;

            foreach (var depth in slidingWindows)
            {
                if (depth > previous)
                    count++;
                previous = depth;
            }

            Console.WriteLine(count);
        }
    }
}