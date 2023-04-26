namespace AdventOfCode2021.Day02;

public class Day02
{
    private readonly AdventSession _session = new(2021, 2, "Dive!");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var directions = ParseInput(input);

        var x = 0;
        var depth = 0;

        foreach (var direction in directions)
        {
            switch (direction.Way)
            {
                case "forward":
                    x += direction.Distance;
                    break;
                case "up":
                    depth -= direction.Distance;
                    break;
                case "down":
                    depth += direction.Distance;
                    break;
                default:
                    break;
            }
        }

        var answer = x * depth;
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(2117664);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var directions = ParseInput(input);

        var x = 0;
        var depth = 0;
        var aim = 0;

        foreach (var direction in directions)
        {
            switch (direction.Way)
            {
                case "forward":
                    x += direction.Distance;
                    depth += aim * direction.Distance;
                    break;
                case "up":
                    aim -= direction.Distance;
                    break;
                case "down":
                    aim += direction.Distance;
                    break;
                default:
                    break;
            }
        }

        var answer = x * depth;
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(2073416724);
    }

    private static Direction[] ParseInput(string input)
    {
        var lines = input.Split("\n");
        var depths = lines.Select(ParseLine).ToArray();
        return depths;
    }

    private static Direction ParseLine(string arg)
    {
        var parts = arg.Split(" ");
        return new Direction(parts[0], int.Parse(parts[1]));
    }

    public record Direction(string Way, int Distance);
}
