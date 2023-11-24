namespace AdventOfCode2015.Day01;

public class Day01
{
    private readonly AdventSession _session = new(2015, 1, "Not Quite Lisp");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");

        var answer = input
            .ToCharArray()
            .Select(x => x == '(' ? 1 : -1)
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(138);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");

        var runningTotal = 0;
        var answer = input
            .ToCharArray()
            .Select(x => x == '(' ? 1 : -1)
            .Select((x, i) =>
                {
                    runningTotal += x;
                    return (i, runningTotal);
                })
            .First(x => x.runningTotal == -1).i + 1;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(1771);
    }

    private async Task<int[]> ParseInput()
    {
        var input = await _session.Start("Puzzle Input.txt");
        return input.ToCharArray().Select(x => x == '(' ? 1 : -1).ToArray();
    }
}
