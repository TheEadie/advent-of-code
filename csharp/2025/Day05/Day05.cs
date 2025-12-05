namespace AdventOfCode2025.Day05;

public class Day05
{
    // https://adventofcode.com/2025/day/5
    private readonly AdventSession _session = new(2025, 05, "Cafeteria");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 3)]
    [TestCase("Puzzle Input.txt", 661)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (ranges, ingredients) = ParseInput(input);

        var answer = ingredients.Count(x => ranges.Any(r => x >= r.Min && x <= r.Max));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 14)]
    [TestCase("Puzzle Input.txt", 359_526_404_143_208)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var (ranges, _) = ParseInput(input);

        var answer = ranges.OrderBy(r => r.Min)
            .Aggregate(
                new List<Range>(),
                (acc, x) =>
                    {
                        var current = acc.LastOrDefault();
                        if (current != null && current.Overlaps(x))
                        {
                            acc[^1] = current.Merge(x);
                        }
                        else
                        {
                            acc.Add(x);
                        }

                        return acc;
                    })
            .Sum(r => r.Length());

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private record Range(long Min, long Max)
    {
        public bool Overlaps(Range other) => Min <= other.Max && other.Min <= Max;

        public Range Merge(Range other) => new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public long Length() => Max - Min + 1;
    };

    private static (IEnumerable<Range> Ranges, IEnumerable<long> Ingredients) ParseInput(string input)
    {
        var sections = input.Split("\n\n", StringSplitOptions.TrimEntries);
        var ranges = sections[0]
            .Split('\n', StringSplitOptions.TrimEntries)
            .Select(line =>
                {
                    var parts = line.Split("-");
                    return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
                });
        var ingredients = sections[1].Split('\n', StringSplitOptions.TrimEntries).Select(long.Parse);
        return (ranges, ingredients);
    }
}
