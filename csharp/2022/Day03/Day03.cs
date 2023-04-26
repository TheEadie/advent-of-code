namespace AdventOfCode2022.Day03;

public class Day03
{
    private readonly AdventSession _session = new(2022, 3, "Rucksack Reorganization");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 157)]
    [TestCase("Puzzle Input.txt", 7727)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = input
            .Split("\n")
            .Select(x => (x[..(x.Length / 2)], x[(x.Length / 2)..]))
            .Select(x => x.Item1.Intersect(x.Item2).Single())
            .Select(Score)
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 70)]
    [TestCase("Puzzle Input.txt", 2609)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = input
            .Split("\n")
            .Chunk(3)
            .Select(x => x[0].Intersect(x[1].Intersect(x[2])).Single())
            .Select(Score)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static int Score(char item) => char.IsUpper(item) ? item - 'A' + 27 : item - 'a' + 1;
}
