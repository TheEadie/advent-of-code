using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2015
{
    public class Day01
    {
        [Test]
        public void Part1()
        {
            var moves = ParseInput();

            var answer = moves.Sum();
            Console.WriteLine(answer);
            answer.ShouldBe(138);
        }

        [Test]
        public void Part2()
        {
            var moves = ParseInput();
            var currentFloor = 0;
            var answer = 0;
            for(var i = 0; i < moves.Length; i++)
            {
                currentFloor += moves[i];
                if (currentFloor == -1)
                {
                    answer = i + 1;
                    break;
                }
            }

            Console.WriteLine(answer);
            answer.ShouldBe(1771);
        }

        private static int[] ParseInput()
        {
            var lines = File.ReadAllLines("Day01.txt");
            return lines[0].ToCharArray().Select(x => (x == '(') ? 1 : -1).ToArray();
        }
    }
}
