namespace AdventOfCode2023.Day01;

public class Day01
{
    // https://adventofcode.com/2023/day/1
    private readonly AdventSession _session = new(2023, 01, "Trebuchet?!");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 142)]
    [TestCase("Puzzle Input.txt", 55_090)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = input.Split("\n")
            .Select(x => x.ToCharArray()
                .Where(c => int.TryParse(c.ToString(), out _))
                .ToList())
            .Select(x => $"{x.First()}{x.Last()}")
            .Select(int.Parse)
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample-2.txt", 281)]
    [TestCase("Puzzle Input.txt", 54_845)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var answer = input.Split("\n")
            .Select(input1 => input1.Select((x, i) => input1[i..])
                .Select(
                    x => (char?) (x switch
                    {
                        ['1', ..] or ['o', 'n', 'e', ..] => '1',
                        ['2', ..] or ['t', 'w', 'o', ..] => '2',
                        ['3', ..] or ['t', 'h', 'r', 'e', 'e', ..] => '3',
                        ['4', ..] or ['f', 'o', 'u', 'r', ..] => '4',
                        ['5', ..] or ['f', 'i', 'v', 'e', ..] => '5',
                        ['6', ..] or ['s', 'i', 'x', ..] => '6',
                        ['7', ..] or ['s', 'e', 'v', 'e', 'n', ..] => '7',
                        ['8', ..] or ['e', 'i', 'g', 'h', 't', ..] => '8',
                        ['9', ..] or ['n', 'i', 'n', 'e', ..] => '9',
                        _ => null
                    }))
                .Where(x => x is not null)
                .Select(number => number!.Value)
                .ToList())
            .Select(x => $"{x.First()}{x.Last()}")
            .Select(int.Parse)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }
}
