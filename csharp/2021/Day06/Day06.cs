namespace AdventOfCode2021.Day06;

public class Day06
{
    private readonly AdventSession _session = new(2021, 6, "Lanternfish");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var answer = Run(ParseInput(input), 80);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(362346);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var answer = Run(ParseInput(input), 256);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(1639643057051);
    }

    private static long Run(IReadOnlyCollection<int> fish, int days)
    {
        var countOfFish = new long[9];
        for (var i = 0; i <= 8; i++)
        {
            countOfFish[i] = fish.Count(x => x == i);
        }

        for (var i = 0; i < days; i++)
        {
            var temp = countOfFish[0];
            countOfFish[0] = countOfFish[1];
            countOfFish[1] = countOfFish[2];
            countOfFish[2] = countOfFish[3];
            countOfFish[3] = countOfFish[4];
            countOfFish[4] = countOfFish[5];
            countOfFish[5] = countOfFish[6];
            countOfFish[6] = countOfFish[7] + temp;
            countOfFish[7] = countOfFish[8];
            countOfFish[8] = temp;
        }

        return countOfFish.Sum();
    }

    private static List<int> ParseInput(string input)
    {
        var lines = input.Split("\n");
        var parseOne = lines[0].Split(",").Select(int.Parse);
        return parseOne.ToList();
    }
}
