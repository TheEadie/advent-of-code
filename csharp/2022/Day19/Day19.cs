using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day19;

public class Day19
{
    private readonly AdventSession _session = new(2022, 19, "Not Enough Minerals");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 33)]
    [TestCase("Puzzle Input.txt", 1306)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var blueprints = ParseInput(input).ToList();

        var answer = blueprints
            .Select(x => (Blueprint: x, Max: FindNumberOfGeodes(x, 24).First()))
            .Select(x => x.Blueprint.Id * x.Max)
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 3472)]
    [TestCase("Puzzle Input.txt", 37604)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var blueprints = ParseInput(input).Take(3).ToList();

        var answer = blueprints
            .Select(x => FindNumberOfGeodes(x, 32).First())
            .Aggregate(1, (current, x) => current * x);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<int> FindNumberOfGeodes(Blueprint blueprint, int maxTime)
    {
        var statesToTry = new PriorityQueue<State, int>();
        var statesTried = new HashSet<State>();
        var cameFrom = new Dictionary<State, State>();

        var start = new State(0, 0, 0, 0, 0, 1, 0, 0, 0);
        statesToTry.Enqueue(start, GetPotential(start, maxTime, blueprint));

        while (statesToTry.Count > 0)
        {
            var current = statesToTry.Dequeue();
            if (statesTried.Contains(current))
            {
                continue;
            }

            _ = statesTried.Add(current);

            if (current.Time == maxTime)
            {
                //TestContext.Progress.WriteLine($"Blueprint {blueprint.Id}: {current.Geodes}");

                var totalPath = new List<State> { current };

                var track = current;
                while (cameFrom.ContainsKey(track))
                {
                    track = cameFrom[track];
                    totalPath = totalPath.Prepend(track).ToList();
                }

                //TestContext.Progress.WriteLine(string.Join(Environment.NewLine, totalPath));

                yield return current.Geodes;
            }

            foreach (var next in GetPossibleStates(current, blueprint))
            {
                var nextPotential = GetPotential(next, maxTime, blueprint);
                cameFrom[next] = current;
                statesToTry.Enqueue(next, -(next.Geodes + nextPotential));
            }
        }

        TestContext.Progress.WriteLine("Failed to find a path");
    }

    private static int GetPotential(State next, int maxTime, Blueprint blueprint)
    {
        var time = maxTime - next.Time;

        if (next.ClayRobots == 0)
        {
            time -= blueprint.FastestTimeToGetEnoughClay;
        }

        if (next.ObsidianRobots == 0)
        {
            time -= blueprint.FastestTimeToGetEnoughObsidian;
        }

        if (next.GeodeRobots == 0)
        {
            time -= 1;
        }

        return time < 0 ? 0 : time * (time + 1) / 2 + next.GeodeRobots * time;
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
        if (current.Ore >= blueprint.OreRobotOreCost &&
            current.OreRobots < blueprint.MaxOreNeededPerMin)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.OreRobotOreCost,
                OreRobots = nextTime.OreRobots + 1
            };
        }

        // Build Clay Robot
        if (current.Ore >= blueprint.ClayRobotOreCost &&
            current.ClayRobots < blueprint.ObsidianRobotClayCost)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.ClayRobotOreCost,
                ClayRobots = nextTime.ClayRobots + 1
            };
        }

        // Build Obsidian Robot
        if (current.Ore >= blueprint.ObsidianRobotOreCost &&
            current.Clay >= blueprint.ObsidianRobotClayCost &&
            current.ObsidianRobots < blueprint.GeodeRobotObsidianCost)
        {
            yield return nextTime with
            {
                Ore = nextTime.Ore - blueprint.ObsidianRobotOreCost,
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
                Obsidian = nextTime.Obsidian - blueprint.GeodeRobotObsidianCost,
                GeodeRobots = nextTime.GeodeRobots + 1
            };
        }
    }

    private static IEnumerable<Blueprint> ParseInput(string input)
    {
        static Blueprint ParseBlueprint(string line)
        {
            var regex = new Regex(
                "^Blueprint ([0-9]+): Each ore robot costs ([0-9]+) ore. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian.$");
            var groups = regex.Match(line).Groups;

            var id = int.Parse(groups[1].Value);
            var oreRobotOreCost = int.Parse(groups[2].Value);
            var clayRobotOreCost = int.Parse(groups[3].Value);
            var obsidianRobotOreCost = int.Parse(groups[4].Value);
            var obsidianRobotClayCost = int.Parse(groups[5].Value);
            var geodeRobotOreCost = int.Parse(groups[6].Value);
            var geodeRobotObsidianCost = int.Parse(groups[7].Value);
            var maxOreNeededPerMin = new List<int>() { oreRobotOreCost, clayRobotOreCost, obsidianRobotOreCost, geodeRobotOreCost }.Max();

            return new Blueprint(
                id,
                oreRobotOreCost,
                clayRobotOreCost,
                obsidianRobotOreCost,
                obsidianRobotClayCost,
                geodeRobotOreCost,
                geodeRobotObsidianCost,
                maxOreNeededPerMin,
                (int) Math.Floor(Math.Sqrt(2 * obsidianRobotClayCost)),
                (int) Math.Floor(Math.Sqrt(2 * geodeRobotObsidianCost)));
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
        int GeodeRobotObsidianCost,
        int MaxOreNeededPerMin,
        int FastestTimeToGetEnoughClay,
        int FastestTimeToGetEnoughObsidian);
}
