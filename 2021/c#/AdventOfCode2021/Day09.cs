using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

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
        }

        private bool IsLowest(int x, int y, int[,] heightMap)
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

        [Test]
        public void Part2()
        {

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
