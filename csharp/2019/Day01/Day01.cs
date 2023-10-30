namespace AdventOfCode2019.Day01;

public class Day01
{
    private readonly AdventSession _session = new(2019, 1, "The Tyranny of the Rocket Equation");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var answer = input
            .Split("\n")
            .Select(int.Parse)
            .Select(GetFuelForModule)
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(3401852);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var answer = input
            .Split("\n")
            .Select(int.Parse)
            .Select(GetTotalFuel)
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(5099916);
    }

    private static int GetTotalFuel(int moduleSize)
    {
        var fuel = GetFuelForModule(moduleSize);

        if (fuel >= 0)
        {
            return fuel + GetTotalFuel(fuel);
        }

        fuel = 0;
        return fuel;

    }

    private static int GetFuelForModule(int moduleSize) => (int)Math.Round((double)moduleSize / 3, 0, MidpointRounding.ToZero) - 2;
}
