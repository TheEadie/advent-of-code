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

        foreach (var t in rope.Tail)
        {
            var diff = new Vector(
                Math.Clamp(prevKnot.X - t.X, -1, 1),
                Math.Clamp(prevKnot.Y - t.Y, -1, 1));

            var newKnot = new Coordinate(t.X + diff.X, t.Y + diff.Y);
            if (newKnot == prevKnot)
            {
                newKnot = t;
            }
            
            knots.Add(newKnot);
            prevKnot = newKnot;
        }

        return new Rope(newHead, knots);
    }

    private IEnumerable<Vector> ParseInput(string input)
    {
        IEnumerable<Vector> ParseLine(string line)
        {
            var parts = line.Split(" ");
            var times = int.Parse(parts[1]);

            for (var i = 0; i < times; i++)
            {
                switch (parts[0])
                {
                    case "U":
                        yield return new Vector(0, -1);
                        break;
                    case "D":
                        yield return new Vector(0, 1);
                        break;
                    case "L":
                        yield return new Vector(-1, 0);
                        break;
                    case "R":
                        yield return new Vector(1, 0);
                        break;
                }
            }
        }
        
        return input.Split("\n").SelectMany(ParseLine);
        
    }

    private record Rope(Coordinate Head, IEnumerable<Coordinate> Tail);

    private record Coordinate(int X, int Y);

    private record Vector(int X, int Y);
}