namespace AdventOfCode2022;

public class Day22
{
    [TestCase("data/22 - Sample.txt", 6032, TestName = "Day 22 - Part 1 - Sample")]
    [TestCase("data/22 - Puzzle Input.txt", 122_082, TestName = "Day 22 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var (map, moves) = ParseInput(File.ReadAllText(inputFile));

        var start = map
            .Where(x => x.Key.Y == 0)
            .MinBy(x => x.Key.X)
            .Key;

        var finalPosition = moves.Aggregate(
            new Position(start, Vector.Right),
            (current, move) => ProcessMove(move, current, map,
                p => GetOutOfBoundsPart1(p, map)));

        var answer = 1000 * (finalPosition.Coordinate.Y + 1) +
                     4 * (finalPosition.Coordinate.X + 1) +
                     GetScoreForDirection(finalPosition.Facing);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/22 - Sample.txt", 5031, TestName = "Day 22 - Part 2 - Sample")]
    [TestCase("data/22 - Puzzle Input.txt", 134_076, TestName = "Day 22 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var (map, moves) = ParseInput(File.ReadAllText(inputFile));

        var start = map
            .Where(x => x.Key.Y == 0)
            .MinBy(x => x.Key.X)
            .Key;

        var finalPosition = moves.Aggregate(
            new Position(start, Vector.Right),
            (current, move) => ProcessMove(move, current, map,
                p => GetOutOfBoundsPart2(p,
                    inputFile.Contains("Sample") ? _sampleCubeMappings : _puzzleInputCubeMappings)));

        var answer = 1000 * (finalPosition.Coordinate.Y + 1) +
                     4 * (finalPosition.Coordinate.X + 1) +
                     GetScoreForDirection(finalPosition.Facing);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static Position GetOutOfBoundsPart1(Position desired, IDictionary<Coordinate, bool> map)
    {
        var next = desired.Coordinate;
        next = desired.Facing.X switch
        {
            1 => map.Where(x => x.Key.Y == next.Y).MinBy(x => x.Key.X).Key,
            -1 => map.Where(x => x.Key.Y == next.Y).MaxBy(x => x.Key.X).Key,
            _ => next
        };

        next = desired.Facing.Y switch
        {
            1 => map.Where(x => x.Key.X == next.X).MinBy(x => x.Key.Y).Key,
            -1 => map.Where(x => x.Key.X == next.X).MaxBy(x => x.Key.Y).Key,
            _ => next
        };

        return desired with { Coordinate = next };
    }

    private static Position GetOutOfBoundsPart2(Position desired, List<(Line, Vector, Line, Vector)> mappings)
    {
        var mapping = mappings.First(x =>
            x.Item1.GetCoordinatesOnLine().Contains(desired.Coordinate) &&
            x.Item2 == desired.Facing);
        var distance = mapping.Item1.GetCoordinatesOnLine().TakeWhile(x => x != desired.Coordinate).Count();
        var newCoordinate = mapping.Item3.GetCoordinatesOnLine().Skip(distance).First();

        return new Position(newCoordinate, mapping.Item4);
    }

    private readonly List<(Line, Vector, Line, Vector)> _sampleCubeMappings = new()
    {
        (new Line(new Coordinate(0, 3), new Coordinate(3, 3)),
            Vector.Up,
            new Line(new Coordinate(11, 0), new Coordinate(8, 0)),
            Vector.Down),
        (new Line(new Coordinate(11, -1), new Coordinate(8, -1)),
            Vector.Up,
            new Line(new Coordinate(0, 4), new Coordinate(3, 4)),
            Vector.Down),
        (new Line(new Coordinate(4, 3), new Coordinate(7, 3)),
            Vector.Up,
            new Line(new Coordinate(8, 0), new Coordinate(8, 3)),
            Vector.Right),
        (new Line(new Coordinate(7, 0), new Coordinate(7, 3)),
            Vector.Left,
            new Line(new Coordinate(8, 0), new Coordinate(8, 3)),
            Vector.Down),
        (new Line(new Coordinate(12, 0), new Coordinate(12, 3)),
            Vector.Right,
            new Line(new Coordinate(15, 11), new Coordinate(15, 8)),
            Vector.Left),
        (new Line(new Coordinate(16, 11), new Coordinate(16, 8)),
            Vector.Right,
            new Line(new Coordinate(11, 0), new Coordinate(11, 3)),
            Vector.Left),
        (new Line(new Coordinate(12, 4), new Coordinate(12, 7)),
            Vector.Right,
            new Line(new Coordinate(15, 8), new Coordinate(12, 8)),
            Vector.Down),
        (new Line(new Coordinate(15, 7), new Coordinate(12, 7)),
            Vector.Up,
            new Line(new Coordinate(11, 4), new Coordinate(11, 7)),
            Vector.Left),
        (new Line(new Coordinate(15, 12), new Coordinate(12, 12)),
            Vector.Down,
            new Line(new Coordinate(0, 4), new Coordinate(0, 7)),
            Vector.Right),
        (new Line(new Coordinate(-1, 4), new Coordinate(-1, 7)),
            Vector.Left,
            new Line(new Coordinate(15, 11), new Coordinate(12, 11)),
            Vector.Up),
        (new Line(new Coordinate(0, 8), new Coordinate(3, 8)),
            Vector.Down,
            new Line(new Coordinate(11, 11), new Coordinate(8, 11)),
            Vector.Up),
        (new Line(new Coordinate(11, 12), new Coordinate(8, 12)),
            Vector.Down,
            new Line(new Coordinate(0, 7), new Coordinate(3, 7)),
            Vector.Up),
        (new Line(new Coordinate(4, 8), new Coordinate(7, 8)),
            Vector.Down,
            new Line(new Coordinate(8, 11), new Coordinate(8, 8)),
            Vector.Right),
    };

    private readonly List<(Line, Vector, Line, Vector)> _puzzleInputCubeMappings = new()
    {
        (new Line(new Coordinate(49, 0), new Coordinate(49, 49)),
            Vector.Left,
            new Line(new Coordinate(0, 149), new Coordinate(0, 100)),
            Vector.Right),
        (new Line(new Coordinate(-1, 149), new Coordinate(-1, 100)),
            Vector.Left,
            new Line(new Coordinate(50, 0), new Coordinate(50, 49)),
            Vector.Right),


        (new Line(new Coordinate(49, 50), new Coordinate(49, 99)),
            Vector.Left,
            new Line(new Coordinate(0, 100), new Coordinate(49, 100)),
            Vector.Down),
        (new Line(new Coordinate(0, 99), new Coordinate(49, 99)),
            Vector.Up,
            new Line(new Coordinate(50, 50), new Coordinate(50, 99)),
            Vector.Right),


        (new Line(new Coordinate(50, 150), new Coordinate(50, 199)),
            Vector.Right,
            new Line(new Coordinate(50, 149), new Coordinate(99, 149)),
            Vector.Up),
        (new Line(new Coordinate(50, 150), new Coordinate(99, 150)),
            Vector.Down,
            new Line(new Coordinate(49, 150), new Coordinate(49, 199)),
            Vector.Left),

        (new Line(new Coordinate(100, 100), new Coordinate(100, 149)),
            Vector.Right,
            new Line(new Coordinate(149, 49), new Coordinate(149, 0)),
            Vector.Left),
        (new Line(new Coordinate(150, 49), new Coordinate(150, 0)),
            Vector.Right,
            new Line(new Coordinate(99, 100), new Coordinate(99, 149)),
            Vector.Left),

        (new Line(new Coordinate(50, -1), new Coordinate(99, -1)),
            Vector.Up,
            new Line(new Coordinate(0, 150), new Coordinate(0, 199)),
            Vector.Right),
        (new Line(new Coordinate(-1, 150), new Coordinate(-1, 199)),
            Vector.Left,
            new Line(new Coordinate(50, 0), new Coordinate(99, 0)),
            Vector.Down),

        (new Line(new Coordinate(0, 200), new Coordinate(49, 200)),
            Vector.Down,
            new Line(new Coordinate(100, 0), new Coordinate(149, 0)),
            Vector.Down),
        (new Line(new Coordinate(100, -1), new Coordinate(149, -1)),
            Vector.Up,
            new Line(new Coordinate(0, 199), new Coordinate(49, 199)),
            Vector.Up),

        (new Line(new Coordinate(100, 50), new Coordinate(100, 99)),
            Vector.Right,
            new Line(new Coordinate(100, 49), new Coordinate(149, 49)),
            Vector.Up),
        (new Line(new Coordinate(100, 50), new Coordinate(149, 50)),
            Vector.Down,
            new Line(new Coordinate(99, 50), new Coordinate(99, 99)),
            Vector.Left),
    };

    private static Position ProcessMove(
        Move move,
        Position position,
        IDictionary<Coordinate, bool> map,
        Func<Position, Position> outOfBoundsTransform)
    {
        // Move
        for (var i = 0; i < move.Distance; i++)
        {
            var next = position with
            {
                Coordinate =
                new Coordinate(position.Coordinate.X + position.Facing.X,
                position.Coordinate.Y + position.Facing.Y)
            };

            if (!map.ContainsKey(next.Coordinate))
            {
                next = outOfBoundsTransform(next);
            }

            if (!map[next.Coordinate])
            {
                position = next;
            }
        }

        // Turn
        position = move.Turn switch
        {
            Turn.Left => position with { Facing = new Vector(position.Facing.Y, -position.Facing.X) },
            Turn.Right => position with { Facing = new Vector(-position.Facing.Y, position.Facing.X) },
            _ => position
        };

        return position;
    }

    private (IDictionary<Coordinate, bool>, IEnumerable<Move>) ParseInput(string input)
    {
        var map = new Dictionary<Coordinate, bool>();
        var sections = input.Split("\n\n");
        var lines = sections[0].Split("\n");
        var rows = lines
            .Select(line => line.ToCharArray()
                .Select(x => x).ToList())
            .ToList();

        var sizeY = rows.Count;
        var sizeX = rows.Select(x => x.Count).Max();

        for (var y = 0; y < sizeY; y++)
        {
            for (var x = 0; x < sizeX; x++)
            {
                if (rows[y].Count <= x)
                    continue;
                var value = rows[y][x];
                switch (value)
                {
                    case ' ':
                        break;
                    case '.':
                        map.Add(new Coordinate(x, y), false);
                        break;
                    case '#':
                        map.Add(new Coordinate(x, y), true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(input));
                }
            }
        }

        var distances = sections[1]
            .Split('L', 'R')
            .Select(int.Parse);

        var turns = sections[1]
            .Where(x => !(new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }.Contains(x)))
            .Select(x => x == 'L' ? Turn.Left : Turn.Right)
            .Append(Turn.None);

        var moves = distances.Zip(turns).Select(x => new Move(x.First, x.Second));

        return (map, moves);
    }

    private static int GetScoreForDirection(Vector facing)
    {
        var facingScore = facing.X switch
        {
            1 => 0,
            -1 => 2,
            _ => 0
        };

        facingScore = facing.Y switch
        {
            1 => 1,
            -1 => 3,
            _ => facingScore
        };
        return facingScore;
    }

    private record Position(Coordinate Coordinate, Vector Facing);

    private record Move(int Distance, Turn Turn);

    private enum Turn
    {
        Left,
        Right,
        None
    }
}