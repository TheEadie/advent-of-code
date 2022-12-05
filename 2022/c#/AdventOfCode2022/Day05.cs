namespace AdventOfCode2022;

public class Day05
{
    [TestCase("data/05 - Sample.txt", "CMZ", TestName = "Sample")]
    [TestCase("data/05 - Puzzle Input.txt", "VRWBSFZWM", TestName = "Puzzle Input")]
    public void Part1(string inputFile, string expected)
    {
        (var stacks, var moves) = Parse(File.ReadAllText(inputFile));

        foreach (var move in moves)
        {
            for (var i = 0; i < move.NumberToMove; i++)
            {
                var removed = stacks[move.From - 1].Pop();
                stacks[move.To - 1].Push(removed);
            }
        }

        var answer = "";
        foreach (var stack in stacks)
        {
            answer += stack.Peek();
        }

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/05 - Sample.txt", "MCD", TestName = "Sample")]
    [TestCase("data/05 - Puzzle Input.txt", "RBTWJWMCF", TestName = "Puzzle Input")]
    public void Part2(string inputFile, string expected)
    {
        (var stacks, var moves) = Parse(File.ReadAllText(inputFile));

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

        var answer = "";

        foreach (var stack in stacks)
        {
            answer += stack.Peek();
        }

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static (Stack<char>[], IEnumerable<Move>) Parse(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var stackInput = sections[0].Split(Environment.NewLine).Reverse();
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
            .Split(Environment.NewLine)
            .Select(x => x.Split(' '))
            .Select(x => new Move(int.Parse(x[1]),
                         int.Parse(x[3]),
                         int.Parse(x[5])));

        return (stacks, moves);
    }

    private record Move(int NumberToMove, int From, int To);
}