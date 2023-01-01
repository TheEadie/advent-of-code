namespace AdventOfCode2022;

public class Day08
{
    [TestCase("data/08 - Sample.txt", 21, TestName = "Day 08 - Part 1 - Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 1763, TestName = "Day 08 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var answer = input.Count(x =>
            MoveAlongVector(input, x.Key, Vector.Up).All(i => i < x.Value) ||
            MoveAlongVector(input, x.Key, Vector.Down).All(i => i < x.Value) ||
            MoveAlongVector(input, x.Key, Vector.Left).All(i => i < x.Value) ||
            MoveAlongVector(input, x.Key, Vector.Right).All(i => i < x.Value));

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/08 - Sample.txt", 8, TestName = "Day 08 - Part 2 - Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 671160, TestName = "Day 08 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var answer = input.Max(x =>
            MoveAlongVector(input, x.Key, Vector.Up).TakeUntil(i => i >= x.Value).Count() *
            MoveAlongVector(input, x.Key, Vector.Down).TakeUntil(i => i >= x.Value).Count() *
            MoveAlongVector(input, x.Key, Vector.Left).TakeUntil(i => i >= x.Value).Count() *
            MoveAlongVector(input, x.Key, Vector.Right).TakeUntil(i => i >= x.Value).Count());

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static IEnumerable<int> MoveAlongVector(IDictionary<Coordinate, int> input, Coordinate coordinate, Vector step)
    {
        var next = new Coordinate(coordinate.X + step.X, coordinate.Y + step.Y);

        while (input.ContainsKey(next))
        {
            yield return input[next];
            next = new Coordinate(next.X + step.X, next.Y + step.Y);
        }
    }

    private static IDictionary<Coordinate, int> ParseInput(string input)
    {
        var lines = input.Split("\n");

        var treeField = new Dictionary<Coordinate, int>();
        var y = 0;

        foreach (var line in lines)
        {
            var x = 0;
            foreach (var tree in line)
            {
                treeField.Add(new Coordinate(x, y), int.Parse(tree.ToString()));
                x++;
            }
            y++;
        }

        return treeField;
    }
}