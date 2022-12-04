namespace AdventOfCode2022;

public class Day04
{
    [TestCase("data/04 - Sample.txt", 2, TestName = "Sample")]
    [TestCase("data/04 - Puzzle Input.txt", 584, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var answer = File.ReadAllLines(inputFile)
            .Select(x => x.Split(","))
            .Select(x => (GetRange(x[0]), GetRange(x[1])))
            .Count(x => x.Item1.Intersect(x.Item2).Count() == x.Item1.Count() ||
                        x.Item1.Intersect(x.Item2).Count() == x.Item2.Count());

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/04 - Sample.txt", 4, TestName = "Sample")]
    [TestCase("data/04 - Puzzle Input.txt", 933, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var answer = File.ReadAllLines(inputFile)
            .Select(x => x.Split(","))
            .Select(x => (GetRange(x[0]), GetRange(x[1])))
            .Count(x => x.Item1.Intersect(x.Item2).Any());

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private IEnumerable<int> GetRange(string input)
    {
        var stringParts = input.Split("-");

        var start = int.Parse(stringParts[0]);
        var end = int.Parse(stringParts[1]);

        return Enumerable.Range(start, end - start + 1);
    }
}