using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day01
    {
        [Test]
        public void Part1()
        {
            var answer = File.ReadAllLines("Day01.txt")
                .Select(int.Parse)
                .Select(GetFuelForModule)
                .Sum();

            Console.WriteLine(answer);
            answer.ShouldBe(3401852);
        }

        [Test]
        public void Part2()
        {
            var answer = File.ReadAllLines("Day01.txt")
                .Select(int.Parse)
                .Select(GetTotalFuel)
                .Sum();
            
            Console.WriteLine(answer);
            answer.ShouldBe(5099916);
        }

        private static int GetTotalFuel(int moduleSize)
        {
            var fuel = GetFuelForModule(moduleSize);

            if (fuel >= 0) return fuel + GetTotalFuel(fuel);
            fuel = 0;
            return fuel;

        }

        private static int GetFuelForModule(int moduleSize)
        {
            return (int) Math.Round((double) moduleSize / 3, 0, MidpointRounding.ToZero) - 2;
        }
    }
}