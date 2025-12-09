namespace AdventOfCode2025.Day06;

public class Day06
{
    // https://adventofcode.com/2025/day/6
    private readonly AdventSession _session = new(2025, 06, "Trash Compactor");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 4_277_556)]
    [TestCase("Puzzle Input.txt", 6_635_273_135_233)]
    public async Task Part1(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInput(input)
            .Select(x =>
                {
                    return x.Operator switch
                    {
                        Operator.Add => x.Data.Sum(),
                        Operator.Multiply => x.Data.Aggregate(1L, (acc, val) => acc * val),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                })
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 3_263_827)]
    [TestCase("Puzzle Input.txt", 12_542_543_681_221)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);

        var answer = ParseInputPart2(input)
            .Select(x =>
                {
                    return x.Operator switch
                    {
                        Operator.Add => x.Data.Sum(),
                        Operator.Multiply => x.Data.Aggregate(1L, (acc, val) => acc * val),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                })
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private record Problem(IEnumerable<long> Data, Operator Operator);

    private enum Operator
    {
        Add,
        Multiply
    }

    private static IEnumerable<Problem> ParseInput(string input)
    {
        var lines = input.Split("\n");
        var columns = lines.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        var height = lines.Length - 1;
        for (var w = 0; w < columns[0].Length; w++)
        {
            var data = new List<long>();
            for (var h = 0; h < height; h++)
            {
                data.Add(int.Parse(columns[h][w]));
            }

            var op = columns[^1][w] == "+" ? Operator.Add : Operator.Multiply;
            yield return new Problem(data, op);
        }
    }

    private static IEnumerable<Problem> ParseInputPart2(string input)
    {
        var lines = input.Split("\n");
        var width = lines.Max(x => x.Length) + 1;
        var height = lines.Length - 1;

        var data = new List<long>();
        var currentOperator = Operator.Add;
        var paddedLines = lines.Select(x => x.PadRight(width)).ToArray();

        for (var w = 0; w < width; w++)
        {
            var number = "";
            for (var h = 0; h < height; h++)
            {
                number += paddedLines[h][w];
            }

            if (number.Trim() == "")
            {
                yield return new Problem(data, currentOperator);

                data = [];
            }
            else
            {
                data.Add(long.Parse(number));
                currentOperator = paddedLines[^1][w] switch
                {
                    ' ' => currentOperator,
                    '+' => Operator.Add,
                    '*' => Operator.Multiply,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }
}
