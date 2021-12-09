using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day09
    {
        [Test]
        public void Part1()
        {
            var heightMap = ParseInput();

            var sizeX = heightMap.GetLength(0);
            var sizeY = heightMap.GetLength(1);

            var answer = 0;

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    var minPoint = IsLowest(x, y, heightMap);
                    if (minPoint)
                    {
                        answer += heightMap[x, y] + 1;
                    }
                }
            }

            Console.WriteLine(answer);
            answer.ShouldBe(566);
        }

        [Test]
        public void Part2()
        {
            var heightMap = ParseInput();

            var sizeX = heightMap.GetLength(0);
            var sizeY = heightMap.GetLength(1);

            var basins = new Dictionary<Coordinate, int>();

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    var minPoint = IsLowest(x, y, heightMap);
                    if (minPoint)
                    {
                        var coordinate = new Coordinate(x, y);
                        var neighboursInBasin = NeighboursInBasin(coordinate, heightMap);
                        basins.Add(coordinate, neighboursInBasin.Count());
                    }
                }
            }

            var topThree = basins.OrderByDescending(x => x.Value).Take(3);
            var answer = topThree.Aggregate(1, (a, x) => a * x.Value);

            Console.WriteLine(answer);
            answer.ShouldBe(891684);

        }

        private static bool IsLowest(int x, int y, int[,] heightMap)
        {
            var lowest = true;

            if (x > 0)
                lowest = heightMap[x - 1, y] > heightMap[x, y];
            if (x < heightMap.GetLength(0) - 1)
                lowest = lowest && heightMap[x + 1, y] > heightMap[x, y];
            if (y > 0)
                lowest = lowest && heightMap[x, y - 1] > heightMap[x, y];
            if (y < heightMap.GetLength(1) - 1)
                lowest = lowest && heightMap[x, y + 1] > heightMap[x, y];

            return lowest;

        }

        private static IEnumerable<Coordinate> NeighboursInBasin(Coordinate input, int[,] heightMap)
        {
            var inBasin = new List<Coordinate>{input};
            var neighbours = new List<Coordinate>();
            var testCoordinates = new List<Coordinate>();

            if (input.X > 0)
                testCoordinates.Add(new Coordinate(input.X - 1, input.Y));
            if (input.X < heightMap.GetLength(0) - 1)
                testCoordinates.Add(new Coordinate(input.X + 1, input.Y));
            if (input.Y > 0)
                testCoordinates.Add(new Coordinate(input.X, input.Y - 1));
            if (input.Y < heightMap.GetLength(1) - 1)
                testCoordinates.Add(new Coordinate(input.X, input.Y + 1));

            neighbours.AddRange(testCoordinates.Where(testCoordinate =>
                heightMap[testCoordinate.X, testCoordinate.Y] > heightMap[input.X, input.Y] &&
                heightMap[testCoordinate.X, testCoordinate.Y] != 9));

            inBasin.AddRange(neighbours.SelectMany(x => NeighboursInBasin(x, heightMap)));

            return inBasin.Distinct().ToList();

        }

        private static int[,] ParseInput()
        {
            var lines = File.ReadAllLines("Day09.txt");
            var rows = lines.Select(line => line.ToCharArray().Select(x => int.Parse(x.ToString())).ToList()).ToList();

            var sizeX = rows.Count;
            var sizeY = rows[0].Count();
            var heightMap = new int[sizeX, sizeY];

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    heightMap[x, y] = rows[x][y];
                }
            }

            return heightMap;
        }
    }
}
