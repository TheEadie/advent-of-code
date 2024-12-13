using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day13;

public partial class Day13
{
    // https://adventofcode.com/2024/day/13
    private readonly AdventSession _session = new(2024, 13, "Claw Contraption");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 480)]
    [TestCase("Puzzle Input.txt", 36_571)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var machines = Parse(input);

        var answer = machines.Select(Solve).Select(x => x.X * 3 + x.Y).Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }


    [TestCase("Sample.txt", 875_318_608_908)]
    [TestCase("Puzzle Input.txt", 85_527_711_500_010)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var machines = Parse(input);
        const long add = 10000000000000;

        var answer = machines.Select(
                x => x with
                {
                    GoalX = x.GoalX + add,
                    GoalY = x.GoalY + add
                })
            .Select(Solve)
            .Select(x => x.X * 3 + x.Y)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (long X, long Y) Solve(Machine machine)
    {
        var bPresses = (double) (machine.ButtonA.X * machine.GoalY - machine.ButtonA.Y * machine.GoalX)
            / (machine.ButtonA.X * machine.ButtonB.Y - machine.ButtonA.Y * machine.ButtonB.X);
        var aPresses = (machine.GoalX - bPresses * machine.ButtonB.X) / machine.ButtonA.X;

        if (Math.Abs(aPresses % 1) > 0.0001 || Math.Abs(bPresses % 1) > 0.0001)
        {
            return (0, 0);
        }

        return ((long) aPresses, (long) bPresses);
    }

    private static IEnumerable<Machine> Parse(string input) =>
        input.Split("\n\n")
            .Select(
                x =>
                    {
                        var buttonA = ButtonARegex().Match(x);
                        var buttonB = ButtonBRegex().Match(x);
                        var goal = GoalRegex().Match(x);
                        return new Machine(
                            new Vector(int.Parse(buttonA.Groups["x"].Value), int.Parse(buttonA.Groups["y"].Value)),
                            new Vector(int.Parse(buttonB.Groups["x"].Value), int.Parse(buttonB.Groups["y"].Value)),
                            long.Parse(goal.Groups["x"].Value),
                            long.Parse(goal.Groups["y"].Value));
                    });

    private record Machine(Vector ButtonA, Vector ButtonB, long GoalX, long GoalY);

    [GeneratedRegex(@"Button A: X\+(?<x>\d+), Y\+(?<y>\d+)")]
    private static partial Regex ButtonARegex();

    [GeneratedRegex(@"Button B: X\+(?<x>\d+), Y\+(?<y>\d+)")]
    private static partial Regex ButtonBRegex();

    [GeneratedRegex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)")]
    private static partial Regex GoalRegex();
}
