namespace AdventOfCode2022.Day20;

public class Day20
{
    private readonly AdventSession _session = new(2022, 20, "Grove Positioning System");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 3)]
    [TestCase("Puzzle Input.txt", 6640)]
    public async Task Part1(string inputFile, double expected)
    {
        var input = await _session.Start(inputFile);
        var numbers = input
            .Split("\n")
            .Select(double.Parse)
            .ToList();

        var mixed = Mix(numbers).Take(1).Last();
        var (x, y, z) = GetCoordinates(mixed);
        var answer = x + y + z;

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 1_623_178_306)]
    [TestCase("Puzzle Input.txt", 11_893_839_037_215)]
    public async Task Part2(string inputFile, double expected)
    {
        var input = await _session.Start(inputFile);
        var numbers = input
            .Split("\n")
            .Select(i => double.Parse(i) * 811589153)
            .ToList();

        var mixed = Mix(numbers).Take(10).Last();
        var (x, y, z) = GetCoordinates(mixed);
        var answer = x + y + z;

        _session.PrintAnswer(2, answer);
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

                var offset = currentPosition + value.x;
                var newPosition = (int) Mod(offset, length - 1);

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

    private static double Mod(double x, double m) => x - m * Math.Floor(x / m);
}
