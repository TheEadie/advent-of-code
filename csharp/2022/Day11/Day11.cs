namespace AdventOfCode2022.Day11;

public class Day11
{
    private readonly AdventSession _session = new(2022, 11, "Monkey in the Middle");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 10605)]
    [TestCase("Puzzle Input.txt", 95472)]
    public async Task Part1(string inputFile, int expected)
    {
        var _ = await _session.Start(inputFile);
        var monkeys = ParseInput(inputFile);

        var inspected = Inspect(monkeys, x => x / 3).Take(20).Last();
        var topTwo = inspected.OrderByDescending(x => x).Take(2).ToArray();

        var answer = topTwo[0] * topTwo[1];

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 2713310158)]
    [TestCase("Puzzle Input.txt", 17926061332)]
    public async Task Part2(string inputFile, long expected)
    {
        var _ = await _session.Start(inputFile);
        var input = ParseInput(inputFile);

        var commonFactor = input.Select(x => x.Divider).Aggregate((total, x) => total * x);

        var inspected = Inspect(input, x => x % commonFactor).Take(10_000).Last();
        var topTwo = inspected.OrderByDescending(x => x).Take(2).ToArray();

        var answer = topTwo[0] * topTwo[1];

        _session.PrintAnswer(2, answer);
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
                    _ = monkey.Items.Remove(d);
                }
            }
            yield return inspected;
        }


        // ReSharper disable once IteratorNeverReturns
    }

    private static Monkey[] ParseInput(string inputFile)
    {
        return inputFile.Contains("Sample")
            ? (new[]
            {
                new Monkey(0, new List<long> {79, 98}, o => o * 19, 23, 2, 3),
                new Monkey(1, new List<long> {54, 65, 75, 74}, o => o + 6, 19, 2, 0),
                new Monkey(2, new List<long> {79, 60, 97}, o => o * o, 13, 1, 3),
                new Monkey(3, new List<long> {74}, o => o + 3, 17, 0, 1)
            })
            : inputFile.Contains("Puzzle Input")
            ? (new[]
            {
                new Monkey(0, new List<long> {52, 60, 85, 69, 75, 75}, o => o * 17, 13, 6, 7),
                new Monkey(1, new List<long> {96, 82, 61, 99, 82, 84, 85}, o => o + 8, 7, 0, 7),
                new Monkey(2, new List<long> {95, 79}, o => o + 6, 19, 5, 3),
                new Monkey(3, new List<long> {88, 50, 82, 65, 77}, o => o * 19, 2, 4, 1),
                new Monkey(4, new List<long> {66, 90, 59, 90, 87, 63, 53, 88}, o => o + 7, 5, 1, 0),
                new Monkey(5, new List<long> {92, 75, 62}, o => o * o, 3, 3, 4),
                new Monkey(6, new List<long> {94, 86, 76, 67}, o => o + 1, 11, 5, 2),
                new Monkey(7, new List<long> {57}, o => o + 2, 17, 6, 2),
            })
            : throw new NotImplementedException();
    }

    private record Monkey(
        int Number,
        List<long> Items,
        Func<long, long> Operation,
        long Divider,
        int PassToOnTrue,
        int PassToOnFalse);
}
