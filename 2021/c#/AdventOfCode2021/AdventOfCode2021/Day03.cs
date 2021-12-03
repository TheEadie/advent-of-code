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
            var gammaBinary = string.Empty;

            for (var position = 0; position < input[0].Length; position++)
            {
                var ones = NumberOfBits(input, '1', position);
                var zeros = NumberOfBits(input, '0', position);
                gammaBinary += ones > zeros ? '1' : '0';
            }

            var epsilonBinary = new string(gammaBinary.Select(x => x == '1' ? '0' : '1').ToArray());

            var gamma = Convert.ToInt32(gammaBinary, 2);
            var epsilon = Convert.ToInt32(epsilonBinary, 2);

            var answer = gamma * epsilon;
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

            var oxygen = Convert.ToInt32(new string(oxygenInput[0]), 2);

            var co2Input = ParseInput().ToList();

            for (var position = 0; position < co2Input[0].Length; position++)
            {
                var ones = NumberOfBits(co2Input, '1', position);
                var zeros = NumberOfBits(co2Input, '0', position);
                co2Input.RemoveAll(x => x[position] == (ones >= zeros ? '1' : '0'));

                if (co2Input.Count == 1) break;
            }

            var co2 = Convert.ToInt32(new string(co2Input[0]), 2);

            var answer = oxygen * co2;
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
