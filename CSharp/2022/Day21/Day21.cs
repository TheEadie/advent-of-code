namespace AdventOfCode2022.Day21;

public class Day21
{
    [TestCase("Day21/Sample.txt", 152, TestName = "Day 21 - Part 1 - Sample")]
    [TestCase("Day21/Puzzle Input.txt", 299_983_725_663_456, TestName = "Day 21 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, long expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var answer = GetMonkeyNumber("root", input);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day21/Sample.txt", 301, TestName = "Day 21 - Part 2 - Sample")]
    [TestCase("Day21/Puzzle Input.txt", 3_093_175_982_595, TestName = "Day 21 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, long expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var root = input.Single(x => x.Id == "root");
        var human = input.Single(x => x.Id == "humn");

        var answer = Solve(human, root, input);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static long Solve(Monkey valueToSolve, Monkey equalityPoint, List<Monkey> input)
    {
        while (GetDirectionTowardsMonkey(valueToSolve, equalityPoint, input) != valueToSolve.Id)
        {
            var directionFromRootToValueToSolve = WhichWay(valueToSolve, equalityPoint, input);
            var rootMonkeyIdAwayFromValueToSolve = GetOppositeDirectionFromMonkey(valueToSolve, equalityPoint, input);

            var nextMonkeyId = GetDirectionTowardsMonkey(valueToSolve, equalityPoint, input);
            var nextMonkey = input.Single(x => x.Id == nextMonkeyId);

            var directionToValueToSolve = WhichWay(valueToSolve, nextMonkey, input);
            var monkeyIdAwayFromValueToSolve = GetOppositeDirectionFromMonkey(valueToSolve, nextMonkey, input);
            var value = GetMonkeyNumber(monkeyIdAwayFromValueToSolve, input);

            switch (nextMonkey.Operation)
            {
                case Operation.Sub when directionToValueToSolve == Direction.Right:
                    // Swap x and the other side
                    input.Add(new Monkey(nextMonkey.Id,
                        nextMonkey.Operation,
                        nextMonkey.Left,
                        rootMonkeyIdAwayFromValueToSolve,
                        null
                    ));
                    break;
                case Operation.Div when directionToValueToSolve == Direction.Right:
                    throw new Exception("HELP!");
                default:
                    input.Add(new Monkey(nextMonkey.Id,
                        Invert(nextMonkey.Operation),
                        rootMonkeyIdAwayFromValueToSolve,
                        monkeyIdAwayFromValueToSolve,
                        null
                    ));
                    break;
            }

            input.Remove(input.Single(x => x.Id == monkeyIdAwayFromValueToSolve));
            input.Add(new Monkey(monkeyIdAwayFromValueToSolve, Operation.Number, null, null, value));

            if (directionFromRootToValueToSolve == Direction.Left)
            {
                equalityPoint.Left = GetDirectionTowardsMonkey(valueToSolve, nextMonkey, input);
                equalityPoint.Right = nextMonkey.Id;
            }
            else
            {
                equalityPoint.Right = GetDirectionTowardsMonkey(valueToSolve, nextMonkey, input);
                equalityPoint.Left = nextMonkey.Id;
            }

            input.Remove(nextMonkey);
        }

        var answer = GetMonkeyNumber(GetOppositeDirectionFromMonkey(valueToSolve, equalityPoint, input), input);
        return answer;
    }

    private static string GetOppositeDirectionFromMonkey(Monkey toFind, Monkey start, List<Monkey> input)
    {
        return WhichWay(toFind, start, input) == Direction.Left ? start.Right! : start.Left!;
    }

    private static string GetDirectionTowardsMonkey(Monkey toFind, Monkey start, List<Monkey> input)
    {
        return WhichWay(toFind, start, input) == Direction.Left ? start.Left! : start.Right!;
    }

    private enum Direction
    {
        Left,
        Right
    }

    private static Direction WhichWay(Monkey toFind, Monkey start, List<Monkey> input)
    {
        var toSearch = new Stack<Monkey>();

        toSearch.Push(input.Single(x => x.Id == start.Left));

        while (toSearch.Any())
        {
            var next = toSearch.Pop();

            if (next == toFind)
            {
                return Direction.Left;
            }

            if (next.Left is null ||
                next.Right is null)
            {
                continue;
            }

            var left = input.Single(x => x.Id == next.Left);
            var right = input.Single(x => x.Id == next.Right);
            toSearch.Push(left);
            toSearch.Push(right);
        }

        // Assume it must be right
        return Direction.Right;
    }

    private static IEnumerable<Monkey> ParseInput(string input)
    {
        var lines = input.Split("\n");
        var allMonkeys = new List<Monkey>();

        foreach (var line in lines)
        {
            var parts = line.Split(": ");
            var monkeyId = parts[0];
            var op = parts[1];
            var opParts = op.Split(" ");
            if (opParts.Length == 1)
            {
                allMonkeys.Add(new(monkeyId, Operation.Number, null, null, long.Parse(opParts[0])));
            }
            else
            {
                switch (opParts[1])
                {
                    case "+":
                        allMonkeys.Add(new Monkey(monkeyId, Operation.Add, opParts[0], opParts[2], null));
                        break;
                    case "-":
                        allMonkeys.Add(new Monkey(monkeyId, Operation.Sub, opParts[0], opParts[2], null));
                        break;
                    case "*":
                        allMonkeys.Add(new Monkey(monkeyId, Operation.Mul, opParts[0], opParts[2], null));
                        break;
                    case "/":
                        allMonkeys.Add(new Monkey(monkeyId, Operation.Div, opParts[0], opParts[2], null));
                        break;
                }
            }
        }

        return allMonkeys;
    }

    private static long GetMonkeyNumber(string monkeyId, IList<Monkey> monkeys)
    {
        var monkey = monkeys.Single(x => x.Id == monkeyId);
        return monkey.Operation switch
        {
            Operation.Number => monkey.Value!.Value,
            Operation.Add => GetMonkeyNumber(monkey.Left!, monkeys) + GetMonkeyNumber(monkey.Right!, monkeys),
            Operation.Sub => GetMonkeyNumber(monkey.Left!, monkeys) - GetMonkeyNumber(monkey.Right!, monkeys),
            Operation.Mul => GetMonkeyNumber(monkey.Left!, monkeys) * GetMonkeyNumber(monkey.Right!, monkeys),
            Operation.Div => GetMonkeyNumber(monkey.Left!, monkeys) / GetMonkeyNumber(monkey.Right!, monkeys),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private enum Operation
    {
        Number,
        Add,
        Sub,
        Mul,
        Div
    }

    private static Operation Invert(Operation op)
    {
        return op switch
        {
            Operation.Add => Operation.Sub,
            Operation.Sub => Operation.Add,
            Operation.Mul => Operation.Div,
            Operation.Div => Operation.Mul,
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }

    private class Monkey
    {
        public string Id { get; }
        public Operation Operation { get; }
        public string? Left { get; set; }
        public string? Right { get; set; }
        public long? Value { get; }

        public Monkey(string id, Operation operation, string? left, string? right, long? value)
        {
            Id = id;
            Operation = operation;
            Left = left;
            Right = right;
            Value = value;
        }

    }
}