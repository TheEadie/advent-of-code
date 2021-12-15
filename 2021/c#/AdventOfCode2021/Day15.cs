using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day15
    {
        [Test]
        public void Part1()
        {
            var riskPlane = ParseInput();

            var start = riskPlane.First().Key;
            var goal = riskPlane.Last().Key;

            var queue = new PriorityQueue<Coordinate, int>();

            var gScore = new Dictionary<Coordinate, int>();
            gScore[start] = 0;

            var cameFrom = new Dictionary<Coordinate, Coordinate>();

            var totalPath = new List<Coordinate> { goal };

            queue.Enqueue(start, DistanceToGoal(start, goal));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == goal)
                {
                    while (cameFrom.ContainsKey(current))
                    {
                        current = cameFrom[current];
                        totalPath = totalPath.Prepend(current).ToList();
                    }
                    break;
                }

                foreach (var neighbour in GetNeighbours(current, riskPlane))
                {
                    var tentativeGScore = gScore[current] + riskPlane[neighbour];
                    if (!gScore.ContainsKey(neighbour) || tentativeGScore < gScore[neighbour])
                    {
                        cameFrom[neighbour] = current;
                        gScore[neighbour] = tentativeGScore;
                        queue.Enqueue(neighbour, tentativeGScore + DistanceToGoal(neighbour, goal));
                    }
                }
            }

            var answer = gScore[goal];
            Console.WriteLine(answer);
            answer.ShouldBe(373);
        }

        private int DistanceToGoal(Coordinate current, Coordinate goal)
        {
            return (goal.X - current.X) + (goal.Y - current.Y);
        }

        private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, IReadOnlyDictionary<Coordinate, int> map)
        {
            var testCoordinates = new List<Coordinate>
            {
                new(input.X - 1, input.Y),
                new(input.X, input.Y - 1),
                new(input.X, input.Y + 1),
                new(input.X + 1, input.Y)
            };

            return testCoordinates.Where(map.ContainsKey);
        }

        [Test]
        public void Part2()
        {
            var input = ParseInput();
            var riskPlane = new Dictionary<Coordinate, int>();

            var width = input.MaxBy(x => x.Key.X).Key.X + 1;
            var height = input.MaxBy(x => x.Key.Y).Key.Y + 1;

            foreach (var (key, value) in input)
            {
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        var cell = new Coordinate(key.X + (x * width), key.Y + (y * height));

                        var newRisk = (value + x + y);
                        if (newRisk > 9)
                        {
                            newRisk = (newRisk % 10) + 1;
                        }

                        riskPlane.Add(cell, newRisk);
                    }
                }
            }

            var start = riskPlane.First().Key;
            var goal = riskPlane.Last().Key;

            var queue = new PriorityQueue<Coordinate, int>();

            var gScore = new Dictionary<Coordinate, int>();
            gScore[start] = 0;

            var cameFrom = new Dictionary<Coordinate, Coordinate>();

            var totalPath = new List<Coordinate> { goal };

            queue.Enqueue(start, DistanceToGoal(start, goal));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == goal)
                {
                    while (cameFrom.ContainsKey(current))
                    {
                        current = cameFrom[current];
                        totalPath = totalPath.Prepend(current).ToList();
                    }
                    break;
                }

                foreach (var neighbour in GetNeighbours(current, riskPlane))
                {
                    var tentativeGScore = gScore[current] + riskPlane[neighbour];
                    if (!gScore.ContainsKey(neighbour) || tentativeGScore < gScore[neighbour])
                    {
                        cameFrom[neighbour] = current;
                        gScore[neighbour] = tentativeGScore;
                        queue.Enqueue(neighbour, tentativeGScore + DistanceToGoal(neighbour, goal));
                    }
                }
            }

            var answer = gScore[goal];
            Console.WriteLine(answer);
            answer.ShouldBe(373);
        }

        private static Dictionary<Coordinate, int> ParseInput()
        {
            var map = new Dictionary<Coordinate, int>();
            var lines = File.ReadAllLines("Day15.txt");
            var rows = lines.Select(line => line.ToCharArray().Select(x => int.Parse(x.ToString())).ToList()).ToList();

            var sizeX = rows.Count;
            var sizeY = rows[0].Count();

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
