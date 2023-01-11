namespace AdventOfCode2022.Day06;

public class Day06
{
    [TestCase("Day06/Sample.txt", 7, TestName = "Day 06 - Part 1 - Sample")]
    [TestCase("Day06/Puzzle Input.txt", 1034, TestName = "Day 06 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var answer = File.ReadAllText(inputFile)
            .Window(4)
            .TakeWhile(x => x.Distinct().Count() != x.Count())
            .Count()
            + 4;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day06/Sample.txt", 19, TestName = "Day 06 - Part 2 - Sample")]
    [TestCase("Day06/Puzzle Input.txt", 2472, TestName = "Day 06 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var answer = File.ReadAllText(inputFile)
            .Window(14)
            .TakeWhile(x => x.Distinct().Count() != x.Count())
            .Count()
            + 14;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }
}