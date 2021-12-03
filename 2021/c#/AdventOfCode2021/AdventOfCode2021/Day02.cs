using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day02
    {
        [Test]
        public void Part1()
        {
            var directions = ParseInput();

            var x = 0;
            var depth = 0;

            foreach (var direction in directions)
            {
                switch (direction.Way)
                {
                    case "forward":
                        x += direction.Distance;
                        break;
                    case "up":
                        depth -= direction.Distance;
                        break;
                    case "down":
                        depth += direction.Distance;
                        break;
                }
            }

            var answer = x * depth;
            Console.WriteLine(answer);
            answer.ShouldBe(2117664);
        }

        [Test]
        public void Part2()
        {
            var directions = ParseInput();

            var x = 0;
            var depth = 0;
            var aim = 0;

            foreach (var direction in directions)
            {
                switch (direction.Way)
                {
                    case "forward":
                        x += direction.Distance;
                        depth += aim * direction.Distance;
                        break;
                    case "up":
                        aim -= direction.Distance;
                        break;
                    case "down":
                        aim += direction.Distance;
                        break;
                }
            }

            var answer = x * depth;
            Console.WriteLine(answer);
        }

        private static Direction[] ParseInput()
        {
            var lines = File.ReadAllLines("Day02.txt");
            var depths = lines.Select(ParseLine).ToArray();
            return depths;
        }

        private static Direction ParseLine(string arg)
        {
            var parts = arg.Split(" ");
            return new Direction(parts[0], int.Parse(parts[1]));
        }

        public record Direction (string Way, int Distance);
    }
}
