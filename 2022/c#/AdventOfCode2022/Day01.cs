namespace AdventOfCode2022;

public class Day01
{
    [TestCase("data/01 - Sample.txt", 24000, TestName = "Sample")]
    [TestCase("data/01 - Puzzle Input.txt", 69795, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var elves = ParseInput(File.ReadAllLines(inputFile));

        var answer = elves.Max(x => x.Calories.Sum());
        
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
    
    [TestCase("data/01 - Sample.txt", 45000, TestName = "Sample")]
    [TestCase("data/01 - Puzzle Input.txt", 208437, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var elves = ParseInput(File.ReadAllLines(inputFile));

        var answer = elves.OrderByDescending(x => x.Calories.Sum())
            .Take(3)
            .SelectMany(x => x.Calories)
            .Sum();
        
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Elf> ParseInput(IEnumerable<string> input)
    {
        var builder = new ElfBuilder();
        var elves = new List<Elf>();

        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(builder.Build());
                builder = new ElfBuilder();
            }
            else
            {
                builder.Add(int.Parse(line));
            }
        }
        
        // Add the last elf
        elves.Add(builder.Build());
        return elves;
    }

    private record Elf(IReadOnlyCollection<int> Calories);

    private class ElfBuilder
    {
        private readonly List<int> _calories = new();

        public void Add(int calorie)
        {
            _calories.Add(calorie);
        }

        public Elf Build() => new(_calories);
    }
}