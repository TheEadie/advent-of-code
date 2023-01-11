using System.Text;

namespace AdventOfCode2022.Day25;

public class Day25
{
    [TestCase("Day25/Sample.txt", "2=-1=0", TestName = "Day 25 - Part 1 - Sample")]
    [TestCase("Day25/Puzzle Input.txt", "2011-=2=-1020-1===-1", TestName = "Day 25 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, string expected)
    {
        var total = File.ReadAllLines(inputFile).Select(GetDecimalFromSNAFU).Sum();

        var answer = GetSNAFUFromDecimal(total);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static long GetDecimalFromSNAFU(string snafu)
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

    private static string GetSNAFUFromDecimal(long input)
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