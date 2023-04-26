namespace AdventOfCode2022.Day01;

public class Day01
{
    private readonly AdventSession _session = new(2022, 1, "Calorie Counting");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 24000)]
    [TestCase("Puzzle Input.txt", 69795)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input).Max(x => x.Calories.Sum());

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 45000)]
    [TestCase("Puzzle Input.txt", 208437)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input)
            .OrderByDescending(x => x.Calories.Sum())
            .Take(3)
            .SelectMany(x => x.Calories)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Elf> ParseInput(string input)
    {
        static Elf ParseElf(string elfString) => new(
            elfString.Split("\n")
                .Select(int.Parse)
                .ToList());

        return input
            .Split("\n" + "\n")
            .Select(ParseElf);
    }

    private record Elf(IReadOnlyCollection<int> Calories);
}
