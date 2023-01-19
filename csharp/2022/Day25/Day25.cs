using System.Text;

namespace AdventOfCode2022.Day25;

public class Day25
{
    private readonly AdventSession _session = new(2022, 25, "Full of Hot Air");

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", "2=-1=0")]
    [TestCase("Puzzle Input.txt", "2011-=2=-1020-1===-1")]
    public async Task Part1(string inputFile, string expected)
    {
        var input = await _session.Start(inputFile);
        var total = input.Split("\n").Select(GetDecimalFromSnafu).Sum();

        var answer = GetSnafuFromDecimal(total);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    private static long GetDecimalFromSnafu(string snafu)
    {
        long number = 0;
        long i = 1;

        foreach (var character in snafu.ToCharArray().Reverse())
        {
            number += character switch
            {
                '2' => 2 * i,
                '1' => 1 * i,
                '0' => 0 * i,
                '-' => -1 * i,
                '=' => -2 * i,
                _ => throw new ArgumentOutOfRangeException(character.ToString())
            };

            i *= 5;
        }

        return number;
    }

    private static string GetSnafuFromDecimal(long input)
    {
        var snafu = new StringBuilder();

        while (input >= 1)
        {
            var remainder = input % 5;
            input /= 5;

            if (remainder > 2)
            {
                input++;
                remainder -= 5;
            }

            snafu.Insert(0, remainder switch
            {
                -2 => "=",
                -1 => "-",
                0 => "0",
                1 => "1",
                2 => "2",
                _ => throw new ArgumentOutOfRangeException(remainder.ToString())
            });
        }

        return snafu.ToString();
    }
}