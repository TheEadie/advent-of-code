namespace AdventOfCode2024.Day04;

public class Day04
{
    // https://adventofcode.com/2024/day/4
    private readonly AdventSession _session = new(2024, 04, "Ceres Search");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 18)]
    [TestCase("Puzzle Input.txt", 2_557)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var crossword = Parse(input);

        var answer = crossword.Keys.Sum(
            x => Vector.EightDirections().Count(vector => CheckForWord("XMAS", x, vector, crossword)));

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 9)]
    [TestCase("Puzzle Input.txt", 1_854)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var crossword = Parse(input);

        var answer = crossword.Keys.Count(
            x => Vector.DiagonalDirections().Sum(vector => CheckForWord("MAS", x, vector, crossword, 1) ? 1 : 0) >= 2);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static Dictionary<Coordinate, char> Parse(string input) =>
        input.Split("\n")
            .SelectMany((str, y) => str.ToCharArray().Select((c, x) => (Position: new Coordinate(x, y), Value: c)))
            .ToDictionary(x => x.Position, x => x.Value);

    private static bool CheckForWord(
        string word,
        Coordinate coordinate,
        Vector vector,
        Dictionary<Coordinate, char> grid,
        int offset = 0) =>
        word.Select(
                (c, i) =>
                    {
                        var position = coordinate + new Vector(vector.X * (i - offset), vector.Y * (i - offset));
                        return grid.TryGetValue(position, out var gridLetter) && gridLetter == c;
                    })
            .All(x => x);
}
