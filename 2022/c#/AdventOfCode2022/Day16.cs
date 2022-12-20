using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16
{
    [TestCase("data/16 - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var answer = 0;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/16 - Sample.txt", 0, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private IEnumerable<Valve> ParseInput(string input)
    {
        Valve ParseValve (string line)
        {
            var regex = new Regex(
                //"Sensor at x=([-|0-9]+), y=([-|0-9]+): closest beacon is at x=([-|0-9]+), y=([-|0-9]+)");
                "Valve ([\\S+]) has flow rate=([0-9]+); tunnels lead to valves ([.*])");
            var groups = regex.Match(line).Groups;

            return new Valve(groups[1].Value, int.Parse(groups[2].Value), groups[3].Value.Split(", "));
        }
        
        return input.Split("\n").Select(ParseValve);
        
    }

    private record Valve(string Id, int FlowRate, IEnumerable<string> Tunnels);
}