namespace AdventOfCode2022;

public class Day08
{
    [TestCase("data/08 - Sample.txt", 21, TestName = "Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 1763, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var answer = 0;

        for (var x = 0; x < input.GetLength(0); x++)
        {
            for (var y = 0; y < input.GetLength(1); y++)
            {
                if (FindTreesUp(input, x, y).All(i => i < input[x, y]) ||
                    FindTreesDown(input, x, y).All(i => i < input[x, y]) ||
                    FindTreesLeft(input, x, y).All(i => i < input[x, y]) ||
                    FindTreesRight(input, x, y).All(i => i < input[x, y]))
                {
                    answer++;
                }
            }
        }

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/08 - Sample.txt", 8, TestName = "Part 2 -Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 671160, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var scores = new List<int>();

        for (var x = 0; x < input.GetLength(0); x++)
        {
            for (var y = 0; y < input.GetLength(1); y++)
            {
                scores.Add(FindTreesUp(input, x, y).TakeUntil(i => i >= input[x, y]).Count() *
                           FindTreesDown(input, x, y).TakeUntil(i => i >= input[x, y]).Count() *
                           FindTreesLeft(input, x, y).TakeUntil(i => i >= input[x, y]).Count() *
                           FindTreesRight(input, x, y).TakeUntil(i => i >= input[x, y]).Count());
            }
        }
        
        var answer = scores.Max();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
    
    private static IEnumerable<int> FindTreesUp(int[,] input, int x, int y)
    {
        for (var i = 1; i <= y; i++)
        {
            yield return input[x, y - i];
        }
    }
    
    private static IEnumerable<int> FindTreesDown(int[,] input, int x, int y)
    {
        for (var i = 1; i < input.GetLength(1) - y; i++)
        {
            yield return input[x, y + i];
        }
    }
    
    private static IEnumerable<int> FindTreesLeft(int[,] input, int x, int y)
    {
        for (var i = 1; i <= x; i++)
        {
            yield return input[x - i, y];
        }
    }
    
    private static IEnumerable<int> FindTreesRight(int[,] input, int x, int y)
    {
        for (var i = 1; i < input.GetLength(0) - x; i++)
        {
            yield return input[x + i, y];
        }
    }

    private static int[,] ParseInput(string input)
    {
        var lines = input.Split("\n");

        var treeField = new int[lines[0].Length, lines.Length];
        var y = 0;

        foreach (var line in lines)
        {
            var x = 0;
            foreach (var tree in line.ToCharArray())
            {
                treeField[x, y] = int.Parse(tree.ToString());
                x++;
            }
            y++;
        }

        return treeField;
    }
}

public static partial class LinqExtensions
{
    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> data, Func<T, bool> predicate) {
        foreach (var item in data) {
            yield return item;
            if (predicate(item))
                break;
        }
    }
}