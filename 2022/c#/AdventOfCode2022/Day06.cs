namespace AdventOfCode2022;

public class Day06
{
    [TestCase("data/06 - Sample.txt", 7, TestName = "Sample")]
    [TestCase("data/06 - Puzzle Input.txt", 1034, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var answer = File.ReadAllText(inputFile)
            .Window(4)
            .TakeWhile(x => x.Distinct().Count() != x.Count())
            .Count()
            + 4;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/06 - Sample.txt", 19, TestName = "Sample")]
    [TestCase("data/06 - Puzzle Input.txt", 2472, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var answer = File.ReadAllText(inputFile)
            .Window(14)
            .TakeWhile(x => x.Distinct().Count() != x.Count())
            .Count()
            + 14;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
}

public static class LinqExtensions
{
    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int size)
    {
        for (var i = 0; i < source.Count(); i++)
        {
            yield return source.Skip(i).Take(size);
        }
    }
}