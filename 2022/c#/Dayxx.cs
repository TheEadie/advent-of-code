namespace AdventOfCode2022;

public class Dayxx
{
    [TestCase("data/xx - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/xx - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/xx - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/xx - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
}