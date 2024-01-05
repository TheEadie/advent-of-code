using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day08;

public class Day08
{
    // https://adventofcode.com/2023/day/8
    private readonly AdventSession _session = new(2023, 08, "Haunted Wasteland");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 2)]
    [TestCase("Sample-2.txt", 6)]
    [TestCase("Puzzle Input.txt", 20_221)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (directions, map) = Parse(input);

        var (cycleStart, movesToZzz, _) = FindCycle(map.First(x => x.Name == "AAA"), map, directions);

        var answer = cycleStart + movesToZzz;
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample-3.txt", 6)]
    [TestCase("Puzzle Input.txt", 14_616_363_770_447L)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var (directions, map) = Parse(input);

        var cycleLengths = map.Where(x => x.Name.EndsWith("A"))
            .Select(x => FindCycle(x, map, directions))
            .Select(x => x.CycleLength);

        var answer = cycleLengths.Aggregate(LowestCommonDenominator);
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (long CycleStart, long FirstZInCycle, long CycleLength) FindCycle(
        Node start,
        IList<Node> map,
        IReadOnlyList<char> directions)
    {
        var visited = new HashSet<string>();
        var route = new List<Node>();

        var node = start;
        var currentDirection = 0;

        route.Add(node);

        while (!visited.Contains($"{node.Name}{currentDirection}"))
        {
            visited.Add($"{node.Name}{currentDirection}");

            var direction = directions[currentDirection];
            currentDirection = (currentDirection + 1) % directions.Count;
            node = direction switch
            {
                'L' => map.First(x => x.Name == node.Left),
                'R' => map.First(x => x.Name == node.Right),
                _ => throw new Exception($"Unknown Direction {direction}")
            };

            route.Add(node);
        }

        var cycleStart = route.IndexOf(route.First(x => x.Name == node.Name));
        var zNode = route.IndexOf(route.First(x => x.Name.EndsWith("Z"))) - cycleStart;
        var cycleLength = route.Count - cycleStart - 1;

        return (cycleStart, zNode, cycleLength);
    }

    private static (char[] Directions, IList<Node> Map) Parse(string input)
    {
        var parts = input.Split("\n\n");
        var nodes = parts[1]
            .Split("\n")
            .Select(
                x =>
                    {
                        var matches = Regex.Match(x, @"(\w+) = \((\w+), (\w+)\)");
                        var name = matches.Groups[1].Value;
                        var left = matches.Groups[2].Value;
                        var right = matches.Groups[3].Value;
                        return new Node(name, left, right);
                    })
            .ToList();

        return (parts[0].ToCharArray(), nodes);
    }

    private record Node(string Name, string Left, string Right);

    private static long LowestCommonDenominator(long a, long b) => Math.Abs(a * b) / GreatestCommonDenominator(a, b);

    private static long GreatestCommonDenominator(long a, long b)
    {
        while (true)
        {
            if (b == 0)
            {
                return a;
            }

            var a1 = a;
            a = b;
            b = a1 % b;
        }
    }
}
