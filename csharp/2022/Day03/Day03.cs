namespace AdventOfCode2022.Day03;

public class Day03
{
    [TestCase("Day03/Sample.txt", 157, TestName = "Day 03 - Part 1 - Sample")]
    [TestCase("Day03/Puzzle Input.txt", 7727, TestName = "Day 03 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var answer = File.ReadAllLines(inputFile)
            .Select(x => (x[..(x.Length / 2)], x[(x.Length / 2)..]))
            .Select(x => x.Item1.Intersect(x.Item2).Single())
            .Select(Score)
            .Sum();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day03/Sample.txt", 70, TestName = "Day 03 - Part 2 - Sample")]
    [TestCase("Day03/Puzzle Input.txt", 2609, TestName = "Day 03 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var answer = File.ReadAllLines(inputFile)
            .Chunk(3)
            .Select(x => x[0].Intersect(x[1].Intersect(x[2])).Single())
            .Select(Score)
            .Sum();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private int Score(char item)
    {
        if (char.IsUpper(item))
        {
            return item - 'A' + 27;
        }

        return item - 'a' + 1;
    }
}