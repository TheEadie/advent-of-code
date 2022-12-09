namespace AdventOfCode2022;

public class Day09
{
    [TestCase("data/09 - Sample.txt", 13, TestName = "Sample")]
    [TestCase("data/09 - Puzzle Input.txt", 5735, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var start = new Rope(new Coordinate(0, 0), Enumerable.Repeat(new Coordinate(0, 0), 1));

        var answer = ParseInput(File.ReadAllText(inputFile))
            .Aggregate(new List<Rope> {start},
                (ropes, vector) =>
                {
                    ropes.Add(Move(ropes.Last(), vector));
                    return ropes;
                })
            .DistinctBy(x => x.Tail.Last()).Count();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/09 - Sample.txt", 1, TestName = "Part 2 - Sample")]
    [TestCase("data/09 - Sample - 2.txt", 36, TestName = "Part 2 - Sample (2)")]
    [TestCase("data/09 - Puzzle Input.txt", 2478, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var start = new Rope(new Coordinate(0, 0), Enumerable.Repeat(new Coordinate(0,0), 9));

        var answer = ParseInput(File.ReadAllText(inputFile))
            .Aggregate(new List<Rope> {start},
                (ropes, vector) =>
                {
                    ropes.Add(Move(ropes.Last(), vector));
                    return ropes;
                })
            .DistinctBy(x => x.Tail.Last()).Count();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static Rope Move(Rope rope, Vector vector)
    {
        var newHead = new Coordinate(rope.Head.X + vector.X, rope.Head.Y + vector.Y);
        var knots = new List<Coordinate>();
        var prevKnot = newHead;

        foreach (var currentTail in rope.Tail)
        {
            var diff = new Vector(prevKnot.X - currentTail.X, prevKnot.Y - currentTail.Y);

            var newKnot = Math.Abs(diff.X) > 1 || Math.Abs(diff.Y) > 1 ? 
                new Coordinate(currentTail.X + Math.Clamp(diff.X, -1, 1), 
                    currentTail.Y + Math.Clamp(diff.Y, -1, 1)) :
                currentTail;
            
            knots.Add(newKnot);

            prevKnot = newKnot;
        }

        return new Rope(newHead, knots);
    }

    private static IEnumerable<Vector> ParseInput(string input)
    {
        return input.Split("\n")
            .SelectMany(line => 
                Enumerable.Repeat(line[0], int.Parse(line[2..]))
            .Select(x => 
                x switch
                {
                    'U' => new Vector(0, -1),
                    'D' => new Vector(0, 1),
                    'L' => new Vector(-1, 0),
                    'R' => new Vector(1, 0),
                    _ => throw new ArgumentOutOfRangeException()
                }));
    }

    private record Rope(Coordinate Head, IEnumerable<Coordinate> Tail);

    private record Coordinate(int X, int Y);

    private record Vector(int X, int Y);
}