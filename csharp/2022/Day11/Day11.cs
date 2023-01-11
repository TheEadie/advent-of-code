namespace AdventOfCode2022.Day11;

public class Day11
{
    [TestCase("Day11/Sample.txt", 10605, TestName = "Day 11 - Part 1 - Sample")]
    [TestCase("Day11/Puzzle Input.txt", 95472, TestName = "Day 11 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var monkeys = ParseInput(inputFile);

        var inspected = Inspect(monkeys, x => x / 3).Take(20).Last();
        var topTwo = inspected.OrderByDescending(x => x).Take(2).ToArray();

        var answer = topTwo[0] * topTwo[1];

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day11/Sample.txt", 2713310158, TestName = "Day 11 - Part 2 - Sample")]
    [TestCase("Day11/Puzzle Input.txt", 17926061332, TestName = "Day 11 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, long expected)
    {
        var input = ParseInput(inputFile);

        var commonFactor = input.Select(x => x.Divider).Aggregate((total, x) => total * x);

        var inspected = Inspect(input, x => x % commonFactor).Take(10_000).Last();
        var topTwo = inspected.OrderByDescending(x => x).Take(2).ToArray();

        var answer = topTwo[0] * topTwo[1];

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static IEnumerable<IEnumerable<long>> Inspect(IReadOnlyList<Monkey> monkeys, Func<long, long> worryControl)
    {
        var inspected = new long[monkeys.Count];

        while (true)
        {
            foreach (var monkey in monkeys)
            {
                var toRemove = new List<long>();

                foreach (var item in monkey.Items)
                {
                    inspected[monkey.Number]++;

                    var newItem = monkey.Operation(item);
                    newItem = worryControl(newItem);

                    if (newItem % monkey.Divider == 0)
                    {
                        monkeys[monkey.PassToOnTrue].Items.Add(newItem);
                    }
                    else
                    {
                        monkeys[monkey.PassToOnFalse].Items.Add(newItem);
                    }

                    toRemove.Add(item);
                }

                foreach (var d in toRemove)
                {
                    monkey.Items.Remove(d);
                }
            }
            yield return inspected;
        }


    }

    private Monkey[] ParseInput(string inputFile)
    {
        if (inputFile.Contains("Sample"))
        {
            return new[]
            {
                new Monkey(0, new List<long> {79, 98}, o => o * 19, 23, 2, 3),
                new Monkey(1, new List<long> {54, 65, 75, 74}, o => o + 6, 19, 2, 0),
                new Monkey(2, new List<long> {79, 60, 97}, o => o * o, 13, 1, 3),
                new Monkey(3, new List<long> {74}, o => o + 3, 17, 0, 1)
            };
        }
        if (inputFile.Contains("Puzzle Input"))
        {
            return new[]
            {
                new Monkey(0, new List<long> {52, 60, 85, 69, 75, 75}, o => o * 17, 13, 6, 7),
                new Monkey(1, new List<long> {96, 82, 61, 99, 82, 84, 85}, o => o + 8, 7, 0, 7),
                new Monkey(2, new List<long> {95, 79}, o => o + 6, 19, 5, 3),
                new Monkey(3, new List<long> {88, 50, 82, 65, 77}, o => o * 19, 2, 4, 1),
                new Monkey(4, new List<long> {66, 90, 59, 90, 87, 63, 53, 88}, o => o + 7, 5, 1, 0),
                new Monkey(5, new List<long> {92, 75, 62}, o => o * o, 3, 3, 4),
                new Monkey(6, new List<long> {94, 86, 76, 67}, o => o + 1, 11, 5, 2),
                new Monkey(7, new List<long> {57}, o => o + 2, 17, 6, 2),
            };
        }

        throw new NotImplementedException();
    }

    private record Monkey(
        int Number,
        List<long> Items,
        Func<long, long> Operation,
        long Divider,
        int PassToOnTrue,
        int PassToOnFalse);
}