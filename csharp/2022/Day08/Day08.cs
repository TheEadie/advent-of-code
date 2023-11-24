namespace AdventOfCode2022.Day08;

public class Day08
{
    private readonly AdventSession _session = new(2022, 8, "Treetop Tree House");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 21)]
    [TestCase("Puzzle Input.txt", 1763)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = ParseInput(input);

        var answer = map.Count(x =>
            MoveAlongVector(map, x.Key, Vector.Up).All(i => i < x.Value) ||
            MoveAlongVector(map, x.Key, Vector.Down).All(i => i < x.Value) ||
            MoveAlongVector(map, x.Key, Vector.Left).All(i => i < x.Value) ||
            MoveAlongVector(map, x.Key, Vector.Right).All(i => i < x.Value));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 8)]
    [TestCase("Puzzle Input.txt", 671160)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = ParseInput(input);

        var answer = map.Max(x =>
            MoveAlongVector(map, x.Key, Vector.Up).TakeUntil(i => i >= x.Value).Count() *
            MoveAlongVector(map, x.Key, Vector.Down).TakeUntil(i => i >= x.Value).Count() *
            MoveAlongVector(map, x.Key, Vector.Left).TakeUntil(i => i >= x.Value).Count() *
            MoveAlongVector(map, x.Key, Vector.Right).TakeUntil(i => i >= x.Value).Count());

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<int> MoveAlongVector(IDictionary<Coordinate, int> input, Coordinate coordinate, Vector step)
    {
        var next = coordinate + step;

        while (input.ContainsKey(next))
        {
            yield return input[next];
            next += step;
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
