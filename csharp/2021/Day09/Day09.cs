namespace AdventOfCode2021.Day09;

public class Day09
{
    private readonly AdventSession _session = new(2021, 9, "Smoke Basin");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var heightMap = ParseInput(input);

        var sizeX = heightMap.GetLength(0);
        var sizeY = heightMap.GetLength(1);

        var answer = 0;

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                var minPoint = IsLowest(new Coordinate(x, y), heightMap);
                if (minPoint)
                {
                    answer += heightMap[x, y] + 1;
                }
            }
        }

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(566);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var heightMap = ParseInput(input);

        var sizeX = heightMap.GetLength(0);
        var sizeY = heightMap.GetLength(1);

        var basins = new List<int>();

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                var coordinate = new Coordinate(x, y);
                var minPoint = IsLowest(coordinate, heightMap);
                if (minPoint)
                {
                    basins.Add(GetNeighboursInBasin(coordinate, heightMap).Count());
                }
            }
        }

        var answer = basins
            .OrderByDescending(x => x)
            .Take(3)
            .Aggregate((a, x) => a * x);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(891684);

    }

    private static bool IsLowest(Coordinate input, int[,] heightMap)
    {
        return GetNeighbours(input, heightMap)
            .All(x => heightMap[input.X, input.Y] < heightMap[x.X, x.Y]);
    }

    private static IEnumerable<Coordinate> GetNeighboursInBasin(Coordinate input, int[,] heightMap)
    {
        return GetNeighbours(input, heightMap)
            .Where(testCoordinate => heightMap[testCoordinate.X, testCoordinate.Y] > heightMap[input.X, input.Y] &&
                                     heightMap[testCoordinate.X, testCoordinate.Y] != 9)
            .SelectMany(x => GetNeighboursInBasin(x, heightMap))
            .Append(input)
            .Distinct();

    }

    private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, int[,] heightMap)
    {
        var testCoordinates = new List<Coordinate>();

        if (input.X > 0)
        {
            testCoordinates.Add(new Coordinate(input.X - 1, input.Y));
        }

        if (input.X < heightMap.GetLength(0) - 1)
        {
            testCoordinates.Add(new Coordinate(input.X + 1, input.Y));
        }

        if (input.Y > 0)
        {
            testCoordinates.Add(new Coordinate(input.X, input.Y - 1));
        }

        if (input.Y < heightMap.GetLength(1) - 1)
        {
            testCoordinates.Add(new Coordinate(input.X, input.Y + 1));
        }

        return testCoordinates;
    }

    private static int[,] ParseInput(string input)
    {
        var lines = input.Split("\n");
        var rows = lines.Select(line => line.ToCharArray().Select(x => int.Parse(x.ToString())).ToList()).ToList();

        var sizeX = rows.Count;
        var sizeY = rows[0].Count();
        var heightMap = new int[sizeX, sizeY];

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                heightMap[x, y] = rows[x][y];
            }
        }

        return heightMap;
    }
}
