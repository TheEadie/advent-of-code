namespace AdventOfCode2024.Day05;

public class Day05
{
    // https://adventofcode.com/2024/day/5
    private readonly AdventSession _session = new(2024, 05, "Print Queue");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 143)]
    [TestCase("Puzzle Input.txt", 5_588)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (rules, updates) = Parse(input);

        var answer = updates.Where(x => ValidUpdate(x, rules)).Select(GetMiddleNumber).Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 123)]
    [TestCase("Puzzle Input.txt", 5_331)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (rules, updates) = Parse(input);

        var answer = updates.Where(x => !ValidUpdate(x, rules))
            .Select(x => x.OrderBy(o => o, new RuleComparison(rules)).ToList())
            .Select(GetMiddleNumber)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static bool ValidUpdate(List<int> update, HashSet<Rule> rules) =>
        update.OrderBy(x => x, new RuleComparison(rules)).SequenceEqual(update);

    private class RuleComparison(HashSet<Rule> rules) : IComparer<int>
    {
        public int Compare(int x, int y) =>
            rules.Contains(new Rule(x, y)) ? -1 : rules.Contains(new Rule(y, x)) ? 1 : 0;
    }

    private static int GetMiddleNumber(IReadOnlyList<int> update) => update[update.Count / 2];

    private static (HashSet<Rule> rules, IReadOnlyCollection<List<int>> updates) Parse(string input)
    {
        var sections = input.Split("\n\n");
        var rules = sections[0]
            .Split("\n")
            .Select(x => x.Split("|"))
            .Select(x => new Rule(int.Parse(x[0]), int.Parse(x[1])))
            .ToHashSet();
        var updates = sections[1]
            .Split("\n")
            .Select(x => x.Split(",").Select(int.Parse).ToList())
            .ToList()
            .AsReadOnly();
        return (rules, updates);
    }

    private record Rule(int A, int B);
}
