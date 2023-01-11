using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2015
{
    public class Day02
    {
        [Test]
        public void Part1()
        {
            var presents = ParseInput().ToList();

            var presentArea = presents.Sum(x => 2 * x.Length * x.Width + 2 * x.Width * x.Height + 2 * x.Height * x.Length);
            var extra = presents.Sum(x => new List<int> {x.Length * x.Width, x.Width * x.Height, x.Height * x.Length}.Min());

            var answer = presentArea + extra;
            Console.WriteLine(answer);
            answer.ShouldBe(1588178);
        }

        [Test]
        public void Part2()
        {
            var presents = ParseInput().ToList();

            var ribbon = presents.Sum(RibbonForBox);
            var bows = presents.Sum(x => x.Height * x.Length * x.Width);

            var answer = ribbon + bows;
            Console.WriteLine(answer);
            answer.ShouldBe(3783758);
        }

        private static int RibbonForBox(Box box)
        {
            var smallestSides = new List<int> {box.Length, box.Width, box.Height}.OrderBy(x => x).ToList();
            return 2 * (smallestSides[0] + smallestSides[1]);
        }

        private static IEnumerable<Box> ParseInput()
        {
            var lines = File.ReadAllLines("Day02.txt");
            return lines.Select(ParseBox);
        }

        private static Box ParseBox(string input)
        {
            var sections = input.Split('x');
            return new Box(int.Parse(sections[0]), int.Parse(sections[1]), int.Parse(sections[2]));
        }

        private record Box(int Length, int Width, int Height);
    }


}
