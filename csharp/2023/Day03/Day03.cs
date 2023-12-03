using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day03;

public class Day03
{
    // https://adventofcode.com/2023/day/3
    private readonly AdventSession _session = new(2023, 03, "Gear Ratios");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 4_361)]
    [TestCase("Puzzle Input.txt", 540_131)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (numbers, symbols) = ParseInput(input);

        var answer = symbols
            .Select(x => Vector.EightDirections().Select(v => x.Position + v).ToHashSet())
            .Select(x => numbers.Where(n => n.Positions.Intersect(x).Any()))
            .SelectMany(x => x)
            .Sum(x => x.Value);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 467_835)]
    [TestCase("Puzzle Input.txt", 86_879_020)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var (numbers, symbols) = ParseInput(input);

        var answer = symbols
            .Where(x => x.Value == "*")
            .Select(x => Vector.EightDirections().Select(v => x.Position + v).ToHashSet())
            .Select(x => numbers.Where(n => n.Positions.Intersect(x).Any()).ToList())
            .Where(x => x.Count == 2)
            .Select(x => x[0].Value * x[1].Value)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (IEnumerable<Number>, IEnumerable<Symbol>) ParseInput(string input)
    {
        var lines = input.Split("\n");

        var numbers = lines.SelectMany(
            (x, i) => Regex.Matches(x, @"\d+").Select(
                m => new Number(
                    int.Parse(m.Value),
                    Enumerable.Range(m.Index, m.Length)
                        .Select(j => new Coordinate(j, i))
                        .ToList())));

        var symbols = lines.SelectMany(
            (x, i) => Regex.Matches(x, @"[^.0-9]").Select(
                m => new Symbol(m.Value, new Coordinate(m.Index, i))));
        return (numbers, symbols);
    }

    private record Number(int Value, IReadOnlyList<Coordinate> Positions);

    private record Symbol(string Value, Coordinate Position);
}
