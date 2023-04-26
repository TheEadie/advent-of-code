namespace AdventOfCode2015.Day03;

public class Day03
{
    private readonly AdventSession _session = new(2015, 3, "Perfectly Spherical Houses in a Vacuum");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var directions = (await ParseInput()).ToList();
        var visited = GetVisitedHouses(directions);

        var answer = visited.GroupBy(x => x).Count();
        Console.WriteLine(answer);
        answer.ShouldBe(2572);
    }

    [Test]
    public async Task Part2()
    {
        var directions = (await ParseInput()).ToList();
        var santa = directions.Where((x, i) => i % 2 == 0);
        var roboSanta = directions.Where((x, i) => i % 2 != 0);

        var visited = GetVisitedHouses(santa);
        visited.AddRange(GetVisitedHouses(roboSanta));

        var answer = visited.GroupBy(x => x).Count();
        Console.WriteLine(answer);
        answer.ShouldBe(2631);
    }

    private static List<Coordinate> GetVisitedHouses(IEnumerable<char> directions)
    {
        var visited = new List<Coordinate>();
        var current = new Coordinate(0, 0);
        foreach (var direction in directions)
        {
            switch (direction)
            {
                case '^':
                    current = new Coordinate(current.X, current.Y + 1);
                    visited.Add(current);
                    break;
                case '>':
                    current = new Coordinate(current.X + 1, current.Y);
                    visited.Add(current);
                    break;
                case '<':
                    current = new Coordinate(current.X - 1, current.Y);
                    visited.Add(current);
                    break;
                case 'v':
                    current = new Coordinate(current.X, current.Y - 1);
                    visited.Add(current);
                    break;
                default:
                    break;
            }
        }

        return visited;
    }

    private async Task<IEnumerable<char>> ParseInput()
    {
        var input = await _session.Start("Puzzle Input.txt");
        return input.ToCharArray();
    }

    private record Coordinate(int X, int Y);
}
