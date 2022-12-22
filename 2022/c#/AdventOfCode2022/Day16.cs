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

    [TestCase("data/16 - Sample.txt", 1707, TestName = "Part 2 - Sample")]
    [TestCase("data/16 - Puzzle Input.txt", 0, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile)).ToList();
        var answer = FindRouteWith2(input, 5);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
        // 2557 - Too low
    }
    
    private int FindRouteWith2(List<Valve> input, int branches)
    {
        const int time = 26;
        
        var statesToTry = new HashSet<(State, State)>();
        var allStates = new HashSet<(State, State)>();
        var finished = new List<(State, State)>();

        statesToTry.Add((new State(input.Single(x => x.Id == "AA"), new List<Valve>(), 0, 0, 0),
            new State(input.Single(x => x.Id == "AA"), new List<Valve>(), 0, 0, 0)));

        while (statesToTry.Any())
        {
            var current = statesToTry.ElementAt(0);
            statesToTry.Remove(current);

            var nextTime = current.Item1.Minute + 1;
            var nextFlowOne = current.Item1.Flow + current.Item1.OpenValves.Sum(x => x.FlowRate);
            var nextFlowTwo = current.Item2.Flow + current.Item2.OpenValves.Sum(x => x.FlowRate);
            var optionsOne = ScoreValves(input, current.Item1.CurrentRoom, time - current.Item1.Minute)
                .Where(x => !current.Item1.OpenValves.Concat(current.Item2.OpenValves).Contains(x.Key))
                .OrderByDescending(x => x.Value.Item1)
                .Take(branches)
                .ToList();
            
            var optionsTwo = ScoreValves(input, current.Item2.CurrentRoom, time - current.Item2.Minute)
                .Where(x => !current.Item1.OpenValves.Concat(current.Item2.OpenValves).Contains(x.Key))
                .OrderByDescending(x => x.Value.Item1)
                .Take(branches)
                .ToList();

            var nextStatesOne = new List<State>();
            var nextStatesTwo = new List<State>();
            
            foreach (var option in optionsOne)
            {
                var nextRoom = current.Item1.CurrentRoom;
                var nextOpenValves = current.Item1.OpenValves.ToList();
                
                if (option.Key == current.Item1.CurrentRoom)
                {
                    nextOpenValves.Add(option.Key);
                }
                else
                {
                    nextRoom = option.Value.Item2.Skip(1).First();
                }

                nextStatesOne.Add(new State(nextRoom, nextOpenValves, nextTime, nextFlowOne, option.Value.Item1));
            }
            
            foreach (var option in optionsTwo)
            {
                var nextRoom = current.Item2.CurrentRoom;
                var nextOpenValves = current.Item2.OpenValves.ToList();
                
                if (option.Key == current.Item2.CurrentRoom)
                {
                    nextOpenValves.Add(option.Key);
                }
                else
                {
                    nextRoom = option.Value.Item2.Skip(1).First();
                }

                nextStatesTwo.Add(new State(nextRoom, nextOpenValves, nextTime, nextFlowTwo, option.Value.Item1));
            }

            foreach (var optionOne in nextStatesOne)
            {
                foreach (var optionTwo in nextStatesTwo)
                {
                    if (optionOne.CurrentRoom.Id == optionTwo.CurrentRoom.Id)
                        continue;
                    
                    if (nextTime < time)
                    {
                        if (statesToTry.Any(x => 
                                x.Item1.Minute == nextTime &&
                                x.Item1.CurrentRoom.Id == optionOne.CurrentRoom.Id &&
                                x.Item2.CurrentRoom.Id == optionTwo.CurrentRoom.Id))
                            continue;
                        
                        var otherOptions = statesToTry
                            .Where(x => x.Item1.Minute == nextTime)
                            .ToList();
                        
                        var topOptions = otherOptions
                            .Append((optionOne, optionTwo))
                            .OrderByDescending(x => (x.Item1.Flow + x.Item2.Flow + x.Item1.Potential + x.Item2.Potential))
                            .Take(branches);

                        foreach (var other in otherOptions)
                        {
                            statesToTry.Remove(other);
                            allStates.Remove(other);
                        }

                        foreach (var top in topOptions)
                        {
                            statesToTry.Add(top);
                            allStates.Add(top);
                        }
                    }
                    else
                    {
                        var totalFlow = optionOne.Flow + optionTwo.Flow;
                        var totalOpenValves = optionOne.OpenValves.Concat(optionTwo.OpenValves);
                        //Console.WriteLine(
                        //    $"Finished: {totalFlow} Me: {string.Join(", ", optionOne.Item1.OpenValves.Select(x => x.Id))}  " +
                        //    $"Elephant: {string.Join(", ", optionTwo.Item1.OpenValves.Select(x => x.Id))}");
                        finished.Add((optionOne,optionTwo));
                    }
                }
            }
        }

        foreach (var state in allStates.OrderBy(x => x.Item1.Minute))
        {
            Console.WriteLine($"Minute {state.Item1.Minute} " +
                              $"me: {state.Item1.CurrentRoom.Id}, " +
                              $"Elephant: {state.Item2.CurrentRoom.Id}, " +
                              $"Score: {state.Item1.Flow + state.Item2.Flow + state.Item1.Potential + state.Item2.Potential}");
        }

        return finished.Max(x => x.Item1.Flow + x.Item2.Flow);
    }

    private int FindRoute(List<Valve> input, int branches)
    {
        var statesToTry = new HashSet<State>();
        var finished = new List<State>();

        statesToTry.Add(new State(input.Single(x => x.Id == "AA"), new List<Valve>(), 0, 0, 0));

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

                var nextState = new State(nextRoom, nextOpenValves, nextTime, nextFlow, option.Value.Item1);

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
            found.Add(valve, (opportunity / (distance + 1), path));
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

    private record State(Valve CurrentRoom, IEnumerable<Valve> OpenValves, int Minute, int Flow, double Potential);

    private record Valve(string Id, int FlowRate, IEnumerable<string> Tunnels);
}