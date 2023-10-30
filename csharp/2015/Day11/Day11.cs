
namespace AdventOfCode2015.Day11;

public class Day11
{
    private readonly AdventSession _session = new(2015, 11, "Corporate Policy");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", "abcdffaa")]
    [TestCase("Puzzle Input.txt", "hepxxyzz")]
    public async Task Part1(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);

        while (!IsValid(input))
        {
            input = Increment(input);
        }

        var answer = input;
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", "heqaabcc")]
    public async Task Part2(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);

        while (!IsValid(input))
        {
            input = Increment(input);
        }

        input = Increment(input);

        while (!IsValid(input))
        {
            input = Increment(input);
        }

        var answer = input;
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static string Increment(string input)
    {
        var chars = input.ToCharArray();
        var index = chars.Length - 1;
        while (index >= 0)
        {
            if (chars[index] == 'z')
            {
                chars[index] = 'a';
                index--;
            }
            else
            {
                chars[index]++;
                break;
            }
        }

        return new string(chars);
    }

    private static bool IsValid(string input)
    {
        if (input.Contains('i') || input.Contains('o') || input.Contains('l'))
        {
            return false;
        }

        var hasStraight = false;

        for (var i = 0; i < input.Length - 2; i++)
        {
            var first = input[i] - 'a';
            var second = input[i + 1] - 'a';
            var third = input[i + 2] - 'a';
            if (first == second - 1 && second == third - 1)
            {
                hasStraight = true;
                break;
            }
        }

        if (!hasStraight)
        {
            return false;
        }

        var numberOfPairs = 0;
        for (var i = 0; i < input.Length - 1; i++)
        {
            var first = input[i];
            var second = input[i + 1];
            if (first == second)
            {
                numberOfPairs++;
                i++;
            }
        }

        return numberOfPairs >= 2;
    }
}
