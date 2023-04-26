namespace AdventOfCode2022.Day09;

public class Day09
{
    private readonly AdventSession _session = new(2022, 9, "Rope Bridge");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 13)]
    [TestCase("Puzzle Input.txt", 5735)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input)
            .Aggregate(
                new List<Rope> { Rope.Create(1) },
                (ropes, vector) =>
                    ropes.Append(Move(ropes.Last(), vector)).ToList())
            .DistinctBy(x => x.Knots.Last())
            .Count();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 1)]
    [TestCase("Sample - 2.txt", 36)]
    [TestCase("Puzzle Input.txt", 2478)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input)
            .Aggregate(
                new List<Rope> { Rope.Create(9) },
                (ropes, vector) =>
                    ropes.Append(Move(ropes.Last(), vector)).ToList())
            .DistinctBy(x => x.Knots.Last())
            .Count();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static Rope Move(Rope rope, Vector vector)
    {
        static bool IsAdjacent(Vector diff) =>
            Math.Abs(diff.X) <= 1 && Math.Abs(diff.Y) <= 1;

        static Coordinate MoveKnot(Coordinate current, Vector vector) =>
            new(current.X + Math.Clamp(vector.X, -1, 1),
                current.Y + Math.Clamp(vector.Y, -1, 1));

        var newHead = MoveKnot(rope.Head, vector);
        var knots = new List<Coordinate>();
        var prevKnot = newHead;

        foreach (var currentKnot in rope.Knots)
        {
            var diff = new Vector(prevKnot.X - currentKnot.X, prevKnot.Y - currentKnot.Y);
            var newKnot = IsAdjacent(diff) ? currentKnot : MoveKnot(currentKnot, diff);
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
}
