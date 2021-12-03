using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day03
    {
        [Test]
        public void Part1()
        {
            var grid = ParseInput();
            var gamma = string.Empty;
            var epsilon = string.Empty;


            for (var j = 0; j < grid[0].Length; j++)
            {
                var ones = 0;
                var zeros = 0;
                for (var i = 0; i < grid.Length; i++)
                {
                    if (grid[i][j] == '1')
                        ones++;
                    if (grid[i][j] == '0')
                        zeros++;
                }

                if (ones > zeros)
                {
                    gamma += '1';
                    epsilon += '0';
                }
                else
                {
                    gamma += '0';
                    epsilon += '1';
                }
            }

            var gammaAsNumber = Convert.ToInt32(gamma, 2);
            var epsilonAsNumber = Convert.ToInt32(epsilon, 2);


            var answer = gammaAsNumber * epsilonAsNumber;
            Console.WriteLine(answer);
            answer.ShouldBe(3429254);
        }

        [Test]
        public void Part2()
        {
            var grid = ParseInput();

            // for each position j
            // lines = most common of 0 or 1 at current position


            for (var j = 0; j < grid[0].Length; j++)
            {
                var ones = 0;
                var zeros = 0;
                var binary1 = new List<char[]>();
                var binary0 = new List<char[]>();

                for (var i = 0; i < grid.Length; i++)
                {
                    if (grid[i][j] == '1')
                    {
                        ones++;
                        binary1.Add(grid[i]);
                    }

                    if (grid[i][j] == '0')
                    {
                        zeros++;
                        binary0.Add(grid[i]);
                    }
                }

                if (ones >= zeros)
                {
                    grid = binary1.ToArray();
                }
                else
                {
                    grid = binary0.ToArray();
                }
            }

            var oxygen = new string(grid[0]);

            grid = ParseInput();

            for (var j = 0; j < grid[0].Length; j++)
            {
                var ones = 0;
                var zeros = 0;
                var binary1 = new List<char[]>();
                var binary0 = new List<char[]>();

                for (var i = 0; i < grid.Length; i++)
                {
                    if (grid[i][j] == '1')
                    {
                        ones++;
                        binary1.Add(grid[i]);
                    }

                    if (grid[i][j] == '0')
                    {
                        zeros++;
                        binary0.Add(grid[i]);
                    }
                }

                if (ones >= zeros)
                {
                    if (binary0.Count == 0)
                        break;
                    grid = binary0.ToArray();
                }
                else
                {
                    if (binary1.Count == 0)
                        break;
                    grid = binary1.ToArray();
                }
            }

            var co2 = new string(grid[0]);

            var oxygenAsNumber = Convert.ToInt32(oxygen, 2);
            var co2AsNumber = Convert.ToInt32(co2, 2);
            var answer = oxygenAsNumber * co2AsNumber;

            Console.WriteLine(answer);
            answer.ShouldBe(5410338);
        }

        private static char[][] ParseInput()
        {
            var lines = File.ReadAllLines("Day03.txt");
            var characters = lines.Select(x => x.ToCharArray()).ToArray();
            return characters;
        }
    }
}
