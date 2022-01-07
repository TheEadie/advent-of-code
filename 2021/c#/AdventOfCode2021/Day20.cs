using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day20
    {
        [Test]
        public void Part1()
        {
            var (lookup, image) = ParseInput();

            image = Enhance(image, lookup, 2);

            var answer = image.Count(x => x.Value);
            Console.WriteLine(answer);
            answer.ShouldBe(5622);
        }

        [Test]
        public void Part2()
        {
            var (lookup, image) = ParseInput();

            image = Enhance(image, lookup, 50);

            var answer = image.Count(x => x.Value);
            Console.WriteLine(answer);
            answer.ShouldBe(20395);
        }

        private static Dictionary<Coordinate, bool> Enhance(Dictionary<Coordinate, bool> image, Dictionary<int, bool> lookup, int numberOfTimes)
        {
            var background = false;
            for (var i = 0; i < numberOfTimes; i++)
            {
                image = Enhance(image, lookup, background);
                background = !background;
            }
            return image;
        }

        private static Dictionary<Coordinate, bool> Enhance(Dictionary<Coordinate, bool> image, Dictionary<int, bool> lookup, bool background)
        {
            var newImage = new Dictionary<Coordinate, bool>();

            var minX = image.Min(i => i.Key.X) - 1;
            var maxX = image.Max(i => i.Key.X) + 1;
            var minY = image.Min(i => i.Key.Y) - 1;
            var maxY = image.Max(i => i.Key.Y) + 1;

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    var score = GetNeighbourhoodScore(new Coordinate(x, y), image, background);
                    newImage.Add(new Coordinate(x, y), lookup[score]);
                }
            }

            return newImage;
        }

        private static int GetNeighbourhoodScore(Coordinate input, Dictionary<Coordinate, bool> image, bool background)
        {
            var testCoordinates = new List<Coordinate>
            {
                new(input.X - 1, input.Y - 1),
                new(input.X - 1, input.Y),
                new(input.X - 1, input.Y + 1),
                new(input.X, input.Y - 1),
                new(input.X, input.Y),
                new(input.X, input.Y + 1),
                new(input.X + 1, input.Y - 1),
                new(input.X + 1, input.Y),
                new(input.X + 1, input.Y + 1)
            };

            var binary = new Binary(testCoordinates.Select(x => image.ContainsKey(x) ? (image[x] ? '1' : '0') : (background ? '1' : '0')).ToArray());
            return binary.ToInt();

        }

        private static (Dictionary<int, bool>, Dictionary<Coordinate, bool>) ParseInput()
        {
            var image = new Dictionary<Coordinate, bool>();
            var lines = File.ReadAllLines("Day20.txt");

            var lookup = lines[0]
                .Select((x, i) => new { On = (x == '#'), Number = i })
                .ToDictionary(x => x.Number, x => x.On);


            var rows = lines.Skip(2).Select(line => line.ToCharArray().Select(x => x == '#').ToList()).ToList();

            var sizeX = rows.Count;
            var sizeY = rows[0].Count();

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    image.Add(new Coordinate(x, y), rows[x][y]);
                }
            }

            return (lookup, image);
        }
    }
}
