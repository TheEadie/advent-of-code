using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day05
    {
        [Test]
        public void Part1()
        {
            var answer = ParseInput()
                .Where(x => x.Start.X == x.End.X || x.Start.Y == x.End.Y)
                .SelectMany(x => x.GetCoordinatesOnLine())
                .GroupBy(x => x)
                .Count(x => x.Count() > 1);

            Console.WriteLine(answer);
            answer.ShouldBe(5145);
        }

        [Test]
        public void Part2()
        {
            var answer = ParseInput()
                .SelectMany(x => x.GetCoordinatesOnLine())
                .GroupBy(x => x)
                .Count(x => x.Count() > 1);

            Console.WriteLine(answer);
            answer.ShouldBe(16518);
        }

        private static IEnumerable<Line> ParseInput()
        {
            var lines = File.ReadAllLines("Day05.txt");
            var parseOne = lines.Select(x => x.Split(" -> ")).Select(ParseVentLine);
            return parseOne.ToList();
        }

        private static Line ParseVentLine(string[] values)
        {
            var firstCoordinates = values[0].Split(',');
            var firstX = int.Parse(firstCoordinates[0]);
            var firstY = int.Parse(firstCoordinates[1]);
            var secondCoordinates = values[1].Split(',');
            var secondX = int.Parse(secondCoordinates[0]);
            var secondY = int.Parse(secondCoordinates[1]);

            return new Line(new Coordinate(firstX, firstY), new Coordinate(secondX, secondY));
        }


    }
}
