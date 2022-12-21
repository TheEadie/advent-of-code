namespace AdventOfCode2022;

public class Day21
{
    [TestCase("data/21 - Sample.txt", 152, TestName = "Sample")]
    [TestCase("data/21 - Puzzle Input.txt", 299_983_725_663_456, TestName = "Puzzle Input")]
    public void Part1(string inputFile, long expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        Console.WriteLine(PrintTree("root", input));
        var answer = GetMonkeyNumber("root", input);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/21 - Sample.txt", 301, TestName = "Part 2 - Sample")]
    [TestCase("data/21 - Puzzle Input.txt", 3_093_175_982_595, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, long expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var root = input.Single(x => x.Id == "root");
        var human = input.Single(x => x.Id == "humn");

        var directionFromRoot = WhichWay(human, root, input);

        while (GetDirectionTowardsMonkey(human, root, input) != human.Id)
        {
            var nextId = GetDirectionTowardsMonkey(human, root, input);
            var next = input.Single(x => x.Id == nextId);
            
            var dir = WhichWay(human, next, input);
            var value = GetMonkeyNumber(GetOppositeDirectionFromMonkey(human, next, input), input);

            switch (next.Operation)
            {
                case Operation.Sub when dir == Direction.Right:
                    // Swap x and the other side
                    input.Add(new Monkey(next.Id,
                        next.Operation,
                        next.Left,
                        directionFromRoot == Direction.Left ? root.Right : root.Left,
                        null
                    ));
                    break;
                case Operation.Div when dir == Direction.Right:
                    throw new Exception("HELP!");
                    break;
                default:
                    input.Add(new Monkey(next.Id,
                        Invert(next.Operation),
                        directionFromRoot == Direction.Left ? root.Right : root.Left,
                        dir == Direction.Left ? next.Right : next.Left,
                        null
                    ));
                    break;
            }

            var calc = dir == Direction.Left ? next.Right : next.Left;
            input.Remove(input.Single(x => x.Id == calc));
            input.Add(new Monkey(calc, Operation.Number, null, null, value));

            if (directionFromRoot == Direction.Left)
            {
                root.Left = dir == Direction.Left ? next.Left : next.Right;
                root.Right = next.Id;
            }
            else
            {
                root.Right = dir == Direction.Left ? next.Left : next.Right;
                root.Left = next.Id;
            }
            
            input.Remove(next);
            
            directionFromRoot = WhichWay(human, root, input);
        }
        
        var answer = GetMonkeyNumber(GetOppositeDirectionFromMonkey(human, root, input), input);
        
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
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

    private string PrintTree(string monkeyId, IList<Monkey> monkeys)
    {
        var monkey = monkeys.Single(x => x.Id == monkeyId);
        return monkey.Operation switch
        {
            Operation.Number => monkey.Value!.Value.ToString(),
            Operation.Add => "(" + PrintTree(monkey.Left!, monkeys) + "+" + PrintTree(monkey.Right!, monkeys) + ")",
            Operation.Sub => "(" + PrintTree(monkey.Left!, monkeys) + "-" + PrintTree(monkey.Right!, monkeys) + ")",
            Operation.Mul => "(" + PrintTree(monkey.Left!, monkeys) + "*" + PrintTree(monkey.Right!, monkeys) + ")",
            Operation.Div => "(" + PrintTree(monkey.Left!, monkeys) + "/" + PrintTree(monkey.Right!, monkeys) + ")",
            _ => throw new ArgumentOutOfRangeException()
        };        
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

    private Monkey SearchForParent(Monkey human, List<Monkey> input)
    {
        var toSearch = new Stack<Monkey>();
        var parents = new Dictionary<Monkey, Monkey>(); 
        
        toSearch.Push(input.Single(x => x.Id == "root"));

        while (toSearch.Any())
        {
            var next = toSearch.Pop();

            if (next == human)
            {
                return parents[next];
            }

            if (next.Left is null ||
                next.Right is null)
            {
                continue;
            }
            
            var left = input.Single(x => x.Id == next.Left);
            var right = input.Single(x => x.Id == next.Right);
            parents.Add(left, next);
            parents.Add(right, next);
            toSearch.Push(left);
            toSearch.Push(right);
        }

        return null;
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
                allMonkeys.Add(new (monkeyId, Operation.Number, null, null, long.Parse(opParts[0])));
            }
            else
            {
                switch (opParts[1])
                {
                    case "+":
                        allMonkeys.Add(new (monkeyId, Operation.Add,  opParts[0], opParts[2], null));
                        break;
                    case "-":
                        allMonkeys.Add(new (monkeyId, Operation.Sub,  opParts[0], opParts[2], null));
                        break;
                    case "*":
                        allMonkeys.Add(new (monkeyId, Operation.Mul,  opParts[0], opParts[2], null));
                        break;
                    case "/":
                        allMonkeys.Add(new (monkeyId, Operation.Div,  opParts[0], opParts[2], null));
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
        public string Id { get; init; }
        public Operation Operation { get; init; }
        public string? Left { get; set; }
        public string? Right { get; set; }
        public long? Value { get; set; }
        
        public Monkey(string id, Operation operation, string? left, string? right, long? value)
        {
            this.Id = id;
            this.Operation = operation;
            this.Left = left;
            this.Right = right;
            this.Value = value;
        }

    }
}