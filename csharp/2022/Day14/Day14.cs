namespace AdventOfCode2022.Day14;

public class Day14
{
    private readonly AdventSession _session = new(2022, 14, "Regolith Reservoir");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 24)]
    [TestCase("Puzzle Input.txt", 858)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var cave = ParseInput(input);
        var start = new Coordinate(500, 0);
        var bottomOfCave = cave.Keys.Max(x => x.Y);

        var answer = DropSand(start, cave)
            .TakeWhile(x => x.Y < bottomOfCave)
            .Count();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 93)]
    [TestCase("Puzzle Input.txt", 26845)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var cave = ParseInput(input);
        var start = new Coordinate(500, 0);

        var answer = DropSand(start, cave)
            .TakeUntil(x => x == start)
            .Count();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Coordinate> DropSand(Coordinate start, IDictionary<Coordinate, Item> map)
    {
        var current = start;
        var maxDepth = map.Keys.Max(x => x.Y) + 1;

        while (true)
        {
            if (current.Y >= maxDepth)
            {
                yield return current;
                current = start;
            }

            var down = current with { Y = current.Y + 1 };
            var downLeft = new Coordinate(current.X - 1, current.Y + 1);
            var downRight = new Coordinate(current.X + 1, current.Y + 1);
            if (!map.ContainsKey(down))
            {
                _ = map.Remove(current);
                map.Add(down, Item.Sand);
                current = down;
            }
            else if (!map.ContainsKey(downLeft))
            {
                _ = map.Remove(current);
                map.Add(downLeft, Item.Sand);
                current = downLeft;
            }
            else if (!map.ContainsKey(downRight))
            {
                _ = map.Remove(current);
                map.Add(downRight, Item.Sand);
                current = downRight;
            }
            else
            {
                yield return current;
                current = start;
            }
        }
    }

    private static IDictionary<Coordinate, Item> ParseInput(string input)
    {
        return input.Split("\n")
            .SelectMany(x =>
            {
                var coordinates = x.Split(" -> ")
                    .Select(s =>
                    {
                        var numbers = s.Split(",").Select(int.Parse).ToArray();
                        return new Coordinate(numbers[0], numbers[1]);
                    }).ToList();

                return coordinates
                    .Zip(coordinates.Skip(1))
                    .Select(c => new Line(c.First, c.Second));
            })
            .SelectMany(x => x.GetCoordinatesOnLine())
            .Distinct()
            .ToDictionary(x => x, _ => Item.Rock);
    }

    private enum Item
    {
        Rock,
        Sand
    }
}
