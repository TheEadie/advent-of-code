using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day19
    {
        [Test]
        public void Part1()
        {
            var locatedScanners = LocateScanners(ParseInput());

            var answer = locatedScanners.SelectMany(x => x.BeaconsInWorld).Distinct().Count();
            Console.WriteLine(answer);
            answer.ShouldBe(479);
        }

        [Test]
        public void Part2()
        {
            var locatedScanners = LocateScanners(ParseInput()).ToList();

            var distances = (
                    from scannerA in locatedScanners
                    from scannerB in locatedScanners
                    select Math.Abs(scannerA.Centre.X - scannerB.Centre.X)
                           + Math.Abs(scannerA.Centre.Y - scannerB.Centre.Y)
                           + Math.Abs(scannerA.Centre.Z - scannerB.Centre.Z));


            var answer = distances.Max();
            Console.WriteLine(answer);
            answer.ShouldBe(13113);
        }

        private IEnumerable<Scanner> LocateScanners(IEnumerable<Scanner> scanners)
        {
            var scannersToLocate = scanners.ToHashSet();
            var locatedScanners = new HashSet<Scanner>();
            var queue = new Queue<Scanner>();

            locatedScanners.Add(scannersToLocate.First());
            queue.Enqueue(scannersToLocate.First());

            scannersToLocate.Remove(scannersToLocate.First());

            while (queue.Any())
            {
                var locatedScanner = queue.Dequeue();
                foreach (var scannerToFind in scannersToLocate)
                {
                    // Get possible rotations
                    var possibleRotations = GetRotations(scannerToFind);

                    // Check for intersection with completed set
                    var found = possibleRotations.Select(x => TryLocate(x, locatedScanner))
                        .Where(x => x is not null)
                        .ToList();

                    if (found.Any())
                    {
                        locatedScanners.Add(found.Single());
                        queue.Enqueue(found.Single());

                        scannersToLocate.Remove(scannerToFind);
                    }
                }
            }

            return locatedScanners;
        }

        private Scanner TryLocate(Scanner scannerToLocate, Scanner scannerAlreadyLocated)
        {
            foreach (var beacon in scannerToLocate.BeaconsInWorld)
            {
                foreach (var alreadyLocatedBeacon in scannerAlreadyLocated.BeaconsInWorld)
                {
                    var translated = scannerToLocate.Translate(new Coordinate3D(
                        beacon.X - alreadyLocatedBeacon.X,
                        beacon.Y - alreadyLocatedBeacon.Y,
                        beacon.Z - alreadyLocatedBeacon.Z));

                    var beaconsInWorld = translated.BeaconsInWorld;

                    if (beaconsInWorld.Intersect(scannerAlreadyLocated.BeaconsInWorld).Count() >= 12)
                        return translated;
                }
            }

            return null;
        }

        private static IEnumerable<Scanner> GetRotations(Scanner scanner)
        {
            var scanners = new List<Scanner>();

            for(var i = 0; i < 24; i++)
            {
                scanners.Add(scanner with { Rotation = i});
            }

            return scanners;
        }

        private static IEnumerable<Scanner> ParseInput()
        {
            var lines = File.ReadAllLines("Day19.txt");

            var scanners = new List<Scanner>();
            var position = 0;

            while (position < lines.Length)
            {
                var header = lines.Skip(position).First();
                var beacons = lines
                    .Skip(position + 1)
                    .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Split(','))
                    .Select(x => new Coordinate3D(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
                    .ToList();
                scanners.Add(new Scanner(header, new Coordinate3D(0,0,0), 0, beacons));
                position += 1 + beacons.Count + 1; // Header + beacons + one blank line
            }

            return scanners;
        }

        private record Scanner(string Name, Coordinate3D Centre, int Rotation, IEnumerable<Coordinate3D> Beacons)
        {
            public Scanner Translate(Coordinate3D coordinate3D) =>
                new(Name,
                    Centre.Translate(new Coordinate3D(-coordinate3D.X, -coordinate3D.Y, -coordinate3D.Z)),
                    Rotation,
                    Beacons);

            public IEnumerable<Coordinate3D> BeaconsInWorld => Beacons.Select(Transform);

            private Coordinate3D Transform(Coordinate3D coordinate) {
                var (x, y, z) = coordinate;

                switch (Rotation % 6) {
                    case 0: (x, y, z) = (x, y, z); break;
                    case 1: (x, y, z) = (-x, y, -z); break;
                    case 2: (x, y, z) = (y, -x, z); break;
                    case 3: (x, y, z) = (-y, x, z); break;
                    case 4: (x, y, z) = (z, y, -x); break;
                    case 5: (x, y, z) = (-z, y, x); break;
                }

                switch ((Rotation / 6) % 4) {
                    case 0: (x, y, z) = (x, y, z); break;
                    case 1: (x, y, z) = (x, -z, y); break;
                    case 2: (x, y, z) = (x, -y, -z); break;
                    case 3: (x, y, z) = (x, z, -y); break;
                }

                return new Coordinate3D(Centre.X + x, Centre.Y + y, Centre.Z + z);
            }
        }

        private record Coordinate3D(int X, int Y, int Z)
        {
            public Coordinate3D Translate(Coordinate3D coordinate3D) =>
                new(X + coordinate3D.X, Y + coordinate3D.Y, Z + coordinate3D.Z);
        }
    }
}
