using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day19
{
    [TestCase("data/19 - Sample.txt", 33, TestName = "Day 19 - Part 1 - Sample")]
    [TestCase("data/19 - Puzzle Input.txt", 0, TestName = "Day 19 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var hack = FindLargestNumberOfGeodes(input.First(), 24);
        
        

        var answer = input.Select(x => (Blueprint: x, Max: FindLargestNumberOfGeodes(x, 24)))
            .Select(x => x.Blueprint.Id * x.Max)
            .Sum();
        
        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static int FindLargestNumberOfGeodes(Blueprint blueprint, int maxTime)
    {
        var statesToTry = new PriorityQueue<State, int>();
        var statesTried = new HashSet<State>();
        var cameFrom = new Dictionary<State, State>();

        var start = new State(0, 0, 0, 0, 0, 1, 0, 0, 0);
        statesToTry.Enqueue(start, GetPotential(start, maxTime));

        while (statesToTry.Count > 0)
        {
            var current = statesToTry.Dequeue();
            if(statesTried.Contains(current))
                continue;
            
            statesTried.Add(current);

            if (current.Time == maxTime)
            {
                TestContext.Progress.WriteLine($"Blueprint {blueprint.Id}: {current.Geodes}");
                
                var totalPath = new List<State> { current };
                
                var track = current;
                while (cameFrom.ContainsKey(track))
                {
                    track = cameFrom[track];
                    totalPath = totalPath.Prepend(track).ToList();
                }
                
                TestContext.Progress.WriteLine(string.Join(Environment.NewLine, totalPath));
                
                return current.Geodes;
            }

            foreach (var next in GetPossibleStates(current, blueprint))
            {
                var nextPotential = GetPotential(next, maxTime);
                cameFrom[next] = current;
                statesToTry.Enqueue(next, -(next.Geodes + nextPotential));
            }
        }

        return 0;
    }

    private static int GetPotential(State next, int maxTime)
    {
        return (maxTime - next.Time) * ((maxTime - next.Time) + 1) / 2;
    }

    private static IEnumerable<State> GetPossibleStates(State current, Blueprint blueprint)
    {
        var nextTime = current with
        {
            Time = current.Time + 1,
            Ore = current.Ore + current.OreRobots,
            Clay = current.Clay + current.ClayRobots,
            Obsidian = current.Obsidian + current.ObsidianRobots,
            Geodes = current.Geodes + current.GeodeRobots,
        };

        // Do nothing
        yield return nextTime;

        // Build Ore Robot
        if (current.Ore >= blueprint.OreRobotOreCost)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.OreRobotOreCost,
                OreRobots = nextTime.OreRobots + 1
            };
        }
        
        // Build Clay Robot
        if (current.Ore >= blueprint.ClayRobotOreCost)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.ClayRobotOreCost,
                ClayRobots = nextTime.ClayRobots + 1
            };
        }
        
        // Build Obsidian Robot
        if (current.Ore >= blueprint.ObsidianRobotOreCost &&
            current.Clay >= blueprint.ObsidianRobotClayCost)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.OreRobotOreCost,
                Clay = nextTime.Clay - blueprint.ObsidianRobotClayCost,
                ObsidianRobots = nextTime.ObsidianRobots + 1
            };
        }
        
        // Build Geode Robot
        if (current.Ore >= blueprint.GeodeRobotOreCost &&
            current.Obsidian >= blueprint.GeodeRobotObsidianCost)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.GeodeRobotOreCost,
                Obsidian = nextTime.Clay - blueprint.GeodeRobotObsidianCost,
                GeodeRobots = nextTime.GeodeRobots + 1
            };
        }
    }

    [TestCase("data/19 - Sample.txt", 0, TestName = "Day 19 - Part 2 - Sample")]
    [TestCase("data/19 - Puzzle Input.txt", 0, TestName = "Day 19 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile);

        var answer = 0;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }
    
    private static IEnumerable<Blueprint> ParseInput(string input)
    {
        Blueprint ParseBlueprint(string line)
        {
            var regex = new Regex(
                "^Blueprint ([0-9]+): Each ore robot costs ([0-9]+) ore. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian.$");
            var groups = regex.Match(line).Groups;

            return new Blueprint(
                int.Parse(groups[1].Value),
        int.Parse(groups[2].Value),
        int.Parse(groups[3].Value),
        int.Parse(groups[4].Value),
        int.Parse(groups[5].Value),
        int.Parse(groups[6].Value),
        int.Parse(groups[7].Value));
        }

        return input.Split("\n").Select(ParseBlueprint);
    }

    private record State(
        int Time,
        int Ore,
        int Clay,
        int Obsidian,
        int Geodes,
        int OreRobots,
        int ClayRobots,
        int ObsidianRobots,
        int GeodeRobots);

    private record Blueprint(
        int Id,
        int OreRobotOreCost,
        int ClayRobotOreCost,
        int ObsidianRobotOreCost,
        int ObsidianRobotClayCost,
        int GeodeRobotOreCost,
        int GeodeRobotObsidianCost);
}