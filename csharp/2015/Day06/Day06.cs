namespace AdventOfCode2015.Day06;

public class Day06
{
    private readonly AdventSession _session = new(2015, 6, "Probably a Fire Hazard");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var commands = await ParseInput();
        var lights = new Dictionary<Coordinate, bool>();

        foreach (var command in commands)
        {
            switch (command.What)
            {
                case What.On:
                    for (var x = command.Start.X; x <= command.End.X; x++)
                    {
                        for (var y = command.Start.Y; y <= command.End.Y; y++)
                        {
                            lights[new Coordinate(x, y)] = true;
                        }
                    }
                    break;
                case What.Off:
                    for (var x = command.Start.X; x <= command.End.X; x++)
                    {
                        for (var y = command.Start.Y; y <= command.End.Y; y++)
                        {
                            lights[new Coordinate(x, y)] = false;
                        }
                    }
                    break;
                case What.Toggle:
                    for (var x = command.Start.X; x <= command.End.X; x++)
                    {
                        for (var y = command.Start.Y; y <= command.End.Y; y++)
                        {
                            var coordinate = new Coordinate(x, y);
                            lights[coordinate] = !lights.ContainsKey(coordinate) || !lights[coordinate];
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var answer = lights.Count(x => x.Value);
        Console.WriteLine(answer);
        answer.ShouldBe(377891);
    }

    [Test]
    public async Task Part2()
    {
        var commands = await ParseInput();
        var lights = new Dictionary<Coordinate, int>();

        foreach (var command in commands)
        {
            switch (command.What)
            {
                case What.On:
                    for (var x = command.Start.X; x <= command.End.X; x++)
                    {
                        for (var y = command.Start.Y; y <= command.End.Y; y++)
                        {
                            var coordinate = new Coordinate(x, y);
                            if (lights.ContainsKey(coordinate))
                            {
                                lights[coordinate] += 1;
                            }
                            else
                            {
                                lights[coordinate] = 1;
                            }
                        }
                    }
                    break;
                case What.Off:
                    for (var x = command.Start.X; x <= command.End.X; x++)
                    {
                        for (var y = command.Start.Y; y <= command.End.Y; y++)
                        {
                            var coordinate = new Coordinate(x, y);
                            if (lights.ContainsKey(coordinate) &&
                                lights[coordinate] > 0)
                            {
                                lights[coordinate] -= 1;
                            }
                            else
                            {
                                lights[coordinate] = 0;
                            }
                        }
                    }
                    break;
                case What.Toggle:
                    for (var x = command.Start.X; x <= command.End.X; x++)
                    {
                        for (var y = command.Start.Y; y <= command.End.Y; y++)
                        {
                            var coordinate = new Coordinate(x, y);
                            if (lights.ContainsKey(coordinate))
                            {
                                lights[coordinate] += 2;
                            }
                            else
                            {
                                lights[coordinate] = 2;
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var answer = lights.Sum(x => x.Value);
        Console.WriteLine(answer);
        answer.ShouldBe(14110788);
    }

    private async Task<IEnumerable<Command>> ParseInput()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var lines = input.Split("\n");

        return lines.Select(ParseCommand);
    }

    private static Command ParseCommand(string input)
    {
        var parts = input.Split(' ');
        if (parts[0] == "toggle")
        {
            return CreateCommand(What.Toggle, parts[1], parts[3]);
        }

        var what = parts[1] == "on" ? What.On : What.Off;
        return CreateCommand(what, parts[2], parts[4]);
    }

    private static Command CreateCommand(What what, string start, string end)
    {
        var startSplit = start.Split(',');
        var endSplit = end.Split(',');
        return new Command(what, new Coordinate(int.Parse(startSplit[0]), int.Parse(startSplit[1])),
            new Coordinate(int.Parse(endSplit[0]), int.Parse(endSplit[1])));
    }

    private record Command(What What, Coordinate Start, Coordinate End);

    private enum What { On, Off, Toggle };

    private record Coordinate(int X, int Y);
}
