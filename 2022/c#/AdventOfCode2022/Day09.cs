namespace AdventOfCode2022;

public class Day09
{
    [TestCase("data/09 - Sample.txt", 13, TestName = "Sample")]
    [TestCase("data/09 - Puzzle Input.txt", 5735, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var start = Rope.Create(1);

        var answer = ParseInput(File.ReadAllText(inputFile))
            .Aggregate(
                new List<Rope> { start },
                (ropes, vector) =>
                    ropes.Append(Move(ropes.Last(), vector)).ToList())
            .DistinctBy(x => x.Knots.Last())
            .Count();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/09 - Sample.txt", 1, TestName = "Part 2 - Sample")]
    [TestCase("data/09 - Sample - 2.txt", 36, TestName = "Part 2 - Sample (2)")]
    [TestCase("data/09 - Puzzle Input.txt", 2478, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var start = Rope.Create(9);

        var answer = ParseInput(File.ReadAllText(inputFile))
            .Aggregate(
                new List<Rope> { start },
                (ropes, vector) =>
                    ropes.Append(Move(ropes.Last(), vector)).ToList())
            .DistinctBy(x => x.Knots.Last())
            .Count();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static Rope Move(Rope rope, Vector vector)
    {
        static bool IsAdjacent(Vector diff) =>
            Math.Abs(diff.X) <= 1 && Math.Abs(diff.Y) <= 1;

        static Coordinate Move(Coordinate current, Vector vector) =>
            new(current.X + Math.Clamp(vector.X, -1, 1),
                current.Y + Math.Clamp(vector.Y, -1, 1));

        var newHead = Move(rope.Head, vector);
        var knots = new List<Coordinate>();
        var prevKnot = newHead;

        foreach (var currentKnot in rope.Knots)
        {
            var diff = new Vector(prevKnot.X - currentKnot.X, prevKnot.Y - currentKnot.Y);
            var newKnot = IsAdjacent(diff) ? currentKnot : Move(currentKnot, diff);
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

    private record Rope(Coordinate Head, IEnumerable<Coordinate> Knots)
    {
        public static Rope Create(int length) =>
            new(new Coordinate(0, 0), Enumerable.Repeat(new Coordinate(0, 0), length));
    }

    private record Coordinate(int X, int Y);

    private record Vector(int X, int Y);
}