namespace AdventOfCode2022.Day05;

public class Day05
{
    private readonly AdventSession _session = new(2022, 5, "Supply Stacks");

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", "CMZ")]
    [TestCase("Puzzle Input.txt", "VRWBSFZWM")]
    public async Task Part1(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);
        var (stacks, moves) = Parse(input);

        foreach (var move in moves)
        {
            for (var i = 0; i < move.NumberToMove; i++)
            {
                var removed = stacks[move.From - 1].Pop();
                stacks[move.To - 1].Push(removed);
            }
        }

        var answer = stacks.Aggregate("", (current, stack) => current + stack.Peek());

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", "MCD")]
    [TestCase("Puzzle Input.txt", "RBTWJWMCF")]
    public async Task Part2(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);
        var (stacks, moves) = Parse(input);

        foreach (var move in moves)
        {
            var removed = new List<char>();
            for (var i = 0; i < move.NumberToMove; i++)
            {
                removed.Add(stacks[move.From - 1].Pop());
            }
            removed.Reverse();

            foreach (var item in removed)
            {
                stacks[move.To - 1].Push(item);
            }
        }

        var answer = stacks.Aggregate("", (current, stack) => current + stack.Peek());

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (Stack<char>[], IEnumerable<Move>) Parse(string input)
    {
        var sections = input.Split("\n" + "\n");
        var stackInput = sections[0].Split("\n").Reverse().ToList();
        var noOfStacks = (stackInput.First().Length + 1) / 4;

        var stacks = new Stack<char>[noOfStacks];
        for (var i = 0; i < noOfStacks; i++)
        {
            stacks[i] = new Stack<char>();
        }

        foreach (var row in stackInput)
        {
            for (var i = 0; i < noOfStacks; i++)
            {
                var found = row[(i * 4) + 1];
                if (found != ' ')
                    stacks[i].Push(found);
            }
        }

        var moves = sections[1]
            .Split("\n")
            .Select(x => x.Split(' '))
            .Select(x => new Move(int.Parse(x[1]),
                         int.Parse(x[3]),
                         int.Parse(x[5])));

        return (stacks, moves);
    }

    private record Move(int NumberToMove, int From, int To);
}