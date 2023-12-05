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

        var answer = almanac.Seeds.Select(x => ApplyMap(x, almanac.Maps[0]))
            .Select(x => ApplyMap(x, almanac.Maps[1]))
            .Select(x => ApplyMap(x, almanac.Maps[2]))
            .Select(x => ApplyMap(x, almanac.Maps[3]))
            .Select(x => ApplyMap(x, almanac.Maps[4]))
            .Select(x => ApplyMap(x, almanac.Maps[5]))
            .Select(x => ApplyMap(x, almanac.Maps[6]))
            .MinBy(x => x);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 46)]
    [TestCase("Puzzle Input.txt", 108_956_227)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var almanac = Parse(input);

        var currentRanges = almanac.Seeds.Chunk(2).Select(x => (Start: x[0], End: x[0] + x[1] - 1));

        var inputRanges = almanac.Maps.Select(
                x => x.Select(m => (m.SourceStart, m.SourceStart + m.Length - 1))
                    .Concat(
                        new List<(long, long)>
                        {
                            (0L, x.Min(m => m.SourceStart)),
                            (x.Max(m => m.SourceStart + m.Length - 1), long.MaxValue)
                        }))
            .ToList();

        for (var i = 0; i < inputRanges.Count; i++)
        {
            currentRanges = Intersect(currentRanges, inputRanges[i])
                .Select(x => (ApplyMap(x.Item1, almanac.Maps[i]), ApplyMap(x.Item2, almanac.Maps[i])))
                .ToList();
        }

        var answer = currentRanges.Min(x => x.Start);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<(long, long)> Intersect(IEnumerable<(long, long)> a, IEnumerable<(long, long)> b) =>
        a.SelectMany(x => b.SelectMany(y => Intersect(x, y)));

    private static IEnumerable<(long, long)> Intersect((long, long) a, (long, long) b)
    {
        if (a.Item1 > b.Item2 || a.Item2 < b.Item1)
        {
            yield break;
        }

        yield return (Math.Max(a.Item1, b.Item1), Math.Min(a.Item2, b.Item2));
    }

    private static long ApplyMap(long value, IEnumerable<Map> maps)
    {
        var map = maps.FirstOrDefault(x => value >= x.SourceStart && value < x.SourceStart + x.Length);
        if (map is null)
        {
            return value;
        }

        return map.DestinationStart + (value - map.SourceStart);
    }

    private static Almanac Parse(string input)
    {
        var sections = input.Split("\n\n");
        return new Almanac(
            sections[0].Split(" ").Skip(1).Select(long.Parse).ToList(),
            sections.Skip(1).Select(x => x.Split("\n").Skip(1).Select(ParseMap)).ToList());
    }

    private static Map ParseMap(string input)
    {
        var parts = input.Split(" ");
        return new Map(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
    }

    private record Almanac(IList<long> Seeds, IReadOnlyList<IEnumerable<Map>> Maps);

    private record Map(long DestinationStart, long SourceStart, long Length);
}
