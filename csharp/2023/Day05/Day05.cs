namespace AdventOfCode2023.Day05;

public class Day05
{
    // https://adventofcode.com/2023/day/5
    private readonly AdventSession _session = new(2023, 05, "If You Give A Seed A Fertilizer");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 35)]
    [TestCase("Puzzle Input.txt", 322_500_873)]
    public async Task Part1(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var almanac = Parse(input);

        var answer = almanac.Maps
            .Aggregate(almanac.Seeds, (current, map) =>
                current.Select(x => ApplyMap(x, map)).ToList())
            .Min();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 46)]
    [TestCase("Puzzle Input.txt", 108_956_227)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var almanac = Parse(input);

        var seedRanges = almanac.Seeds
            .Chunk(2)
            .Select(x => new Range(x[0], x[0] + x[1] - 1));

        var answer = almanac.Maps
            .Aggregate(seedRanges, (current, map) =>
                    Intersect(current, map.Select(x => x.Source))
                        .Select(x => new Range(ApplyMap(x.Start, map), ApplyMap(x.End, map)))
                        .ToList())
            .Min(x => x.Start);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Range> Intersect(IEnumerable<Range> a, IEnumerable<Range> b) =>
        a.SelectMany(x => b.SelectMany(y => Intersect(x, y)));

    private static IEnumerable<Range> Intersect(Range a, Range b)
    {
        if (a.Start > b.End || a.End < b.Start)
        {
            yield break;
        }

        yield return new Range(Math.Max(a.Start, b.Start), Math.Min(a.End, b.End));
    }

    private static long ApplyMap(long value, IEnumerable<Map> maps)
    {
        var map = maps.FirstOrDefault(x => value >= x.Source.Start && value <= x.Source.End);
        if (map == null)
        {
            return value;
        }
        return map.Destination.Start + (value - map.Source.Start);
    }

    private static Almanac Parse(string input)
    {
        var sections = input.Split("\n\n");
        return new Almanac(
            sections[0].Split(" ").Skip(1).Select(long.Parse).ToList(),
            sections.Skip(1).Select(x =>
                {
                    var parsedMaps = x.Split("\n").Skip(1).Select(ParseMap).ToList();
                    parsedMaps.AddRange(
                        new List<Map>()
                        {
                            new(
                                new Range(0L, parsedMaps.Min(m => m.Source.Start - 1)),
                                new Range(0L, parsedMaps.Min(m => m.Destination.Start - 1))),
                            new(
                                new Range(parsedMaps.Max(m => m.Source.End + 1), long.MaxValue),
                                new Range(parsedMaps.Max(m => m.Destination.End + 1), long.MaxValue))
                        });
                    return parsedMaps;
                }).ToList()
            );
    }

    private static Map ParseMap(string input)
    {
        var parts = input.Split(" ");
        return new Map(
            new Range(long.Parse(parts[1]), long.Parse(parts[1]) + long.Parse(parts[2]) - 1),
            new Range(long.Parse(parts[0]), long.Parse(parts[0]) + long.Parse(parts[2]) - 1));
    }

    private record Almanac(IList<long> Seeds, IReadOnlyList<IReadOnlyList<Map>> Maps);

    private record Map(Range Source, Range Destination);

    private record Range(long Start, long End);
}
