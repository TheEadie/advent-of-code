using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day08
    {
        [Test]
        public void Part1()
        {
            var notes = ParseInput();
            var answer = notes.SelectMany(x => x.Result).Count(x => new List<int>{ 2, 3, 4, 7 }.Contains(x.Length));
            
            Console.WriteLine(answer);
            answer.ShouldBe(397);
        }

        [Test]
        public void Part2()
        {
            var notes = ParseInput();

            var total = 0;

            foreach (var note in notes)
            {
                var observed = note.Observed.ToList();
                
                // 1 = length 2
                // 2 = length 5, unique letter
                // 3 = length 5, letters in 1
                // 4 = length 4
                // 5 = length 5, remaining length 5
                // 7 = length 3
                // 0 = length 6, unique letter from set excluding 1 and 7
                // 9 = length 6, all in 4
                // 6 = length 6, remaining length 6
                
                var one = observed.Single(x => x.Length == 2);
                var two = GetOnlyStringWithOneLetterMissing(observed);
                var three = observed.Single(x => x.Length == 5 && one.All(x.Contains));
                var four = observed.Single(x => x.Length == 4);
                var five = observed.Except(new List<string> { two, three}).Single(x => x.Length == 5);
                var seven = observed.Single(x => x.Length == 3);
                var eight = observed.Single(x => x.Length == 7);
                var nine = observed.Single(x => x.Length == 6 && four.All(x.Contains));
                var zero = observed.Except(new List<string> {nine}).Single(x => x.Length == 6 && one.All(x.Contains));
                var six = observed.Except(new List<string> { nine, zero}).Single(x => x.Length == 6);

                var solved = new Dictionary<string, string>();
                solved.Add(one, "1");
                solved.Add(four, "4");
                solved.Add(seven, "7");
                solved.Add(eight, "8");
                solved.Add(two, "2");
                solved.Add(three, "3");
                solved.Add(five, "5");
                solved.Add(zero, "0");
                solved.Add(nine, "9");
                solved.Add(six, "6");

                var signalText = string.Join("", note.Result.Select(x => solved[x]));
            }
            
            


        }

        private string GetOnlyStringWithOneLetterMissing(IEnumerable<string> input)
        {
            var letters = new List<char>() {'a', 'b', 'c', 'd', 'e', 'f', 'g'};
            foreach (var letter in letters)
            {
                var found = input.Where(x => !x.Contains(letter)).ToList();
                if (found.Count == 1)
                {
                    return found.First();
                }
            }

            return "failed";
        }

        private static List<Note> ParseInput()
        {
            var lines = File.ReadAllLines("Day08.txt");
            return lines.Select(x => x.Split(" | ")).Select(x =>
                new Note(x[0].Split(' ', StringSplitOptions.RemoveEmptyEntries),
                    x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))).ToList();
        }

        private record Note(IEnumerable<string> Observed, IEnumerable<string> Result);

        private record Signal(string Value);
    }
}
