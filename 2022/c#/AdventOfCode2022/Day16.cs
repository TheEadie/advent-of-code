using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16
{
    [TestCase("data/16 - Sample.txt", 1651, TestName = "Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 1896, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();
        var answer = FindRoute(input, 2);

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

    private int FindRoute(List<Valve> input, int branches)
    {
        var statesToTry = new HashSet<State>();
        var finished = new List<State>();

        statesToTry.Add(new State(input.Single(x => x.Id == "AA"), new List<Valve>(), 0, 0));

        while (statesToTry.Any())
        {
            var current = statesToTry.ElementAt(0);
            statesToTry.Remove(current);

            var nextTime = current.Minute + 1;
            var nextFlow = current.Flow + current.OpenValves.Sum(x => x.FlowRate);
            var options = ScoreValves(input, current.CurrentRoom, 30 - current.Minute)
                .Where(x => !current.OpenValves.Contains(x.Key))
                .OrderByDescending(x => x.Value.Item1)
                .Take(branches)
                .ToList();

            foreach (var option in options)
            {
                var nextRoom = current.CurrentRoom;
                var nextOpenValves = current.OpenValves.ToList();
                
                if (option.Key == current.CurrentRoom)
                {
                    nextOpenValves.Add(option.Key);
                }
                else
                {
                    nextRoom = option.Value.Item2.Skip(1).First();
                }

                var nextState = new State(nextRoom, nextOpenValves, nextTime, nextFlow);

                if (nextTime < 30)
                {
                    var otherOptions = statesToTry
                        .Where(x => x.Minute == nextTime)
                        .ToList();
                        
                    var topOptions = otherOptions
                        .Append(nextState)
                        .OrderByDescending(x => x.Flow)
                        .Take(branches);

                    foreach (var other in otherOptions)
                    {
                        statesToTry.Remove(other);
                    }

                    foreach (var top in topOptions)
                    {
                        statesToTry.Add(top);
                    }
                }
                else
                {
                    Console.WriteLine(
                        $"Finished: {nextState.Flow} {string.Join(", ", nextState.OpenValves.Select(x => x.Id))}");
                    finished.Add(nextState);
                }
            }
        }

        return finished.Max(x => x.Flow);
    }

    private IDictionary<Valve, (double, IEnumerable<Valve>)> ScoreValves(IList<Valve> valves, Valve currentRoom, int timeLeft)
    {
        var found = new Dictionary<Valve, (double, IEnumerable<Valve>)>();

        foreach (var valve in valves)
        {
            var (distance, path) = PathFinding.AStar(currentRoom,
                valve,
                node => valves.Where(x => x.Tunnels.Contains(node.Id)),
                (_, _) => 1,
                (_, _) => 0);

            var opportunity = (double)(timeLeft - distance - 1) * valve.FlowRate;
            found.Add(valve, (opportunity / ((distance + 1)), path));
        }

        return found;
    }

    private IEnumerable<Valve> ParseInput(string input)
    {
        Valve ParseValve (string line)
        {
            var regex = new Regex(
                "^Valve (\\S*) has flow rate=([0-9]+); tunnel[s]? lead[s]? to valve[s]? ([\\S ]*)$");
            var groups = regex.Match(line).Groups;

            return new Valve(groups[1].Value, int.Parse(groups[2].Value), groups[3].Value.Split(", "));
        }
        
        return input.Split("\n").Select(ParseValve);
        
    }

    private record State(Valve CurrentRoom, IEnumerable<Valve> OpenValves, int Minute, int Flow);

    private record Valve(string Id, int FlowRate, IEnumerable<string> Tunnels);
}