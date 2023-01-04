namespace AdventOfCode2022;

public class Day19
{
    [TestCase("data/19 - Sample.txt", 0, TestName = "Day 19 - Part 1 - Sample")]
    [TestCase("data/19 - Puzzle Input.txt", 0, TestName = "Day 19 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/19 - Sample.txt", 0, TestName = "Day 19 - Part 2 - Sample")]
    [TestCase("data/19 - Puzzle Input.txt", 0, TestName = "Day 19 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }
}