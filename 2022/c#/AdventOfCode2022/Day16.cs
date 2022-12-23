using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16
{
    [TestCase("data/16 - Sample.txt", 1651, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 1896, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();
        var answer = FindRoute(input, 50);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/16 - Sample.txt", 1707, TestName = "Part 2 - Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();
        var answer = 0;//FindRouteWith2(input, 5);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
        // 2557 - Too low
    }

    private int FindRoute(List<Valve> input, int branches)
    {
        var valves = input.ToDictionary(k => k.Id, v => v.FlowRate);
        valves.Add("",0);
        
        var statesToTry = new Queue<State>();
        var statesTried = new HashSet<State>();

        statesToTry.Enqueue(new State("AA", "", 0, 0));

        while (statesToTry.Any())
        {
            var current = statesToTry.Dequeue();
            if (statesTried.Contains(current))
                continue;

            statesTried.Add(current);
            
            var options = ScoreValves(input, current.CurrentRoom, 30 - current.Minute)
                .Where(x => !current.OpenValves.Contains(x.Key.Id))
                .OrderByDescending(x => x.Value.Item2)
                .Take(branches)
                .ToList();

            foreach (var option in options)
            {
                var nextRoom = option.Key.Id;
                var nextOpenValves = current.OpenValves + "," + option.Key.Id;
                var nextTime = current.Minute + option.Value.Item1;

                var flow = current.OpenValves.Split(",").Sum(x => valves[x]);
                var nextTotalFlow = current.TotalFlow + (flow * option.Value.Item1);

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
                    //Console.WriteLine(
                    //    $"Finished: {nextState.TotalFlow} {nextState.OpenValves}");
                }
            }
        }

        var lastState = statesTried.MaxBy(x => x.TotalFlow + ((30 - x.Minute) * x.OpenValves.Split(",").Sum(v => valves[v])));
        Console.WriteLine($"{lastState.TotalFlow} - {lastState.OpenValves}");
        return lastState.TotalFlow + ((30 - lastState.Minute) * lastState.OpenValves.Split(",").Sum(v => valves[v]));
    }

    private IDictionary<Valve, (int, double)> ScoreValves(IList<Valve> valves, string currentRoom, int timeLeft)
    {
        var found = new Dictionary<Valve, (int, double)>();

        foreach (var valve in valves.Where(x => x.FlowRate > 0))
        {
            var (distance, path) = PathFinding.AStar(currentRoom,
                valve.Id,
                node => valves.Where(x => x.Tunnels.Contains(node)).Select(x => x.Id),
                (_, _) => 1,
                (_, _) => 0);

            var opportunity = (double)(timeLeft - distance - 1) * valve.FlowRate;
            found.Add(valve, (distance + 1, opportunity));
        }

        return found;
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