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
            var input = ParseInput();
            var gamma = string.Empty;

            for (var position = 0; position < input[0].Length; position++)
            {
                var ones = NumberOfBits(input, '1', position);
                var zeros = NumberOfBits(input, '0', position);
                gamma += ones > zeros ? '1' : '0';
            }

            var epsilon = new string(gamma.Select(x => x == '1' ? '0' : '1').ToArray());

            var gammaAsNumber = Convert.ToInt32(gamma, 2);
            var epsilonAsNumber = Convert.ToInt32(epsilon, 2);

            var answer = gammaAsNumber * epsilonAsNumber;
            Console.WriteLine(answer);
            answer.ShouldBe(3429254);
        }

        [Test]
        public void Part2()
        {
            var oxygenInput = ParseInput().ToList();

            for (var position = 0; position < oxygenInput[0].Length; position++)
            {
                var ones = NumberOfBits(oxygenInput, '1', position);
                var zeros = NumberOfBits(oxygenInput, '0', position);
                oxygenInput.RemoveAll(x => x[position] == (ones >= zeros ? '0' : '1'));

                if (oxygenInput.Count == 1) break;
            }

            var oxygen = new string(oxygenInput[0]);

            var co2Input = ParseInput().ToList();

            for (var position = 0; position < co2Input[0].Length; position++)
            {
                var ones = NumberOfBits(co2Input, '1', position);
                var zeros = NumberOfBits(co2Input, '0', position);
                co2Input.RemoveAll(x => x[position] == (ones >= zeros ? '1' : '0'));

                if (co2Input.Count == 1) break;
            }

            var co2 = new string(co2Input[0]);

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

        private static int NumberOfBits(IEnumerable<char[]> input, char bit, int position)
        {
            return input.Select(x => x[position]).Count(x => x == bit);
        }
    }
}
