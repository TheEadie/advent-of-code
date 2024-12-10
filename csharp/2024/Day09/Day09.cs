namespace AdventOfCode2024.Day09;

public class Day09
{
    // https://adventofcode.com/2024/day/9
    private readonly AdventSession _session = new(2024, 09, "Disk Fragmenter");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 1_928)]
    [TestCase("Puzzle Input.txt", 6_154_342_787_400)]
    public async Task Part1(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var diskMap = ParseDiskLayout(input);

        var toSort = new Stack<DiskSection>();
        var freeSpace = new Stack<DiskSection>();

        toSort.PushRange(diskMap.Where(disk => disk.Value != -1));
        freeSpace.PushRange(diskMap.Where(disk => disk.Value == -1).Reverse());

        while (true)
        {
            var lastUsed = toSort.Pop();
            var firstFree = freeSpace.Pop();

            if (lastUsed.Start <= firstFree.Start)
            {
                break;
            }

            var (newUsed, newFree) = Move(diskMap, lastUsed, firstFree);
            toSort.PushRange(newUsed);
            freeSpace.PushRange(newFree);
        }

        var finalLayout = diskMap.Where(x => x.Value != -1);
        var answer = finalLayout.Sum(x => x.CheckSum);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 2_858)]
    [TestCase("Puzzle Input.txt", 6_183_632_723_350)]
    public async Task Part2(string inputFile, long expected)
    {
        var input = await _session.Start(inputFile);
        var diskMap = ParseDiskLayout(input);

        var orderedBlocks = diskMap.Where(x => x.Value != -1).OrderByDescending(x => x.Value).ToList();

        var freeSpace = diskMap.Where(x => x.Value == -1)
            .GroupBy(x => x.Length)
            .ToDictionary(
                x => x.Key,
                x =>
                    {
                        var spaces = new PriorityQueue<DiskSection, int>();
                        spaces.EnqueueRange(x.Select(y => (y, y.Start)));
                        return spaces;
                    });

        foreach (var block in orderedBlocks)
        {
            var candidates = freeSpace.Where(x => x.Key >= block.Length)
                .Select(
                    x =>
                        {
                            x.Value.TryPeek(out var found, out var score);
                            return (found, score);
                        })
                .Where(x => x.found is not null && x.score < block.Start)
                .ToList();

            if (candidates.Count == 0)
            {
                continue;
            }

            var (free, _) = candidates.MinBy(x => x.score);
            freeSpace[free!.Length].Dequeue();

            var (_, newFree) = Move(diskMap, block, free);
            foreach (var section in newFree)
            {
                freeSpace[section.Length].Enqueue(section, section.Start);
            }
        }

        var finalLayout = diskMap.Where(x => x.Value != -1).OrderBy(x => x.Start);
        var answer = finalLayout.Sum(x => x.CheckSum);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static (IEnumerable<DiskSection> NewUsed, IEnumerable<DiskSection> NewFree) Move(
        HashSet<DiskSection> map,
        DiskSection used,
        DiskSection free)
    {
        map.Remove(used);
        map.Remove(free);

        if (used.Length > free.Length)
        {
            map.Add(new DiskSection(used.Start, used.End - free.Length, used.Value));
            map.Add(new DiskSection(free.Start, free.End, used.Value));
            return ([new DiskSection(used.Start, used.End - free.Length, used.Value)], []);
        }

        if (used.Length == free.Length)
        {
            map.Add(new DiskSection(free.Start, free.End, used.Value));
            return ([], []);
        }

        map.Add(new DiskSection(free.Start, free.Start + used.Length - 1, used.Value));
        map.Add(new DiskSection(free.Start + used.Length, free.End, -1));
        return ([], [new DiskSection(free.Start + used.Length, free.End, -1)]);
    }


    private static HashSet<DiskSection> ParseDiskLayout(string input)
    {
        var numbers = input.Select(x => int.Parse(x.ToString()));
        var diskMap = new List<DiskSection>();
        var pos = 0;
        var id = 0;
        var free = false;
        foreach (var n in numbers)
        {
            if (n == 0)
            {
                free = !free;
                continue;
            }

            diskMap.Add(new DiskSection(pos, pos + n - 1, free ? -1 : id++));
            pos += n;
            free = !free;
        }

        return diskMap.ToHashSet();
    }

    private record DiskSection(int Start, int End, int Value)
    {
        public int Length => End - Start + 1;
        public long CheckSum => Enumerable.Range(Start, Length).Sum(x => (long) x * Value);
    }
}
