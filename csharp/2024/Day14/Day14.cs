using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day14;

public partial class Day14
{
    // https://adventofcode.com/2024/day/14
    private readonly AdventSession _session = new(2024, 14, "Restroom Redoubt");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 12, 11, 7)]
    [TestCase("Puzzle Input.txt", 220_971_520, 101, 103)]
    public async Task Part1(string inputFile, int expected, int sizeX, int sizeY)
    {
        var input = await _session.Start(inputFile);
        var robots = Parse(input);
        var size = new Coordinate(sizeX, sizeY);

        var map = Run(robots, size).Take(100).Last();
        var answer = GetSafetyFactor(map, size);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 6_355, 101, 103)]
    public async Task Part2(string inputFile, int expected, int sizeX, int sizeY)
    {
        var input = await _session.Start(inputFile);
        var robots = Parse(input);
        var size = new Coordinate(sizeX, sizeY);

        var map = Run(robots, size).TakeUntil(r => r.Count == r.Distinct().Count());
        //PrintMap(map.Last(), size);
        var answer = map.Count();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static void PrintMap(List<Coordinate> last, Coordinate size)
    {
        for (var y = 0; y < size.Y; y++)
        {
            for (var x = 0; x < size.X; x++)
            {
                Console.Write(last.Contains(new Coordinate(x, y)) ? '#' : '.');
            }

            Console.WriteLine();
        }
    }

    private static IEnumerable<List<Coordinate>> Run(IEnumerable<Robot> robots, Coordinate size)
    {
        var localRobots = robots;

        while (true)
        {
            localRobots = localRobots.Select(
                    r =>
                        {
                            var newX = (r.Current.X + r.Movement.X) % size.X;
                            if (newX < 0)
                            {
                                newX = size.X + newX;
                            }

                            var newY = (r.Current.Y + r.Movement.Y) % size.Y;
                            if (newY < 0)
                            {
                                newY = size.Y + newY;
                            }

                            return r with { Current = new Coordinate(newX, newY) };
                        })
                .ToList();

            yield return localRobots.Select(x => x.Current).ToList();
        }
    }

    private static int GetSafetyFactor(List<Coordinate> map, Coordinate size)
    {
        var xLine = (size.X - 1) / 2;
        var yLine = (size.Y - 1) / 2;
        var topLeft = map.Count(r => r.X < xLine && r.Y < yLine);
        var topRight = map.Count(r => r.X > xLine && r.Y < yLine);
        var bottomLeft = map.Count(r => r.X < xLine && r.Y > yLine);
        var bottomRight = map.Count(r => r.X > xLine && r.Y > yLine);
        return topLeft * topRight * bottomLeft * bottomRight;
    }

    private static List<Robot> Parse(string input) =>
        input.Split("\n")
            .Select(x => RobotRegex().Match(x).Groups)
            .Select(
                x => new Robot(
                    new Coordinate(int.Parse(x["px"].Value), int.Parse(x["py"].Value)),
                    new Vector(int.Parse(x["vx"].Value), int.Parse(x["vy"].Value))))
            .ToList();

    internal record Robot(Coordinate Current, Vector Movement);

    [GeneratedRegex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)")]
    private static partial Regex RobotRegex();
}
