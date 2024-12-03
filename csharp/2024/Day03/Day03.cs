using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day03;

public partial class Day03
{
    // https://adventofcode.com/2024/day/3
    private readonly AdventSession _session = new(2024, 03, "Mull It Over");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 161)]
    [TestCase("Puzzle Input.txt", 163_931_492)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var ops = ParseMul(input);
        var answer = Run(ops);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample2.txt", 48)]
    [TestCase("Puzzle Input.txt", 76_911_921)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var ops = ParseAll(input);
        var answer = Run(ops);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Operation> ParseMul(string input) =>
        MulOperations().Matches(input).Select(x => new Mul(int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value)));

    private static IEnumerable<Operation> ParseAll(string input) =>
        AllOperations()
            .Matches(input)
            .Select<Match, Operation>(
                x => x.Groups[0].Value switch
                {
                    "do()" => new Do(),
                    "don't()" => new Dont(),
                    _ => new Mul(int.Parse(x.Groups[1].Value), int.Parse(x.Groups[2].Value))
                });

    private static int Run(IEnumerable<Operation> operations)
    {
        var on = true;
        var answer = 0;

        foreach (var operation in operations)
        {
            switch (operation)
            {
                case Do:
                    on = true;
                    break;
                case Dont:
                    on = false;
                    break;
                case Mul mul when on:
                    answer += mul.A * mul.B;
                    break;
            }
        }

        return answer;
    }

    private record Operation;

    private record Do : Operation;

    private record Dont : Operation;

    private record Mul(int A, int B) : Operation;

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulOperations();

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d+),(\d+)\)")]
    private static partial Regex AllOperations();
}
