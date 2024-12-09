using System.Diagnostics;

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

        while (true)
        {
            var lastUsed = diskMap.Last(x => x.Value != -1);
            var firstFree = diskMap.First(x => x.Value == -1);

            if (lastUsed.Start <= firstFree.Start)
            {
                break;
            }

            Insert(lastUsed, firstFree, diskMap);
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

        foreach (var block in orderedBlocks)
        {
            var firstFree = diskMap.Where(x => x.Value == -1)
                .OrderBy(x => x.Start)
                .ToList()
                .FirstOrDefault(x => x.Length >= block.Length && x.Start <= block.Start);

            if (firstFree == null)
            {
                continue;
            }

            Insert(block, firstFree, diskMap);
        }

        var finalLayout = diskMap.Where(x => x.Value != -1).OrderBy(x => x.Start);
        var answer = finalLayout.Sum(x => x.CheckSum);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static SortedSet<DiskSection> ParseDiskLayout(string input)
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

        return new SortedSet<DiskSection>(diskMap);
    }

    private static void Insert(DiskSection disk, DiskSection free, SortedSet<DiskSection> diskMap)
    {
        diskMap.Remove(disk);
        diskMap.Remove(free);

        if (disk.Length > free.Length)
        {
            diskMap.Add(new DiskSection(disk.Start, disk.End - free.Length, disk.Value));
            diskMap.Add(new DiskSection(free.Start, free.End, disk.Value));
        }
        else if (disk.Length == free.Length)
        {
            diskMap.Add(new DiskSection(free.Start, free.End, disk.Value));
        }
        else
        {
            diskMap.Add(new DiskSection(free.Start, free.Start + disk.Length - 1, disk.Value));
            diskMap.Add(new DiskSection(free.Start + disk.Length, free.End, -1));
        }
    }

    private record DiskSection(int Start, int End, int Value) : IComparable<DiskSection>
    {
        public int Length => End - Start + 1;
        public long CheckSum => Enumerable.Range(Start, Length).Sum(x => (long) x * Value);

        public int CompareTo(DiskSection? other) => other == null ? 1 : Start.CompareTo(other.Start);
    }
}
