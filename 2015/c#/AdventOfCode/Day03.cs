using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day03
    {
        [Test]
        public void Part1()
        {
            var directions = ParseInput().ToList();
            var visited = GetVisitedHouses(directions);

            var answer = visited.GroupBy(x => x).Count();
            Console.WriteLine(answer);
            answer.ShouldBe(2572);
        }

        [Test]
        public void Part2()
        {
            var directions = ParseInput().ToList();
            var santa = directions.Where((x, i) => i % 2 == 0);
            var roboSanta = directions.Where((x, i) => i % 2 != 0);

            var visited = GetVisitedHouses(santa);
            visited.AddRange(GetVisitedHouses(roboSanta));

            var answer = visited.GroupBy(x => x).Count();
            Console.WriteLine(answer);
            answer.ShouldBe(2631);
        }

        private static List<Coordinate> GetVisitedHouses(IEnumerable<char> directions)
        {
            var visited = new List<Coordinate>();
            var current = new Coordinate(0, 0);
            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case '^':
                        current = new Coordinate(current.X, current.Y + 1);
                        visited.Add(current);
                        break;
                    case '>':
                        current = new Coordinate(current.X + 1, current.Y);
                        visited.Add(current);
                        break;
                    case '<':
                        current = new Coordinate(current.X - 1, current.Y);
                        visited.Add(current);
                        break;
                    case 'v':
                        current = new Coordinate(current.X, current.Y - 1);
                        visited.Add(current);
                        break;
                }
            }

            return visited;
        }

        private static IEnumerable<char> ParseInput()
        {
            var lines = File.ReadAllLines("Day03.txt");
            return lines[0].ToCharArray();
        }

        private record Coordinate(int X, int Y);
    }


}
