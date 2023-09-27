using System.Text.Json;
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
    [TestCase("Puzzle Input.txt", 0)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        
        var objectScopes = new Stack<int>();
        var arrayScopes = new Stack<int>();
        var subTotal = 0;
        var objects = new List<string>();

        for(var i = 0; i < input.Length; i++)
        {
            var character = input[i];
            if (character == '{')
            {
                objectScopes.Push(i);
            }

            if (character == '}')
            {
                var start = objectScopes.Pop();
                var content = input.Substring(start, i - start + 1);
                
                if (objectScopes.Count == 0)
                    objects.Add(content);
            }

            if (character == '[')
            {
                arrayScopes.Push(i);
            }

            if (character == ',')
            {
                var start = arrayScopes.Pop();
                var content = input.Substring(start + 1, i - start);
            }
        }


        var answer = subTotal;
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
}
