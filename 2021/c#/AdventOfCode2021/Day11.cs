using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var next = octopuses
                    .ToDictionary(octopus => 
                        octopus.Key,
                        octopus => octopus.Value + 1);

                var flashes = next.Where(x => x.Value > 9).ToList();
                var done = new List<Coordinate>();
                var lastLoop = 0;
                
                while (flashes.Count != lastLoop)
                {
                    lastLoop = flashes.Count;
                    flashes
                        .Where(x => !done.Contains(x.Key))
                        .ToList()
                        .ForEach(x => GetNeighbours(x.Key, next)
                        .ToList()
                        .ForEach(y => next[y]++));
                    done.AddRange(flashes.Select(x => x.Key));

                    flashes = next.Where(x => x.Value > 9).ToList();
                }
                
                flashes.ForEach(x => next[x.Key] = 0);
                answer += flashes.Count;

                octopuses = next;
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
                var next = octopuses
                    .ToDictionary(octopus => 
                            octopus.Key,
                        octopus => octopus.Value + 1);

                var flashes = next.Where(x => x.Value > 9).Select(x => x.Key).ToList();
                var done = new List<Coordinate>();
                var lastLoop = 0;
                
                while (flashes.Count != lastLoop)
                {
                    lastLoop = flashes.Count;
                    flashes
                        .Where(x => !done.Contains(x))
                        .ToList()
                        .ForEach(x => GetNeighbours(x, next)
                            .ToList()
                            .ForEach(y => next[y]++));
                    done.AddRange(flashes);

                    flashes = next.Where(x => x.Value > 9).Select(x => x.Key).ToList();
                }
                
                flashes.ForEach(x => next[x] = 0);
                answer++;

                if (flashes.Count == 100)
                {
                    break;
                }

                octopuses = next;
            }

            Console.WriteLine(answer);
            answer.ShouldBe(422);
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
