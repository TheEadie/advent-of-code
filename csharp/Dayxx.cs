namespace AdventOfCodeYEAR.Dayxx;

public class Dayxx
{
    private readonly AdventSession _session = new(0000, 00, "");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 0)]
    [TestCase("Puzzle Input.txt", 0)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = 0;

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
