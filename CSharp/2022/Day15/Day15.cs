using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day15;

public class Day15
{
    [TestCase("Day15/Sample.txt", 10, 26, TestName = "Day 15 - Part 1 - Sample")]
    [TestCase("Day15/Puzzle Input.txt", 2_000_000, 5_508_234, TestName = "Day 15 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int y, int expected)
    {
        var sensors = ParseInput(File.ReadAllText(inputFile)).ToList();

        var minX = sensors.Min(x => x.Position.X - x.Distance);
        var maxX = sensors.Max(x => x.Position.X + x.Distance);

        var answer = Enumerable.Range(minX, (maxX - minX))
            .Select(x => new Coordinate(x, y))
            .Count(x => sensors.Any(s => x != s.FirstSensor && s.Distance >= GetDistance(x, s.Position)));

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day15/Sample.txt", 20, 56_000_011, TestName = "Day 15 - Part 2 - Sample")]
    [TestCase("Day15/Puzzle Input.txt", 4_000_000, 10_457_634_860_779, TestName = "Day 15 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int maxXY, long expected)
    {
        var sensors = ParseInput(File.ReadAllText(inputFile)).ToList();
        var found = sensors
            .SelectMany(GetBorder)
            .Where(x => x.X > 0 && x.X <= maxXY && x.Y > 0 && x.Y <= maxXY)
            .First(x => sensors.All(s => s.Distance < GetDistance(x, s.Position)));

        var answer = (long)found.X * 4_000_000 + found.Y;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Coordinate> GetBorder(Sensor sensor)
    {
        var yOffset = sensor.Distance + 1;

        for (var xOffset = 0; xOffset <= sensor.Distance + 1; xOffset++)
        {
            yield return new Coordinate(sensor.Position.X + xOffset, sensor.Position.Y + yOffset);
            yield return new Coordinate(sensor.Position.X + xOffset, sensor.Position.Y - yOffset);
            yield return new Coordinate(sensor.Position.X - xOffset, sensor.Position.Y + yOffset);
            yield return new Coordinate(sensor.Position.X - xOffset, sensor.Position.Y - yOffset);
            yOffset--;
        }
    }

    private IEnumerable<Sensor> ParseInput(string input)
    {
        Sensor ParseSensor(string line)
        {
            var regex = new Regex(
                "Sensor at x=([-|0-9]+), y=([-|0-9]+): closest beacon is at x=([-|0-9]+), y=([-|0-9]+)");
            var groups = regex.Match(line).Groups;

            var pos = new Coordinate(int.Parse(groups[1].Value), int.Parse(groups[2].Value));
            var firstBeacon = new Coordinate(int.Parse(groups[3].Value), int.Parse(groups[4].Value));

            return new Sensor(pos, firstBeacon, GetDistance(pos, firstBeacon));
        }

        return input.Split("\n").Select(ParseSensor);
    }

    private static int GetDistance(Coordinate start, Coordinate end)
    {
        return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
    }

    private record Sensor(Coordinate Position, Coordinate FirstSensor, int Distance);
}