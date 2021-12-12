using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day11
    {
        [Test]
        public void Part1()
        {
            var octopuses = ParseInput();
            var answer = 0;

            for (var i = 0; i < 100; i++)
            {
                answer += Run(octopuses);
            }

            Console.WriteLine(answer);
            answer.ShouldBe(1757);
        }
        
        [Test]
        public void Part2()
        {
            var octopuses = ParseInput();
            var answer = 0;

            while(true)
            {
                answer++;
                if (Run(octopuses) == 100)
                {
                    break;
                }
            }

            Console.WriteLine(answer);
            answer.ShouldBe(422);
        }

        private static int Run(Dictionary<Coordinate, int> octopuses)
        {
            var queue = new Queue<Coordinate>();
            var done = new List<Coordinate>();

            foreach (var pos in octopuses.Keys)
            {
                octopuses[pos]++;
                if (octopuses[pos] > 9)
                {
                    queue.Enqueue(pos);
                }
            }

            while (queue.Any())
            {
                var pos = queue.Dequeue();
                if (done.Contains(pos))
                    continue;

                done.Add(pos);

                foreach (var neighbour in GetNeighbours(pos, octopuses))
                {
                    octopuses[neighbour]++;
                    if (octopuses[neighbour] > 9)
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }

            foreach (var pos in done)
            {
                octopuses[pos] = 0;
            }

            return done.Count;
        }

        private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, IReadOnlyDictionary<Coordinate, int> map)
        {
            var testCoordinates = new List<Coordinate>
            {
                new(input.X - 1, input.Y - 1),
                new(input.X - 1, input.Y),
                new(input.X - 1, input.Y + 1),
                new(input.X, input.Y - 1),
                new(input.X, input.Y + 1),
                new(input.X + 1, input.Y - 1),
                new(input.X + 1, input.Y),
                new(input.X + 1, input.Y + 1)
            };

            return testCoordinates.Where(map.ContainsKey);
        }

        private static Dictionary<Coordinate, int> ParseInput()
        {
            var map = new Dictionary<Coordinate, int>();
            var lines = File.ReadAllLines("Day11.txt");
            var rows = lines.Select(line => line.ToCharArray().Select(x => int.Parse(x.ToString())).ToList()).ToList();

            var sizeX = rows.Count;
            var sizeY = rows[0].Count();
            var heightMap = new int[sizeX, sizeY];

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                     map.Add(new Coordinate(x, y), rows[x][y]);
                }
            }

            return map;
        }
    }
}
