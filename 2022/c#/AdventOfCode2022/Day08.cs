namespace AdventOfCode2022;

public class Day08
{
    [TestCase("data/08 - Sample.txt", 21, TestName = "Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 1763, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var answer = input.Coordinates.Count(x =>
            FindTreesUp(input, x.Key).All(i => i < x.Value) ||
            FindTreesDown(input, x.Key).All(i => i < x.Value) ||
            FindTreesLeft(input, x.Key).All(i => i < x.Value) ||
            FindTreesRight(input, x.Key).All(i => i < x.Value));
                     
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/08 - Sample.txt", 8, TestName = "Part 2 -Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 671160, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var scores = input.Coordinates.Select(x =>
            FindTreesUp(input, x.Key).TakeUntil(i => i >= x.Value).Count() *
            FindTreesDown(input, x.Key).TakeUntil(i => i >= x.Value).Count() *
            FindTreesLeft(input, x.Key).TakeUntil(i => i >= x.Value).Count() *
            FindTreesRight(input, x.Key).TakeUntil(i => i >= x.Value).Count());

        var answer = scores.Max();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
    
    private static IEnumerable<int> FindTreesUp(Grid input, Coordinate coordinate)
    {
        for (var i = coordinate.Y - 1; i >= 0; i--)
        {
            yield return input.Coordinates[ coordinate with { Y = i }];
        }
    }
    
    private static IEnumerable<int> FindTreesDown(Grid input, Coordinate coordinate)
    {
        for (var i = coordinate.Y + 1; i < input.Height; i++)
        {
            yield return input.Coordinates[ coordinate with { Y = i }];
        }
    }
    
    private static IEnumerable<int> FindTreesLeft(Grid input, Coordinate coordinate)
    {
        for (var i = coordinate.X - 1; i >= 0; i--)
        {
            yield return input.Coordinates[ coordinate with { X = i }];
        }
    }
    
    private static IEnumerable<int> FindTreesRight(Grid input, Coordinate coordinate)
    {
        for (var i = coordinate.X + 1; i < input.Width; i++)
        {
            yield return input.Coordinates[ coordinate with { X = i}];
        }
    }

    private static Grid ParseInput(string input)
    {
        var lines = input.Split("\n");

        var treeField = new Dictionary<Coordinate, int>();
        var y = 0;

        foreach (var line in lines)
        {
            var x = 0;
            foreach (var tree in line.ToCharArray())
            {
                treeField.Add(new Coordinate(x, y), int.Parse(tree.ToString()));
                x++;
            }
            y++;
        }

        return new Grid(treeField, 
            treeField.Max(x => x.Key.X) + 1, 
            treeField.Max(x => x.Key.Y) + 1);
    }

    private record Coordinate(int X, int Y);

    private record Grid(IDictionary<Coordinate, int> Coordinates, int Width, int Height);
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