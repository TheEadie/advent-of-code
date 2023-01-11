namespace AdventOfCode2022.Dayxx;

public class Dayxx
{
    [TestCase("Dayxx/Sample.txt", 0, TestName = "Day xx - Part 1 - Sample")]
    [TestCase("Dayxx/Puzzle Input.txt", 0, TestName = "Day xx - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Dayxx/Sample.txt", 0, TestName = "Day xx - Part 2 - Sample")]
    [TestCase("Dayxx/Puzzle Input.txt", 0, TestName = "Day xx - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }
}