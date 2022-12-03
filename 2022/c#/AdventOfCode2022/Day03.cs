namespace AdventOfCode2022;

public class Day03
{
    [TestCase("data/03 - Sample.txt", 157, TestName = "Sample")]
    [TestCase("data/03 - Puzzle Input.txt", 7727, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var answer = File.ReadAllLines(inputFile)
            .Select(x => (x[..(x.Length / 2)], x[(x.Length / 2)..]))
            .Select(x => x.Item1.Intersect(x.Item2).Single())
            .Select(Score)
            .Sum();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/03 - Sample.txt", 70, TestName = "Sample")]
    [TestCase("data/03 - Puzzle Input.txt", 2609, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var answer = File.ReadAllLines(inputFile)
            .Chunk(3)
            .Select(x => x[0].Intersect(x[1].Intersect(x[2])).Single())
            .Select(Score)
            .Sum();

        Console.WriteLine(answer);
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