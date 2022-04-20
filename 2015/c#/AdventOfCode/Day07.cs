using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode
{
    public class Day07
    {
        [Test]
        public void Part1()
        {
            var commands = ParseInput();

            var answer = (int) commands["a"].Invoke();
            Console.WriteLine(answer);
            answer.ShouldBe(956);
        }

        [Test]
        public void Part2()
        {
            var commands = ParseInput();
            var a = commands["a"].Invoke();
            
            var resetCommands = ParseInput();
            resetCommands["b"] = () => a;

            var answer = (int) resetCommands["a"].Invoke();
            Console.WriteLine(answer);
            answer.ShouldBe(40149);
        }

        private static IDictionary<string, Func<ushort>> ParseInput()
        {
            var lines = File.ReadAllLines("Day07.txt").ToList();
            var commands = new Dictionary<string, Func<ushort>>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                switch (parts[1])
                {
                    case "->":
                        commands.Add(parts[2], () =>
                        {
                            var value = GetNumber(parts[0], commands);
                            commands[parts[2]] = () => value;
                            return value;
                        });
                        break;
                    case "AND":
                        commands.Add(parts[4], () =>
                        {
                            var value = (ushort) (GetNumber(parts[0], commands) & GetNumber(parts[2], commands));
                            commands[parts[4]] = () => value;
                            return value;
                        });
                        break;
                    case "OR":
                        commands.Add(parts[4], () =>
                        {
                            var value = (ushort) (GetNumber(parts[0], commands) | GetNumber(parts[2], commands));
                            commands[parts[4]] = () => value;
                            return value;
                        });
                        break;
                    case "LSHIFT":
                        commands.Add(parts[4], () =>
                        {
                            var value = (ushort) (GetNumber(parts[0], commands) << int.Parse(parts[2]));
                            commands[parts[4]] = () => value;
                            return value;
                        });
                        break;
                    case "RSHIFT":
                        commands.Add(parts[4], () =>
                        {
                            var value = (ushort) (GetNumber(parts[0], commands) >> int.Parse(parts[2]));
                            commands[parts[4]] = () => value;
                            return value;
                        });
                        break;
                }

                if (parts[0] == "NOT")
                {
                    commands.Add(parts[3], () =>
                    {
                        var value = (ushort) ~GetNumber(parts[1], commands);
                        commands[parts[3]] = () => value;
                        return value;
                    });
                }
            }

            return commands;
        }

        private static ushort GetNumber(string text, IReadOnlyDictionary<string, Func<ushort>> commands)
        {
            var number = ushort.TryParse(text, out var value);
            return number ? value : commands[text].Invoke();
        }
    }


}
