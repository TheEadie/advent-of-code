using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day16;

public class Day16
{
    [TestCase("Day16/Sample.txt", 1651, TestName = "Day 16 - Part 1 - Sample")]
    [TestCase("Day16/Puzzle Input.txt", 1896, TestName = "Day 16 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var answer = FindRoute(input, 30).First();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.TotalFlow.ShouldBe(expected);
    }

    [TestCase("Day16/Sample.txt", 1707, TestName = "Day 16 - Part 2 - Sample")]
    [TestCase("Day16/Puzzle Input.txt", 2576, TestName = "Day 16 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var isSample = inputFile.Contains("Sample");
        var allRoutes = FindRoute(input, 26).ToList();

        var routes = allRoutes.ConvertAll(x => new
        {
            State = x,
            Valves = x.OpenValves.Split(",").Skip(1).ToHashSet()
        });

        var best = (routes.Last().State, routes.Last().State);

        foreach (var myRoute in routes)
        {
            foreach (var elephantRoute in routes)
            {
                if (!myRoute.Valves.Overlaps(elephantRoute.Valves))
                {
                    if (myRoute.State.TotalFlow + elephantRoute.State.TotalFlow > best.Item1.TotalFlow + best.Item2.TotalFlow)
                        best = (myRoute.State, elephantRoute.State);
                    break;
                }
            }
        }

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {best}");
        var answer = best.Item1.TotalFlow + best.Item2.TotalFlow;
        answer.ShouldBe(expected);
    }

    private static IDictionary<string, Dictionary<Valve, int>> GetDistances(List<Valve> valves)
    {
        var distances = new Dictionary<string, Dictionary<Valve, int>>();

        foreach (var v in valves.Where(x => x.FlowRate > 0 || x.Id == "AA"))
        {
            var found = new Dictionary<Valve, int>();

            foreach (var valve in valves.Where(x => x.FlowRate > 0))
            {
                var (distance, path) = PathFinding.AStar(
                    v.Id,
                    n => n == valve.Id,
                    node => valves.Where(x => x.Tunnels.Contains(node)).Select(x => x.Id),
                    (_, _) => 1,
                    _ => 0);

                found.Add(valve, distance + 1);
            }

            distances.Add(v.Id, found);
        }

        return distances;
    }

    private static IEnumerable<State> FindRoute(List<Valve> input, int maxTime)
    {
        var flowRates = input.ToDictionary(k => k.Id, v => v.FlowRate);
        flowRates.Add("", 0);

        var distances = GetDistances(input);
        var neighbours = distances.ToDictionary(k => k.Key, v => v.Value.Keys);

        var allOpen = flowRates.Sum(x => x.Value);

        var statesToTry = new PriorityQueue<State, int>();
        var statesTried = new HashSet<State>();

        var found = new List<State>();

        statesToTry.Enqueue(new State("AA", "", 0, 0), maxTime * allOpen);

        while (statesToTry.Count > 0)
        {
            var current = statesToTry.Dequeue();
            statesTried.Add(current);

            var options = neighbours[current.CurrentRoom]
                .Where(x => !current.OpenValves.Contains(x.Id))
                .ToList();

            if (current.Minute == maxTime)
            {
                yield return current;
                continue;
            }

            var costs = distances[current.CurrentRoom];

            var nextStates = new List<State>();

            foreach (var option in options)
            {
                var nextRoom = option.Id;
                var nextOpenValves = current.OpenValves + "," + option.Id;
                var nextTime = current.Minute + costs[option];

                if (nextTime > maxTime)
                    continue;

                var flow = current.OpenValves.Split(",").Sum(x => flowRates[x]);
                var nextTotalFlow = current.TotalFlow + (flow * costs[option]);

                nextStates.Add(new State(nextRoom, nextOpenValves, nextTime, nextTotalFlow));
            }

            nextStates.Add(current with
            {
                Minute = maxTime,
                TotalFlow = current.TotalFlow +
                    ((maxTime - current.Minute) * current.OpenValves.Split(",").Sum(x => flowRates[x]))
            });

            foreach (var nextState in nextStates)
            {
                if (!statesTried.Contains(nextState))
                {
                    var nextPotential = (maxTime - nextState.Minute) * allOpen;
                    statesToTry.Enqueue(nextState, -(nextState.TotalFlow + nextPotential));
                }
            }
        }
    }

    private static IEnumerable<Valve> ParseInput(string input)
    {
        Valve ParseValve(string line)
        {
            var regex = new Regex(
                "^Valve (\\S*) has flow rate=([0-9]+); tunnel[s]? lead[s]? to valve[s]? ([\\S ]*)$");
            var groups = regex.Match(line).Groups;

            return new Valve(groups[1].Value, int.Parse(groups[2].Value), groups[3].Value.Split(", "));
        }

        return input.Split("\n").Select(ParseValve);
    }

    private record State(
        string CurrentRoom,
        string OpenValves,
        int Minute,
        int TotalFlow);

    private record Valve(string Id, int FlowRate, IEnumerable<string> Tunnels);
}