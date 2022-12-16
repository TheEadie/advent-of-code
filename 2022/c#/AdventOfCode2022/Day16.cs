namespace AdventOfCode2022;

public class Day16
{
    [TestCase("data/16 - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/16 - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
}