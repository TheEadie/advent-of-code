namespace AdventOfCode2019.Day23;

public class Day23
{
    // https://adventofcode.com/2019/day/23
    private readonly AdventSession _session = new(2019, 23, "Category Six");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 27_182)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var computers = CreateNetwork(program, 50);
        var (_, answer) = GetPacketsSentToNat(computers).First();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 19_285)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();

        var computers = CreateNetwork(program, 50);
        var lastY = -1L;

        var answer = GetPacketsSentToNat(computers)
            .TakeUntil(x =>
            {
                if (lastY == x.Item2)
                {
                    return true;
                }

                lastY = x.Item2;
                return false;
            }).ToList().Last().Item2;

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static List<NetworkedComputer> CreateNetwork(long[] program, int size)
    {
        var computers = Enumerable.Range(0, size)
            .Select(x =>
                new NetworkedComputer(
                    x,
                    new IntCode.IntCode(program),
                    new Queue<(long, long)>()))
            .ToList();

        foreach(var computer in computers)
        {
            computer.Queue.Enqueue((computer.Address, 0));
        }

        return computers;
    }

    private static IEnumerable<(long, long)> GetPacketsSentToNat(List<NetworkedComputer> computers)
    {
        var nat = (x: 0L, y: 0L);

        while (true)
        {
            foreach (var (_, intCode, queue) in computers)
            {
                IEnumerable<long> outputs;

                if (queue.Count != 0)
                {
                    var input = queue.Dequeue();
                    (_, _) = intCode.Run(input.Item1);
                    (_, outputs) = intCode.Run(input.Item2);
                }
                else
                {
                    (_, outputs) = intCode.Run(-1);
                }

                foreach (var output in outputs.Chunk(3))
                {
                    var address = (int) output[0];
                    var x = output[1];
                    var y = output[2];

                    if (address == 255)
                    {
                        nat = (x, y);
                    }
                    else
                    {
                        computers[address].Queue.Enqueue((x, y));
                    }
                }
            }

            if (computers.All(x => x.Queue.Count == 0))
            {
                if (nat != (0, 0))
                {
                    yield return nat;
                }

                computers[0].Queue.Enqueue((nat.x, nat.y));
            }
        }
    }

    private record NetworkedComputer(int Address, IntCode.IntCode Computer, Queue<(long, long)> Queue);
}
