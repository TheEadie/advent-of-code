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
            var instructions = File.ReadAllLines("day03.txt");
            var pathOne = instructions[0].Split(',');
            var pathTwo = instructions[1].Split(',');

            var pathOneCoordinates = GetPathCoordinates(pathOne);
            var pathTwoCoordinates = GetPathCoordinates(pathTwo);
            
            var overlap = pathOneCoordinates.Intersect(pathTwoCoordinates);

            var answer =overlap.Select(x => Math.Abs(x.X) + Math.Abs(x.Y)).Min();

            Console.WriteLine(answer);
            answer.ShouldBe(245);
        }

        [Test]
        public void Part2()
        {
        }

        private IEnumerable<Coordinate> GetPathCoordinates(IEnumerable<string> pathOne)
        {
            var pathCoordinates = new List<Coordinate>();
            var current = new Coordinate(0, 0);
            foreach (var line in pathOne)
            {
                var (next, lineCoordinates) = GetLineCoordinates(line, current);
                pathCoordinates.AddRange(lineCoordinates);
                current = next;
            }

            return pathCoordinates;
        }

        private (Coordinate, IEnumerable<Coordinate>) GetLineCoordinates(string input, Coordinate current)
        {
            var length = int.Parse(input[1..]);
            var coordinates = new List<Coordinate>();
            
            switch (input[0])
            {
                case 'U':
                    for (var i = 1; i < length; i++)
                    {
                        coordinates.Add(current with {Y = current.Y + i});
                    }
                    return (current with {Y = current.Y + length}, coordinates);
                case 'D':
                    for (var i = 1; i < length; i++)
                    {
                        coordinates.Add(current with {Y = current.Y - i});
                    }
                    return (current with {Y = current.Y - length}, coordinates);
                case 'R':
                    for (var i = 1; i < length; i++)
                    {
                        coordinates.Add(current with {X = current.X + i});
                    }
                    return (current with {X = current.X + length}, coordinates);
                case 'L':
                    for (var i = 1; i < length; i++)
                    {
                        coordinates.Add(current with {X = current.X - i});
                    }
                    return (current with {X = current.X - length}, coordinates);
                default:
                    throw new ArgumentException($"Unknown direction {input[0]}");
            }
        }

        private record Coordinate(int X, int Y);
    }
}
