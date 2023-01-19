namespace AdventOfCode2022.Day04;

public class Day04
{
    private readonly AdventSession _session = new(2022, 4, "Camp Cleanup");

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", 2)]
    [TestCase("Puzzle Input.txt", 584)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var answer = input
            .Split("\n")
            .Select(x => x.Split(","))
            .Select(x => (GetRange(x[0]), GetRange(x[1])))
            .Count(x => x.Item1.Intersect(x.Item2).Count() == x.Item1.Count() ||
                        x.Item1.Intersect(x.Item2).Count() == x.Item2.Count());

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 4)]
    [TestCase("Puzzle Input.txt", 933)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = input
            .Split("\n")
            .Select(x => x.Split(","))
            .Select(x => (GetRange(x[0]), GetRange(x[1])))
            .Count(x => x.Item1.Intersect(x.Item2).Any());

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private IEnumerable<int> GetRange(string input)
    {
        var stringParts = input.Split("-");

        var start = int.Parse(stringParts[0]);
        var end = int.Parse(stringParts[1]);

        return Enumerable.Range(start, end - start + 1);
    }
}