using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day02;

public class Day02
{
    // https://adventofcode.com/2023/day/2
    private readonly AdventSession _session = new(2023, 02, "Cube Conundrum");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 8)]
    [TestCase("Puzzle Input.txt", 2_720)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var games = input.Split("\n").Select(ParseLine).ToList();

        var answer = games.Select(x => (
                x.Id,
                Red: x.Rounds.Max(r => r.Red),
                Green: x.Rounds.Max(r => r.Green),
                Blue: x.Rounds.Max(r => r.Blue)))
            .Where(x => x is { Red: <= 12, Green: <= 13, Blue: <= 14 })
            .Sum(x => x.Id);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    private Game ParseLine(string line)
    {
        var parts = line.Split(":");
        var gamePart = parts[0].Split(" ")[1];
        var id = int.Parse(gamePart);

        var rounds = new List<Round>();

        var colorParts = parts[1].Split(";");
        foreach (var part in colorParts)
        {
            var red = Regex.Match(part, @"(\d+) red").Groups[1].Value;
            var green = Regex.Match(part, @"(\d+) green").Groups[1].Value;
            var blue = Regex.Match(part, @"(\d+) blue").Groups[1].Value;

            var redNumber = red == "" ? 0 : int.Parse(red);
            var greenNumber = green == "" ? 0 : int.Parse(green);
            var blueNumber = blue == "" ? 0 : int.Parse(blue);

            rounds.Add(new Round(redNumber, greenNumber, blueNumber));
        }

        return new Game(id, rounds);
    }

    [TestCase("Sample.txt", 0)]
    [TestCase("Puzzle Input.txt", 0)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = 0;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private record Game(int Id, IEnumerable<Round> Rounds);
    private record Round(int Red, int Green, int Blue);
}
