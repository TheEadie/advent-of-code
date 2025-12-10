namespace AdventOfCode2025.Day08;

public class Day08
{
    // https://adventofcode.com/2025/day/8
    private readonly AdventSession _session = new(2025, 08, "Playground");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 40, 10)]
    [TestCase("Puzzle Input.txt", 181_584, 1000)]
    public async Task Part1(string inputFile, int expected, int pairs)
    {
        var input = await _session.Start(inputFile);
        var coordinates = ParseInput(input).ToList();

        var answer = MergeCircuits(GetDistances(coordinates))
            .Take(pairs)
            .Last()
            .Circuits.OrderByDescending(x => x.Count)
            .Take(3)
            .Aggregate(1, (acc, set) => acc * set.Count);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 25272)]
    [TestCase("Puzzle Input.txt", 8_465_902_405)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var coordinates = ParseInput(input).ToList();

        var finalCircuits = MergeCircuits(GetDistances(coordinates))
            .TakeUntil(x => x.Circuits.Count == 1 && x.Circuits[0].Count == coordinates.Count)
            .Last();

        var answer = finalCircuits.LastA.X * finalCircuits.LastB.X;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private record Distance(Coordinate3D A, Coordinate3D B, double Value);

    private class Circuits : List<HashSet<Coordinate3D>>;

    private static List<Distance> GetDistances(List<Coordinate3D> coordinates)
    {
        var distances = new List<Distance>();
        foreach (var x in coordinates)
        {
            distances.AddRange(
                coordinates.TakeWhile(y => x != y)
                    .Select(y => new Distance(x, y, Coordinate3D.StraightDistance(x, y))));
        }

        return distances.OrderBy(x => x.Value).ToList();
    }

    private static IEnumerable<(Circuits Circuits, Coordinate3D LastA, Coordinate3D LastB)> MergeCircuits(
        List<Distance> distances)
    {
        var circuits = new Circuits();

        foreach (var (a, b, _) in distances)
        {
            circuits = MergeCircuits(circuits, a, b);
            yield return (circuits, a, b);
        }
    }

    private static Circuits MergeCircuits(Circuits sets, Coordinate3D a, Coordinate3D b)
    {
        var setWithA = sets.FirstOrDefault(x => x.Contains(a));
        var setWithB = sets.FirstOrDefault(x => x.Contains(b));
        if (setWithA != null && setWithB != null)
        {
            if (setWithA != setWithB)
            {
                setWithA.UnionWith(setWithB);
                sets.Remove(setWithB);
            }
        }
        else if (setWithA != null)
        {
            setWithA.Add(b);
        }
        else if (setWithB != null)
        {
            setWithB.Add(a);
        }
        else
        {
            sets.Add(
            [
                a,
                b
            ]);
        }

        return sets;
    }

    private static IEnumerable<Coordinate3D> ParseInput(string input) =>
        input.Split("\n")
            .Select(x =>
                {
                    var parts = x.Split(",");
                    return new Coordinate3D(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                });
}
