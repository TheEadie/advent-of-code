
namespace AdventOfCode2015.Day10;

public class Day10
{
    private readonly AdventSession _session = new(2015, 10, "Elves Look, Elves Say");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 252594)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        
        var start = input.Select(x => int.Parse(x.ToString()));
        var answer = Play(start).Take(40).Last().Count();
        
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
    
    [TestCase("Puzzle Input.txt", 3579328)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        
        var start = input.Select(x => int.Parse(x.ToString()));
        var answer = Play(start).Take(50).Last().Count();
        
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private IEnumerable<IEnumerable<int>> Play(IEnumerable<int> input)
    {
        var current = input;
        
        while (true)
        {
            var next = new List<int>();
            var currentDigit = current.First();
            var count = 0;
            
            foreach (var digit in current)
            {
                if (digit == currentDigit)
                {
                    count++;
                }
                else
                {
                    next.Add(count);
                    next.Add(currentDigit);
                    currentDigit = digit;
                    count = 1;
                }
            }
            
            next.Add(count);
            next.Add(currentDigit);
            yield return next;
            current = next;
        }
    }
}
