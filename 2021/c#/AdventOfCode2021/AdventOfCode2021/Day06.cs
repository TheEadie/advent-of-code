using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day06
    {
        [Test]
        public void Part1()
        {
            var answer = Run(ParseInput(), 80);

            Console.WriteLine(answer);
            answer.ShouldBe(362346);
        }

        [Test]
        public void Part2()
        {
            var answer = Run(ParseInput(), 256);

            Console.WriteLine(answer);
            answer.ShouldBe(1639643057051);
        }

        private static long Run(IReadOnlyCollection<int> fish, int days)
        {
            var countOfFish = new long[9];
            for (var i = 0; i <= 8; i++)
            {
                countOfFish[i] = fish.Count(x => x == i);
            }

            for (var i = 0; i < days; i++)
            {
                var temp = countOfFish[0];
                countOfFish[0] = countOfFish[1];
                countOfFish[1] = countOfFish[2];
                countOfFish[2] = countOfFish[3];
                countOfFish[3] = countOfFish[4];
                countOfFish[4] = countOfFish[5];
                countOfFish[5] = countOfFish[6];
                countOfFish[6] = countOfFish[7] + temp;
                countOfFish[7] = countOfFish[8];
                countOfFish[8] = temp;
            }

            return countOfFish[0] + countOfFish[1] + countOfFish[2] + countOfFish[3] + countOfFish[4] +
                   countOfFish[5] + countOfFish[6] + countOfFish[7] + countOfFish[8];
        }

        private static List<int> ParseInput()
        {
            var lines = File.ReadAllLines("Day06.txt");
            var parseOne = lines[0].Split(",").Select(int.Parse);
            return parseOne.ToList();
        }
    }
}
