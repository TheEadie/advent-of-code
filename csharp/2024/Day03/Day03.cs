using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day03;

public class Day03
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

        var regex = new Regex(@"mul\((\d+),(\d+)\)");
        var matches = regex.Matches(input);

        var answer = matches.Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value)).Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample2.txt", 48)]
    [TestCase("Puzzle Input.txt", 76_911_921)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var regex = new Regex(@"(do\(\)|don't\(\)|mul\((\d+),(\d+)\))");
        var matches = regex.Matches(input);

        var on = true;
        var answer = 0;

        foreach (Match match in matches)
        {
            switch (match.Groups[0].Value)
            {
                case "do()":
                    on = true;
                    break;
                case "don't()":
                    on = false;
                    break;
                default:
                    if (on)
                    {
                        answer += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
                    }

                    break;
            }
        }

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }
}
