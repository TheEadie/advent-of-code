using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day22
    {
        [Test]
        public void Part1()
        {
            var commands = ParseInput();

            commands.RemoveAll(x => x.Cube.End.X > 50 || x.Cube.Start.X < -50
                                                      || x.Cube.End.Y > 50 || x.Cube.Start.Y < -50 ||
                                                      x.Cube.End.Z > 50 || x.Cube.Start.Z < -50);

            var answer = HowManyCubesOn(commands);

            Console.WriteLine(answer);
            answer.ShouldBe(615700);

        }

        [Test]
        public void Part2()
        {
            var commands = ParseInput();

            var answer = HowManyCubesOn(commands);

            Console.WriteLine(answer);
            answer.ShouldBe(1236463892941356);
        }

        private long HowManyCubesOn(List<Command> commands)
        {
            var allCubes = new List<Command>();
            foreach (var command in commands)
            {
                var intersections = new List<Command>();

                foreach (var previousCommand in allCubes)
                {
                    if (GetIntersectionVolume(command.Cube, previousCommand.Cube) > 0)
                    {
                        intersections.Add(new Command(GetIntersection(command.Cube, previousCommand.Cube),
                            !previousCommand.On));
                    }
                }

                if (command.On)
                    allCubes.Add(command);
                allCubes.AddRange(intersections);
            }

            var answer = allCubes.Sum(x => x.On ? GetVolume(x.Cube) : -GetVolume(x.Cube));
            return answer;
        }

        private List<Command> ParseInput()
        {
            return File.ReadAllLines("Day22.txt").Select(ParseCommand).ToList();
        }

        private Command ParseCommand(string input)
        {
            var on = input[1] == 'n';
            var xString = input.Split(' ')[1].Split(',')[0];
            var xFrom = int.Parse(xString.Split("..")[0].Split('=')[1]);
            var xTo = int.Parse(xString.Split("..")[1]);

            var yString = input.Split(',')[1];
            var yFrom = int.Parse(yString.Split("..")[0].Split('=')[1]);
            var yTo = int.Parse(yString.Split("..")[1]);

            var zString = input.Split(',')[2];
            var zFrom = int.Parse(zString.Split("..")[0].Split('=')[1]);
            var zTo = int.Parse(zString.Split("..")[1]);

            return new Command(new Cube(new Coordinate3D(xFrom, yFrom, zFrom), new Coordinate3D(xTo, yTo, zTo)), on);
        }


        private long GetIntersectionVolume(Cube a, Cube b)
        {
            var cubes = new List<Cube> {a, b};
            return Math.Max(cubes.Min(x => x.End.X) + 1L - cubes.Max(x => x.Start.X), 0) *
                   Math.Max(cubes.Min(x => x.End.Y) + 1L - cubes.Max(x => x.Start.Y), 0) *
                   Math.Max(cubes.Min(x => x.End.Z) + 1L - cubes.Max(x => x.Start.Z), 0);
        }

        public Cube GetIntersection(Cube a, Cube b)
        {
            var minX = Math.Max(a.Start.X, b.Start.X);
            var maxX = Math.Min(a.End.X, b.End.X);
            var minY = Math.Max(a.Start.Y, b.Start.Y);
            var maxY = Math.Min(a.End.Y, b.End.Y);
            var minZ = Math.Max(a.Start.Z, b.Start.Z);
            var maxZ = Math.Min(a.End.Z, b.End.Z);

            return new Cube(new Coordinate3D(minX, minY, minZ), new Coordinate3D(maxX, maxY, maxZ));
        }

        private long GetVolume(Cube cube)
        {
            return (cube.End.X + 1L - cube.Start.X) * (cube.End.Y + 1L - cube.Start.Y) * (cube.End.Z + 1L - cube.Start.Z);
        }
    }

    public record Command(Cube Cube, bool On);

    public record Cube(Coordinate3D Start, Coordinate3D End);

    public record Coordinate3D(int X, int Y, int Z);
}
