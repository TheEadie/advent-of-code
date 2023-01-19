using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day15;

public class Day15
{
    private readonly AdventSession _session = new(2022, 15);

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", 10, 26)]
    [TestCase("Puzzle Input.txt", 2_000_000, 5_508_234)]
    public async Task Part1(string inputFile, int y, int expected)
    {
        var input = await _session.Start(inputFile);
        var sensors = ParseInput(input).ToList();

        var minX = sensors.Min(x => x.Position.X - x.Distance);
        var maxX = sensors.Max(x => x.Position.X + x.Distance);

        var answer = Enumerable.Range(minX, (maxX - minX))
            .Select(x => new Coordinate(x, y))
            .Count(x => sensors.Any(s => x != s.FirstSensor && s.Distance >= GetDistance(x, s.Position)));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 20, 56_000_011)]
    [TestCase("Puzzle Input.txt", 4_000_000, 10_457_634_860_779)]
    public async Task Part2(string inputFile, int maxXy, long expected)
    {
        var input = await _session.Start(inputFile);
        var sensors = ParseInput(input).ToList();
        var found = sensors
            .SelectMany(GetBorder)
            .Where(x => x.X > 0 && x.X <= maxXy && x.Y > 0 && x.Y <= maxXy)
            .First(x => sensors.All(s => s.Distance < GetDistance(x, s.Position)));

        var answer = (long)found.X * 4_000_000 + found.Y;

        _session.PrintAnswer(2, answer);
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