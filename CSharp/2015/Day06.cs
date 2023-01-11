using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2015
{
    public class Day06
    {
        [Test]
        public void Part1()
        {
            var commands = ParseInput();
            var lights = new Dictionary<Coordinate, bool>();

            foreach (var command in commands)
            {
                switch (command.What)
                {
                    case What.On:
                        for (var x = command.Start.X; x <= command.End.X; x++)
                        {
                            for(var y = command.Start.Y; y <= command.End.Y; y++)
                            {
                                lights[new Coordinate(x, y)] = true;
                            }
                        }
                        break;
                    case What.Off:
                        for (var x = command.Start.X; x <= command.End.X; x++)
                        {
                            for(var y = command.Start.Y; y <= command.End.Y; y++)
                            {
                                lights[new Coordinate(x, y)] = false;
                            }
                        }
                        break;
                    case What.Toggle:
                        for (var x = command.Start.X; x <= command.End.X; x++)
                        {
                            for(var y = command.Start.Y; y <= command.End.Y; y++)
                            {
                                var coordinate = new Coordinate(x, y);
                                if (lights.ContainsKey(coordinate))
                                {
                                    lights[coordinate] = !lights[coordinate];
                                }
                                else
                                {
                                    lights[coordinate] = true;
                                }
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var answer = lights.Count(x => x.Value);
            Console.WriteLine(answer);
            answer.ShouldBe(377891);
        }

        [Test]
        public void Part2()
        {
            var commands = ParseInput();
            var lights = new Dictionary<Coordinate, int>();

            foreach (var command in commands)
            {
                switch (command.What)
                {
                    case What.On:
                        for (var x = command.Start.X; x <= command.End.X; x++)
                        {
                            for(var y = command.Start.Y; y <= command.End.Y; y++)
                            {
                                var coordinate = new Coordinate(x, y);
                                if (lights.ContainsKey(coordinate))
                                {
                                    lights[coordinate] += 1;
                                }
                                else
                                {
                                    lights[coordinate] = 1;
                                }
                            }
                        }
                        break;
                    case What.Off:
                        for (var x = command.Start.X; x <= command.End.X; x++)
                        {
                            for(var y = command.Start.Y; y <= command.End.Y; y++)
                            {
                                var coordinate = new Coordinate(x, y);
                                if (lights.ContainsKey(coordinate) &&
                                    lights[coordinate] > 0)
                                {
                                    lights[coordinate] -= 1;
                                }
                                else
                                {
                                    lights[coordinate] = 0;
                                }
                            }
                        }
                        break;
                    case What.Toggle:
                        for (var x = command.Start.X; x <= command.End.X; x++)
                        {
                            for(var y = command.Start.Y; y <= command.End.Y; y++)
                            {
                                var coordinate = new Coordinate(x, y);
                                if (lights.ContainsKey(coordinate))
                                {
                                    lights[coordinate] += 2;
                                }
                                else
                                {
                                    lights[coordinate] = 2;
                                }
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var answer = lights.Sum(x => x.Value);
            Console.WriteLine(answer);
            answer.ShouldBe(14110788);
        }

        private static IEnumerable<Command> ParseInput()
        {
            var lines = File.ReadAllLines("Day06.txt").ToList();

            return lines.Select(ParseCommand);
        }

        private static Command ParseCommand(string input)
        {
            var parts = input.Split(' ');
            if (parts[0] == "toggle")
            {
                return CreateCommand(What.Toggle, parts[1], parts[3]);
            }

            var what = parts[1] == "on" ? What.On : What.Off;
            return CreateCommand(what, parts[2], parts[4]);
        }

        private static Command CreateCommand(What what, string start, string end)
        {
            var startSplit = start.Split(',');
            var endSplit = end.Split(',');
            return new Command(what, new Coordinate(int.Parse(startSplit[0]), int.Parse(startSplit[1])),
                new Coordinate(int.Parse(endSplit[0]), int.Parse(endSplit[1])));
        }

        private record Command(What What, Coordinate Start, Coordinate End);

        private enum What { On, Off, Toggle };

        private record Coordinate(int X, int Y);
    }


}
