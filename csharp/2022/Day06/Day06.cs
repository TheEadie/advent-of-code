namespace AdventOfCode2022.Day06;

public class Day06
{
    private readonly AdventSession _session = new(2022, 6, "Tuning Trouble");

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", 7)]
    [TestCase("Puzzle Input.txt", 1034)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        
        var answer = input
            .Window(4)
            .Select(x => x.ToList())
            .TakeWhile(x => x.Distinct().Count() != x.Count)
            .Count()
            + 4;

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 19)]
    [TestCase("Puzzle Input.txt", 2472)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        
        var answer = input
            .Window(14)
            .Select(x => x.ToList())
            .TakeWhile(x => x.Distinct().Count() != x.Count)
            .Count()
            + 14;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }
}