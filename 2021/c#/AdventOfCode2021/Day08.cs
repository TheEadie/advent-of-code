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

            var answer = notes.Sum(GetSignal);

            Console.WriteLine(answer);
            answer.ShouldBe(1027422);

        }

        private static int GetSignal(Note note)
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
            var five = observed.Except(new List<string> {two, three}).Single(x => x.Length == 5);
            var seven = observed.Single(x => x.Length == 3);
            var eight = observed.Single(x => x.Length == 7);
            var nine = observed.Single(x => x.Length == 6 && four.All(x.Contains));
            var zero = observed.Except(new List<string> {nine}).Single(x => x.Length == 6 && one.All(x.Contains));
            var six = observed.Except(new List<string> {nine, zero}).Single(x => x.Length == 6);

            var solved = new Dictionary<string, string>
            {
                {one, "1"},
                {two, "2"},
                {three, "3"},
                {four, "4"},
                {five, "5"},
                {six, "6"},
                {seven, "7"},
                {eight, "8"},
                {nine, "9"},
                {zero, "0"}
            };

            var signalText = string.Join("", note.Result.Select(x => solved[x]));

            return int.Parse(signalText);
        }

        private static string GetOnlyStringWithOneLetterMissing(IEnumerable<string> input)
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
                new Note(x[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortString),
                    x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SortString))).ToList();
        }

        private record Note(IEnumerable<string> Observed, IEnumerable<string> Result);

        private record Signal(string Value);

        private static string SortString(string input)
        {
            var characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }
    }
}
