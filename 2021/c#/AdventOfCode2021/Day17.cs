using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode2021.Utils;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day17
    {
        [Test]
        public void Part1()
        {
            var (_, areaBottomRight) = ParseInput();

            var maxY = GetMaxYVelocity(areaBottomRight.Y);
            var answer = GetMaxHeight(maxY);

            Console.WriteLine(answer);
            answer.ShouldBe(8256);
        }

        [Test]
        public void Part2()
        {
            var (areaTopLeft, areaBottomRight) = ParseInput();

            var minX = GetMinXVelocity(areaTopLeft.X, areaBottomRight.X);
            var maxX = areaBottomRight.X;
            var minY = GetMinYVelocity(areaBottomRight.Y);
            var maxY = GetMaxYVelocity(areaBottomRight.Y);


            var valid = new List<Coordinate>();

            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    if (LandsInTarget(x, y, areaTopLeft, areaBottomRight))
                        valid.Add(new Coordinate(x, y));
                }
            }

            var answer = valid.Count();
            Console.WriteLine(answer);
            answer.ShouldBe(2326);
        }

        private bool LandsInTarget(int vx, int vy, Coordinate topLeft, Coordinate bottomRight)
        {
            var x = 0;
            var y = 0;
            var t = 0;

            var maxX = bottomRight.X;
            var maxY = bottomRight.Y;

            while (x <= maxX && y >= maxY)
            {
                x += (vx - t > 0) ? vx - t : 0;
                y += vy - t;
                t++;

                if (x >= topLeft.X && x <= maxX && y <= topLeft.Y && y >= maxY)
                    return true;
            }

            return false;
        }

        private static int GetMaxYVelocity(int bottomY)
        {
            return -bottomY - 1;
        }

        private static int GetMinYVelocity(int bottomY)
        {
            return bottomY;
        }

        private int GetMinXVelocity(int lower, int upper)
        {
            return Enumerable.Range(0, 100).Where(x => lower <= GetLimitOfX(x) && GetLimitOfX(x) <= upper).Min() - 1;
        }

        private int GetLimitOfX(int vx)
        {
            return (vx / 2) * (1 + vx);
        }

        private int GetMaxHeight(int vy)
        {
            return (vy / 2) * (1 + vy);
        }


        private static (Coordinate, Coordinate) ParseInput()
        {
            var topLeft = new Coordinate(150, -70);
            var bottomRight = new Coordinate(171, -129);

            //var topLeft = new Coordinate(20, -5);
            //var bottomRight = new Coordinate(30, -10);

            return (topLeft, bottomRight);
        }

    }
}
