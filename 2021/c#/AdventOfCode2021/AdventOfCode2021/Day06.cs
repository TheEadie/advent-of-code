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
            var fishes = ParseInput();

            for (var i = 0; i < 80; i++)
            {
                var newList = new List<int>();
                foreach (var fish in fishes)
                {
                    if (fish == 0)
                    {
                        newList.Add(6);
                        newList.Add(8);
                    }
                    else
                    {
                        newList.Add(fish - 1);
                    }
                }

                fishes = newList;
            }

            Console.WriteLine(fishes.Count);
            fishes.Count.ShouldBe(362346);
        }

        [Test]
        public void Part2()
        {
            var fishes = ParseInput();

            var answer = Run(fishes);

            Console.WriteLine(answer);
            answer.ShouldBe(26_984_457_539);
        }

        private static long Run(IReadOnlyCollection<int> fish)
        {
            var countOfFish = new long[9];
            for (var i = 0; i <= 8; i++)
            {
                countOfFish[i] = fish.Count(x => x == i);
            }

            for (var i = 0; i < 256; i++)
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
