using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16
{
    [TestCase("data/16 - Sample.txt", 1651, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 1896, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();

        var answer = FindRoute(input);

        Console.WriteLine(answer);
        answer.TotalFlow.ShouldBe(expected);
    }

    //[TestCase("data/16 - Sample.txt", 1707, TestName = "Part 2 - Sample")]
    //[TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();
        var answer = 0;//FindRouteWith2(input, 5);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
        // 2557 - Too low
    }

    private static IDictionary<string, Dictionary<Valve, int>> GetDistances(List<Valve> valves)
    {
        var distances = new Dictionary<string, Dictionary<Valve, int>>();

        foreach (var v in valves.Where(x => x.FlowRate > 0 || x.Id == "AA"))
        {
            var found = new Dictionary<Valve, int>();

            foreach (var valve in valves.Where(x => x.FlowRate > 0))
            {
                var (distance, path) = PathFinding.AStar(v.Id,
                    valve.Id,
                    node => valves.Where(x => x.Tunnels.Contains(node)).Select(x => x.Id),
                    (_, _) => 1,
                    (_, _) => 0);

                found.Add(valve, distance + 1);
            }

            distances.Add(v.Id, found);
        }

        return distances;
    }

    private static State FindRoute(List<Valve> input)
    {
        var flowRates = input.ToDictionary(k => k.Id, v => v.FlowRate);
        flowRates.Add("", 0);

        var distances = GetDistances(input);
        var neighbours = distances.ToDictionary(k => k.Key, v => v.Value.Keys);

        var allOpen = flowRates.Sum(x => x.Value);

        var statesToTry = new PriorityQueue<State, int>();
        var statesTried = new HashSet<State>();

        State? found = null;

        statesToTry.Enqueue(new State("AA", "", 0, 0), 30 * allOpen);

        while (statesToTry.Count > 0)
        {
            var current = statesToTry.Dequeue();
            statesTried.Add(current);

            var options = neighbours[current.CurrentRoom]
                .Where(x => !current.OpenValves.Contains(x.Id))
                .ToList();

            if (current.Minute >= 30 || options.Count == 0)
            {
                found = current;
                break;
            }

            var costs = distances[current.CurrentRoom];

            foreach (var option in options)
            {
                var nextRoom = option.Id;
                var nextOpenValves = current.OpenValves + "," + option.Id;
                var nextTime = current.Minute + costs[option];

                var flow = current.OpenValves.Split(",").Sum(x => flowRates[x]);
                var nextTotalFlow = current.TotalFlow + (flow * costs[option]);
                var nextPotential = (30 - nextTime) * allOpen;

                var nextState = new State(nextRoom, nextOpenValves, nextTime, nextTotalFlow);

                if (!statesTried.Contains(nextState))
                {
                    statesToTry.Enqueue(nextState, -(nextTotalFlow + nextPotential));
                }
            }
        }

        return found! with
        {
            Minute = 30,
            TotalFlow = found.TotalFlow + ((30 - found.Minute) * found.OpenValves.Split(",").Sum(x => flowRates[x]))
        };
    }

    private IEnumerable<Valve> ParseInput(string input)
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