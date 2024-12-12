namespace AdventOfCode2024.Day12;

public class Day12
{
    // https://adventofcode.com/2024/day/12
    private readonly AdventSession _session = new(2024, 12, "Garden Groups");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 1_930)]
    [TestCase("Puzzle Input.txt", 1_546_338)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = Parse(input);

        var answer = GetCropAreas(map).Sum(x => x.Area * x.Perimeter);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 1_206)]
    [TestCase("Puzzle Input.txt", 978_590)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var map = Parse(input);

        var answer = GetCropAreas(map).Sum(x => x.Area * x.Sides);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static List<CropArea> GetCropAreas(Dictionary<Coordinate, char> map) =>
        Fill.GetAllSections(
                map.Keys,
                x => Vector.FourDirections().Select(v => x + v),
                (a, b) => map.GetValueOrDefault(a) == map.GetValueOrDefault(b))
            .Select(a => new CropArea(a.Count, GetFences(a).Count, GetFenceSections(a).Count))
            .ToList();

    private static HashSet<Fence> GetFences(HashSet<Coordinate> cropArea) =>
        cropArea.SelectMany(
                x => Vector.FourDirections()
                    .Select(v => (Dir: v, Pos: x + v))
                    .Where(n => !cropArea.Contains(n.Pos))
                    .Select(n => n.Dir)
                    .Select(v => new Fence(x, v)))
            .ToHashSet();

    private static List<HashSet<Fence>> GetFenceSections(HashSet<Coordinate> cropArea)
    {
        var fences = GetFences(cropArea);
        return Fill.GetAllSections(
                fences,
                x => Vector.FourDirections().Select(v => x with { Position = x.Position + v }),
                (p, n) => fences.Contains(new Fence(n.Position, p.Direction)))
            .ToList();
    }

    private static Dictionary<Coordinate, char> Parse(string input)
    {
        var map = new Dictionary<Coordinate, char>();

        var lines = input.Split("\n");
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                map.Add(new Coordinate(x, y), c);
            }
        }

        return map;
    }

    private record CropArea(int Area, int Perimeter, int Sides);

    private record Fence(Coordinate Position, Vector Direction);
}
