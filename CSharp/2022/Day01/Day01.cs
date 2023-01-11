namespace AdventOfCode2022.Day01;

public class Day01
{
    [TestCase("Day01/Sample.txt", 24000, TestName = "Day 01 - Part 1 - Sample")]
    [TestCase("Day01/Puzzle Input.txt", 69795, TestName = "Day 01 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var elves = ParseInput(File.ReadAllText(inputFile));

        var answer = elves.Max(x => x.Calories.Sum());

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day01/Sample.txt", 45000, TestName = "Day 01 - Part 2 - Sample")]
    [TestCase("Day01/Puzzle Input.txt", 208437, TestName = "Day 01 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var elves = ParseInput(File.ReadAllText(inputFile));

        var answer = elves.OrderByDescending(x => x.Calories.Sum())
            .Take(3)
            .SelectMany(x => x.Calories)
            .Sum();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
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