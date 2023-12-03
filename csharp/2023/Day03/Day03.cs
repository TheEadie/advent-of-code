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
            .SelectMany(x => GetAdjacentNumbers(x.Key, numbers))
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
            .Select(x => GetAdjacentNumbers(x.Key, numbers).ToList())
            .Where(x => x.Count == 2)
            .Select(x => x[0].Value * x[1].Value)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Number> GetAdjacentNumbers(
        Coordinate position,
        IDictionary<Coordinate, Number> numbers) =>
            Vector.EightDirections().Select(v => position + v)
                .Select(c => numbers.TryGetValue(c, out var n) ? n : null)
                .Where(n => n != null)
                .Distinct()!;

    private static (IDictionary<Coordinate, Number>, IDictionary<Coordinate, string>) ParseInput(string input)
    {
        var lines = input.Split("\n");

        var numbers = lines.SelectMany(
            (x, i) => Regex.Matches(x, @"\d+").Select(
                m => (
                    Value: int.Parse(m.Value),
                    Positions: Enumerable.Range(m.Index, m.Length)
                        .Select(j => new Coordinate(j, i))
                        .ToList())));
        var symbols = lines.SelectMany(
            (x, i) => Regex.Matches(x, @"[^.0-9]").Select(
                m => (m.Value, Position: new Coordinate(m.Index, i))))
            .ToDictionary(x => x.Position, x => x.Value);

        var map = new Dictionary<Coordinate, Number>();
        foreach (var number in numbers)
        {
            foreach (var position in number.Positions)
            {
                map.Add(position, new Number(number.Item1));
            }
        }

        return (map, symbols);
    }

    private record Number(int Value);
}
