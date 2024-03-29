namespace AdventOfCode2019.Day03;

public class Day03
{
    private readonly AdventSession _session = new(2019, 3, "Crossed Wires");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var instructions = input.Split("\n");
        var pathOne = instructions[0].Split(',');
        var pathTwo = instructions[1].Split(',');

        var pathOneCoordinates = GetPathCoordinates(pathOne);
        var pathTwoCoordinates = GetPathCoordinates(pathTwo);

        var overlap = pathOneCoordinates.Keys.Intersect(pathTwoCoordinates.Keys);

        var answer = overlap.Select(x => Math.Abs(x.X) + Math.Abs(x.Y)).Min();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(245);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var instructions = input.Split("\n");
        var pathOne = instructions[0].Split(',');
        var pathTwo = instructions[1].Split(',');

        var pathOneCoordinates = GetPathCoordinates(pathOne);
        var pathTwoCoordinates = GetPathCoordinates(pathTwo);

        var overlap = pathOneCoordinates.Keys.Intersect(pathTwoCoordinates.Keys);

        var answer = overlap.Select(x => pathOneCoordinates[x] + pathTwoCoordinates[x]).Min();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(48262);
    }

    private IDictionary<Coordinate, int> GetPathCoordinates(IEnumerable<string> pathOne)
    {
        var pathCoordinates = new Dictionary<Coordinate, int>();
        var current = new Coordinate(0, 0);
        var currentDistance = 0;
        foreach (var line in pathOne)
        {
            (current, currentDistance) = GetLineCoordinates(line, current, currentDistance, pathCoordinates);
        }
        return pathCoordinates;
    }

    private (Coordinate, int) GetLineCoordinates(string input, Coordinate current, int currentDistance, IDictionary<Coordinate, int> map)
    {
        var length = int.Parse(input[1..]);

        switch (input[0])
        {
            case 'U':
                for (var i = 1; i < length; i++)
                {
                    var up = current with { Y = current.Y + i };
                    if (!map.ContainsKey(up))
                    {
                        map.Add(up, currentDistance + i);
                    }
                }
                return (current with { Y = current.Y + length }, currentDistance + length);
            case 'D':
                for (var i = 1; i < length; i++)
                {
                    var down = current with { Y = current.Y - i };
                    if (!map.ContainsKey(down))
                    {
                        map.Add(down, currentDistance + i);
                    }
                }
                return (current with { Y = current.Y - length }, currentDistance + length);
            case 'R':
                for (var i = 1; i < length; i++)
                {
                    var right = current with { X = current.X + i };
                    if (!map.ContainsKey(right))
                    {
                        map.Add(right, currentDistance + i);
                    }
                }
                return (current with { X = current.X + length }, currentDistance + length);
            case 'L':
                for (var i = 1; i < length; i++)
                {
                    var left = current with { X = current.X - i };
                    if (!map.ContainsKey(left))
                    {
                        map.Add(left, currentDistance + i);
                    }
                }
                return (current with { X = current.X - length }, currentDistance + length);
            default:
                throw new ArgumentException($"Unknown direction {input[0]}");
        }
    }

    private record Coordinate(int X, int Y);
}
