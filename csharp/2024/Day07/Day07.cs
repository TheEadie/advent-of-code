namespace AdventOfCode2024.Day07;

public class Day07
{
    // https://adventofcode.com/2024/day/7
    private readonly AdventSession _session = new(2024, 07, "Bridge Repair");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 3_749)]
    [TestCase("Puzzle Input.txt", 12_839_601_725_877)]
    public async Task Part1(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var equations = input.Split("\n")
            .Select(x => x.Split(": "))
            .Select(x => (Total: long.Parse(x[0]), Numbers: x[1].Split(" ").Select(int.Parse).ToList()))
            .ToList();

        List<Operator> validOperators =
        [
            Operator.Add,
            Operator.Multiply
        ];

        var result = equations.Select(x => Search(x, validOperators)).Where(x => x != null);

        var answer = result.Sum(x => x!.Target);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 11_387)]
    [TestCase("Puzzle Input.txt", 149_956_401_519_484)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var equations = input.Split("\n")
            .Select(x => x.Split(": "))
            .Select(x => (Total: long.Parse(x[0]), Numbers: x[1].Split(" ").Select(int.Parse).ToList()))
            .ToList();

        List<Operator> validOperators =
        [
            Operator.Add,
            Operator.Multiply,
            Operator.Concat
        ];
        var result = equations.Select(x => Search(x, validOperators)).Where(x => x != null);

        var answer = result.Sum(x => x!.Target);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static State? Search((long Total, List<int> Numbers) equation, List<Operator> validOperators)
    {
        var stack = new Stack<State>();
        stack.Push(new State(equation.Total, equation.Numbers, 1, equation.Numbers.First()));

        while (stack.Count > 0)
        {
            var state = stack.Pop();

            var nextNumber = state.Numbers[state.Index];
            foreach (var op in validOperators)
            {
                var newTotal = op switch
                {
                    Operator.Add => state.CurrentTotal + nextNumber,
                    Operator.Multiply => state.CurrentTotal * nextNumber,
                    Operator.Concat => long.Parse($"{state.CurrentTotal}{nextNumber}"),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (newTotal == state.Target && state.Index == state.Numbers.Count - 1)
                {
                    return state;
                }

                if (state.Index == state.Numbers.Count - 1 || state.CurrentTotal > state.Target)
                {
                    continue;
                }

                stack.Push(
                    state with
                    {
                        Index = state.Index + 1,
                        CurrentTotal = newTotal
                    });
            }
        }

        return null;
    }

    private record State(long Target, List<int> Numbers, int Index, long CurrentTotal);

    private enum Operator
    {
        Add,
        Multiply,
        Concat,
    }
}
