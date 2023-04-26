using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2015.Day04;

public class Day04
{
    private readonly AdventSession _session = new(2015, 4, "The Ideal Stocking Stuffer");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public void Part1()
    {
        const string input = "yzbqklnj";
        const int maxToCheck = 1_000_000;

        var answer = Enumerable.Range(0, maxToCheck)
            .Select(x => new { Number = x, Hash = GetMd5Hash(input + x) })
            .First(x => x.Hash.StartsWith("00000"))
            .Number;

        Console.WriteLine(answer);
        answer.ShouldBe(282749);
    }

    [Test]
    public void Part2()
    {
        const string input = "yzbqklnj";
        const int maxToCheck = 1_000_000_000;

        var answer = Enumerable.Range(0, maxToCheck)
            .Select(x => new { Number = x, Hash = GetMd5Hash(input + x) })
            .First(x => x.Hash.StartsWith("000000"))
            .Number;

        Console.WriteLine(answer);
        answer.ShouldBe(9962624);
    }

    private static string GetMd5Hash(string input)
    {
        using var md5 = MD5.Create();

        var hash = md5.ComputeHash(new UTF8Encoding().GetBytes(input));
        var encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        return encoded;
    }
}
