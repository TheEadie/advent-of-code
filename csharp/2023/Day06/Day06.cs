using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day06;

public class Day06
{
    // https://adventofcode.com/2023/day/6
    private readonly AdventSession _session = new(2023, 06, "Wait For It");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 288)]
    [TestCase("Puzzle Input.txt", 741_000)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var lines = input.Split("\n");
        var times = Regex.Matches(lines[0], @"\d+").Select(x => int.Parse(x.Value));
        var records = Regex.Matches(lines[1], @"\d+").Select(x => int.Parse(x.Value));
        var races = times.Select((x, i) => new Race(x, records.ElementAt(i)));

        var answer = races.Select(
            x =>
                {
                    var distances = new List<long>();
                    for (var t = 0; t < x.Time; t++)
                    {
                        distances.Add((x.Time - t) * t);
                    }

                    return distances.Count(d => d > x.Record);
                })
            .Aggregate(1L, (current, x) => current * x);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 71_503)]
    [TestCase("Puzzle Input.txt", 38_220_708)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var lines = input.Split("\n");
        var times = new string(lines[0].Split(":")[1].Where(x => x != ' ').ToArray());
        var records = new string(lines[1].Split(":")[1].Where(x => x != ' ').ToArray());
        var race = new Race(long.Parse(times), long.Parse(records));

        var distances = new List<long>();
        for (var t = 0; t < race.Time; t++)
        {
            distances.Add((race.Time - t) * t);
        }

        var answer = distances.Count(x => x > race.Record);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private record Race(long Time, long Record);
}
