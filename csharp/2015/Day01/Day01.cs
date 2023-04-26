namespace AdventOfCode2015.Day01;

public class Day01
{
    private readonly AdventSession _session = new(2015, 1, "Not Quite Lisp");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var moves = await ParseInput();

        var answer = moves.Sum();
        Console.WriteLine(answer);
        answer.ShouldBe(138);
    }

    [Test]
    public async Task Part2()
    {
        var moves = await ParseInput();
        var currentFloor = 0;
        var answer = 0;
        for (var i = 0; i < moves.Length; i++)
        {
            currentFloor += moves[i];
            if (currentFloor == -1)
            {
                answer = i + 1;
                break;
            }
        }

        Console.WriteLine(answer);
        answer.ShouldBe(1771);
    }

    private async Task<int[]> ParseInput()
    {
        var input = await _session.Start("Puzzle Input.txt");
        return input.ToCharArray().Select(x => (x == '(') ? 1 : -1).ToArray();
    }
}
