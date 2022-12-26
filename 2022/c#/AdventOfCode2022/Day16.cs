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
        answer.ShouldBe(expected);
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

    private static int FindRoute(List<Valve> input)
    {
        var flowRates = input.ToDictionary(k => k.Id, v => v.FlowRate);
        flowRates.Add("", 0);

        var distances = GetDistances(input);
        var neighbours = distances.ToDictionary(k => k.Key, v => v.Value.Keys);

        var statesToTry = new Queue<State>();
        var statesTried = new HashSet<State>();

        statesToTry.Enqueue(new State("AA", "", 0, 0));

        while (statesToTry.Any())
        {
            var current = statesToTry.Dequeue();

            statesTried.Add(current);

            var costs = distances[current.CurrentRoom];

            var options = neighbours[current.CurrentRoom]
                .Where(x => !current.OpenValves.Contains(x.Id))
                .ToList();

            foreach (var option in options)
            {
                var nextRoom = option.Id;
                var nextOpenValves = current.OpenValves + "," + option.Id;
                var nextTime = current.Minute + costs[option];

                var flow = current.OpenValves.Split(",").Sum(x => flowRates[x]);
                var nextTotalFlow = current.TotalFlow + (flow * costs[option]);

                var nextState = new State(nextRoom, nextOpenValves, nextTime, nextTotalFlow);

                if (nextTime < 30)
                {
                    if (!statesTried.Contains(nextState))
                    {
                        statesToTry.Enqueue(nextState);
                    }
                }
                else
                {
                    statesTried.Add(nextState with
                    {
                        Minute = 30,
                        TotalFlow = current.TotalFlow + ((30 - current.Minute) * flow)
                    });
                }
            }
        }

        var finishedStates = statesTried
            .Select(x => x with
            {
                Minute = 30,
                TotalFlow = x.TotalFlow + (Math.Max(30 - x.Minute, 0) * x.OpenValves.Split(",").Sum(x => flowRates[x]))
            });

        var lastState = finishedStates.MaxBy(x => x.TotalFlow);
        Console.WriteLine($"{lastState}");
        return lastState!.TotalFlow;
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