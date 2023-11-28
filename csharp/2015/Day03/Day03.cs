namespace AdventOfCode2015.Day03;

public class Day03
{
    private readonly AdventSession _session = new(2015, 3, "Perfectly Spherical Houses in a Vacuum");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");

        var directions = input.ToCharArray();
        var answer = GetVisitedHouses(directions)
            .GroupBy(x => x)
            .Count();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(2572);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var directions = input.ToCharArray();
        var santa = directions.Where((x, i) => i % 2 == 0);
        var roboSanta = directions.Where((x, i) => i % 2 != 0);

        var visited = GetVisitedHouses(santa);
        visited.AddRange(GetVisitedHouses(roboSanta));

        var answer = visited.GroupBy(x => x).Count();
        _session.PrintAnswer(2, answer);
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
                    current += Vector.Up;
                    visited.Add(current);
                    break;
                case '>':
                    current += Vector.Right;
                    visited.Add(current);
                    break;
                case '<':
                    current += Vector.Left;
                    visited.Add(current);
                    break;
                case 'v':
                    current += Vector.Down;
                    visited.Add(current);
                    break;
                default:
                    break;
            }
        }

        return visited;
    }
}
