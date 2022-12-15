using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day15
{
    [TestCase("data/15 - Sample.txt", 10, 26, TestName = "Sample")]
    [TestCase("data/15 - Puzzle Input.txt", 2_000_000, 5_508_234, TestName = "Puzzle Input")]
    public void Part1(string inputFile, long y, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile))
            .Select(x => new {Sensor = x, Range = GetDistance(x.Position, x.FirstSensor)})
            .ToList();

        var minX = input.Min(x => x.Sensor.Position.X - x.Range);
        var maxX = input.Max(x => x.Sensor.Position.X + x.Range);

        var found = new List<Coordinate>();
        for (var i = minX; i < maxX; i++)
        {
            var test = new Coordinate(i, y);
            if (input
                .Any(x => 
                    x.Range >= GetDistance(test, x.Sensor.Position) &&
                    test != x.Sensor.FirstSensor))
                found.Add(test);
        }
        
        var answer = found.Count;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/15 - Sample.txt", 20, 56_000_011, TestName = "Part 2 - Sample")]
    [TestCase("data/15 - Puzzle Input.txt", 4_000_000, 10_457_634_860_779, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int maxXY, long expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile))
            .Select(x => new {Sensor = x, Range = GetDistance(x.Position, x.FirstSensor)})
            .ToList();


        var borderCoordinates = input.SelectMany(s =>
        {
            var border = new List<Coordinate>();
            var yOffset = s.Range + 1;

            for (var xOffset = 0; xOffset <= s.Range + 1; xOffset++)
            {
                border.Add(new Coordinate(s.Sensor.Position.X + xOffset, s.Sensor.Position.Y + yOffset));
                yOffset--;
            }

            return border;
        });

        var found = borderCoordinates
            .Distinct()
            .Where(x => x.X > 0 && x.X <= maxXY && x.Y > 0 && x.Y <= maxXY)
            .Single(x => input.All(s => s.Range < GetDistance(x, s.Sensor.Position)));
        
        var answer = found.X * 4_000_000 + found.Y;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private IEnumerable<Sensor> ParseInput(string input)
    {
        Sensor ParseSensor (string line)
        {
            var regex = new Regex(
                "Sensor at x=([-|0-9]+), y=([-|0-9]+): closest beacon is at x=([-|0-9]+), y=([-|0-9]+)");
            var groups = regex.Match(line).Groups;

            return new Sensor(
                new Coordinate(int.Parse(groups[1].Value), int.Parse(groups[2].Value)),
                new Coordinate(int.Parse(groups[3].Value), int.Parse(groups[4].Value)));
        }
        
        return input.Split("\n")
            .Select(ParseSensor);
    }

    private static long GetDistance(Coordinate start, Coordinate end)
    {
        return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
    }

    private record Sensor(Coordinate Position, Coordinate FirstSensor);

    private record Coordinate(long X, long Y);
}