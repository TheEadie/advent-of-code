using Shouldly;

namespace AdventOfCode2022;

public class Day01
{
    [Test]
    public void Part1()
    {
        var elves = ParseInput(File.ReadAllLines("Day01.txt"));

        var answer = elves.Max(x => x.Calories.Sum());
        
        Console.WriteLine(answer);
        answer.ShouldBe(69795);
    }
    
    [Test]
    public void Part2()
    {
        var elves = ParseInput(File.ReadAllLines("Day01.txt"));

        var answer = elves.OrderByDescending(x => x.Calories.Sum())
            .Take(3)
            .SelectMany(x => x.Calories)
            .Sum();
        
        Console.WriteLine(answer);
        answer.ShouldBe(208437);
    }

    private static IEnumerable<Elf> ParseInput(IEnumerable<string> input)
    {
        var builder = new List<int>();
        var elves = new List<Elf>();

        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(new Elf(builder));
                builder = new List<int>();
            }
            else
            {
                builder.Add(int.Parse(line));
            }
        }

        return elves;
    }

    private record Elf(IEnumerable<int> Calories);
}