using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace AdventOfCode2015.Day12;

public class Day12
{
    private readonly AdventSession _session = new(2015, 12, "JSAbacusFramework.io");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 156366)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var regex = new Regex(@"-?\d+");
        var matches = regex.Matches(input);
        var answer = matches.Sum(x => int.Parse(x.Value));

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 4)]
    [TestCase("Puzzle Input.txt", 96852)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var json = JsonNode.Parse(input);
        var answer = Traverse(json);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static int Traverse(JsonNode? node) =>
        node switch
        {
            JsonArray array => array.Sum(Traverse),
            JsonObject obj => obj.Any(x => x.Value is JsonValue content && content.ToString() == "red")
                ? 0
                : obj.Sum(x => Traverse(x.Value)),
            _ => node is JsonValue value && int.TryParse(value.ToString(), out var num) ? num : 0
        };
}
