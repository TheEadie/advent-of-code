namespace AdventOfCode2022.Day23;

public class Day23
{
    private readonly AdventSession _session = new(2022, 23, "Unstable Diffusion");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 110)]
    [TestCase("Puzzle Input.txt", 4247)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = ParseInput(input).ToHashSet();

        var finalMap = RunStep(map).Take(10).Last().ToList();

        var start = new Coordinate(finalMap.Min(x => x.X), finalMap.Min(x => x.Y));
        var end = new Coordinate(finalMap.Max(x => x.X), finalMap.Max(x => x.Y));

        var area = (end.X - start.X + 1) * (end.Y - start.Y + 1);

        var answer = area - finalMap.Count;

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 20)]
    [TestCase("Puzzle Input.txt", 1049)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = ParseInput(input).ToHashSet();

        var answer = RunStep(map).Count();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<IEnumerable<Coordinate>> RunStep(HashSet<Coordinate> map)
    {
        var vectors = new List<Vector>
        {
            Vector.Up,
            Vector.Down,
            Vector.Left,
            Vector.Right,
        };

        var vectorPointer = 0;
        var previousMap = new HashSet<Coordinate>();
        var nextMap = map;

        while (!nextMap.All(previousMap.Contains))
        {
            previousMap = nextMap;

            var proposedMoves = new Dictionary<Coordinate, Coordinate>();

            foreach (var elf in previousMap)
            {
                var noNeighbours = !GetNeighbours(elf).Any(previousMap.Contains);

                if (noNeighbours)
                {
                    proposedMoves.Add(elf, elf);
                    continue;
                }

                for (var i = 0; i < vectors.Count; i++)
                {
                    var vector = vectors[(vectorPointer + i) % vectors.Count];
                    if (!GetNeighboursInDirection(elf, vector).Any(previousMap.Contains))
                    {
                        proposedMoves.Add(elf,
                            new Coordinate(elf.X + vector.X,
                                    elf.Y + vector.Y));
                        break;
                    }
                }
            }

            var clash = proposedMoves.GroupBy(p => p.Value)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Select(x => x.Key))
                .ToList();

            foreach (var elf in clash)
            {
                proposedMoves[elf] = elf;
            }

            nextMap = previousMap.Select(elf => proposedMoves.ContainsKey(elf) ? proposedMoves[elf] : elf).ToHashSet();

            //PrintMap(nextMap.ToList());

            yield return nextMap;

            vectorPointer++;
            vectorPointer %= 4;
        }
    }

    private static IEnumerable<Coordinate> GetNeighbours(Coordinate coordinate)
    {
        return new[]
        {
            new Coordinate(coordinate.X -1, coordinate.Y -1),
            new Coordinate(coordinate.X -1, coordinate.Y),
            new Coordinate(coordinate.X -1, coordinate.Y +1),
            new Coordinate(coordinate.X, coordinate.Y -1),
            new Coordinate(coordinate.X, coordinate.Y +1),
            new Coordinate(coordinate.X +1, coordinate.Y -1),
            new Coordinate(coordinate.X +1, coordinate.Y),
            new Coordinate(coordinate.X +1, coordinate.Y +1),
        };
    }

    private static IEnumerable<Coordinate> GetNeighboursInDirection(Coordinate coordinate, Vector vector)
    {
        return vector.X != 0
            ? (new[]
            {
                new Coordinate(coordinate.X + vector.X, coordinate.Y - 1),
                new Coordinate(coordinate.X + vector.X, coordinate.Y),
                new Coordinate(coordinate.X + vector.X, coordinate.Y + 1),
            })
            : (IEnumerable<Coordinate>) (new[]
        {
            new Coordinate(coordinate.X - 1, coordinate.Y + vector.Y),
            new Coordinate(coordinate.X, coordinate.Y + vector.Y),
            new Coordinate(coordinate.X + 1, coordinate.Y + vector.Y),
        });
    }

    private static void PrintMap(ICollection<Coordinate> nextMap)
    {
        Console.WriteLine();
        Console.WriteLine();

        var minY = nextMap.Min(i => i.Y);
        var maxY = nextMap.Max(i => i.Y);

        var minX = nextMap.Min(i => i.X);
        var maxX = nextMap.Max(i => i.X);


        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Console.Write(nextMap.Contains(new Coordinate(x, y)) ? "#" : ".");
            }
            Console.WriteLine();
        }
    }

    private static IEnumerable<Coordinate> ParseInput(string input)
    {
        var map = new HashSet<Coordinate>();
        var lines = input.Split("\n");
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
                {
                    continue;
                }

                var value = rows[y][x];
                if (value == '#')
                {
                    _ = map.Add(new Coordinate(x, y));
                }
            }
        }

        return map;
    }
}
