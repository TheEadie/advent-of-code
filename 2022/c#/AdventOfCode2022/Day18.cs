namespace AdventOfCode2022;

public class Day18
{
    [TestCase("data/18 - Sample.txt", 64, TestName = "Day 18 - Part 1 - Sample")]
    [TestCase("data/18 - Puzzle Input.txt", 4548, TestName = "Day 18 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var droplets = ParseInput(File.ReadAllText(inputFile));

        var answer = GetSurfaceArea(droplets);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/18 - Sample.txt", 58, TestName = "Day 18 - Part 2 - Sample")]
    [TestCase("data/18 - Puzzle Input.txt", 2588, TestName = "Day 18 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var droplets = ParseInput(File.ReadAllText(inputFile));

        var answer = GetSurfaceArea(droplets) - GetAreas(droplets).Skip(1).Sum(GetSurfaceArea);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static int GetSurfaceArea(IReadOnlySet<Coordinate3D> shape)
    {
        return shape.Select(x => 6 - GetNeighbours(x, shape).Count()).Sum();
    }

    private static IEnumerable<IReadOnlySet<Coordinate3D>> GetAreas(IReadOnlySet<Coordinate3D> map)
    {
        var areas = new List<HashSet<Coordinate3D>>();

        var minX = map.Min(x => x.X - 1);
        var maxX = map.Max(x => x.X + 1);
        var minY = map.Min(x => x.Y - 1);
        var maxY = map.Max(x => x.Y + 1);
        var minZ = map.Min(x => x.Z - 1);
        var maxZ = map.Max(x => x.Z + 1);

        var toTry = Enumerable.Range(minX, maxX - minX).SelectMany(x =>
                        Enumerable.Range(minY, maxY - minY).SelectMany(y =>
                            Enumerable.Range(minZ, maxZ - minZ).Select(z =>
                                new Coordinate3D(x, y, z))))
                                .Except(map)
                                .ToHashSet();

        while (toTry.Count > 0)
        {
            var next = toTry.First();

            var area = FloodFill(next, toTry).ToHashSet();
            areas.Add(area);
            toTry = toTry.Except(areas.SelectMany(x => x)).ToHashSet();
        }

        return areas;
    }

    private static IEnumerable<Coordinate3D> FloodFill(Coordinate3D input, IReadOnlySet<Coordinate3D> map)
    {
        var area = new HashSet<Coordinate3D>();
        var queue = new Queue<Coordinate3D>();
        queue.Enqueue(input);

        while (queue.Count > 0)
        {
            var next = queue.Dequeue();
            if (area.Contains(next))
                continue;

            area.Add(next);

            foreach (var neighbour in GetNeighbours(next, map))
            {
                queue.Enqueue(neighbour);
            }
        }

        return area;
    }

    private static IEnumerable<Coordinate3D> GetNeighbours(Coordinate3D input, IReadOnlySet<Coordinate3D> map)
    {
        var toCheck = new HashSet<Coordinate3D>
        {
            new(input.X - 1, input.Y, input.Z),
            new(input.X + 1, input.Y, input.Z),
            new(input.X, input.Y - 1, input.Z),
            new(input.X, input.Y + 1, input.Z),
            new(input.X, input.Y, input.Z - 1),
            new(input.X, input.Y, input.Z + 1),
        };

        return toCheck.Where(map.Contains);
    }

    private static HashSet<Coordinate3D> ParseInput(string input)
    {
        return input.Split("\n")
            .Select(x =>
            {
                var n = x.Split(",");
                return new Coordinate3D(int.Parse(n[0]), int.Parse(n[1]), int.Parse(n[2]));
            })
            .ToHashSet();
    }

    private record Coordinate3D(int X, int Y, int Z);
}