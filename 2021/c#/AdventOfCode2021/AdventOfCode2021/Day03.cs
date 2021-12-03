using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
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
            var gammaString = string.Empty;

            for (var position = 0; position < input[0].Length; position++)
            {
                var ones = NumberOfBits(input, '1', position);
                var zeros = NumberOfBits(input, '0', position);
                gammaString += ones > zeros ? '1' : '0';
            }

            var gammaBinary = new Binary(gammaString);

            var gamma = gammaBinary.ToInt();
            var epsilon = BinaryUtils.Invert(gammaBinary).ToInt();

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
                oxygenInput.RemoveAll(x => x.GetBit(position) == (ones >= zeros ? '0' : '1'));

                if (oxygenInput.Count == 1) break;
            }

            var oxygen = oxygenInput[0].ToInt();

            var co2Input = ParseInput().ToList();

            for (var position = 0; position < co2Input[0].Length; position++)
            {
                var ones = NumberOfBits(co2Input, '1', position);
                var zeros = NumberOfBits(co2Input, '0', position);
                co2Input.RemoveAll(x => x.GetBit(position) == (ones >= zeros ? '1' : '0'));

                if (co2Input.Count == 1) break;
            }

            var co2 = co2Input[0].ToInt();

            var answer = oxygen * co2;
            Console.WriteLine(answer);
            answer.ShouldBe(5410338);
        }

        private static Binary[] ParseInput()
        {
            var lines = File.ReadAllLines("Day03.txt");
            var characters = lines.Select(x => new Binary(x)).ToArray();
            return characters;
        }

        private static int NumberOfBits(IEnumerable<Binary> input, char bit, int position)
        {
            return input.Select(x => x.GetBit(position)).Count(x => x == bit);
        }
    }
}
