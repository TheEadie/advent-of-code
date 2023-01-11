namespace AdventOfCode2022.Day20;

public class Day20
{
    [TestCase("Day20/Sample.txt", 3, TestName = "Day 20 - Part 1 - Sample")]
    [TestCase("Day20/Puzzle Input.txt", 6640, TestName = "Day 20 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, double expected)
    {
        var input = File.ReadAllText(inputFile)
            .Split("\n")
            .Select(double.Parse)
            .ToList();

        var mixed = Mix(input).Take(1).Last();
        var (x, y, z) = GetCoordinates(mixed);
        var answer = x + y + z;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day20/Sample.txt", 1_623_178_306, TestName = "Day 20 - Part 2 - Sample")]
    [TestCase("Day20/Puzzle Input.txt", 11_893_839_037_215, TestName = "Day 20 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, double expected)
    {
        var input = File.ReadAllText(inputFile)
            .Split("\n")
            .Select(i => double.Parse(i) * 811589153)
            .ToList();

        var mixed = Mix(input).Take(10).Last();
        var (x, y, z) = GetCoordinates(mixed);
        var answer = x + y + z;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static IEnumerable<IList<double>> Mix(IList<double> list)
    {
        var length = list.Count;
        var workingSet = list.Select((x, i) => (x, i)).ToList();
        var startingSet = workingSet.ToArray();

        while (true)
        {
            for (var i = 0; i < length; i++)
            {
                var value = startingSet.ElementAt(i);
                var currentPosition = workingSet.IndexOf(value);

                var offset = currentPosition + value.Item1;
                var newPosition = (int)Mod(offset, length - 1);

                workingSet.RemoveAt(currentPosition);
                workingSet.Insert(newPosition, value);
            }

            yield return workingSet.Select(x => x.x).ToList();
        }
    }

    private static (double, double, double) GetCoordinates(IList<double> list)
    {
        var firstZeroPosition = list.IndexOf(list.First(x => x == 0));

        var one = list[(firstZeroPosition + 1000) % list.Count];
        var two = list[(firstZeroPosition + 2000) % list.Count];
        var three = list[(firstZeroPosition + 3000) % list.Count];
        return (one, two, three);
    }

    private static double Mod(double x, double m)
    {
        return x - m * Math.Floor(x / m);
    }
}