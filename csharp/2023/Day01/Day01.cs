namespace AdventOfCode2023.Day01;

public class Day01
{
    // https://adventofcode.com/2023/day/1
    private readonly AdventSession _session = new(2023, 01, "Trebuchet?!");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 142)]
    [TestCase("Puzzle Input.txt", 55090)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var rows = input.Split("\n")
            .Select(x => x.ToCharArray()
                .Where(c => int.TryParse(c.ToString(), out _)));
        var answer = rows.Select(x => $"{x.First()}{x.Last()}").Select(int.Parse).Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
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
}
