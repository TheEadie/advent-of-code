namespace AdventOfCode2015.Day07;

public class Day07
{
    private readonly AdventSession _session = new(2015, 7, "Some Assembly Required");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var commands = await ParseInput();

        var answer = (int) commands["a"].Invoke();
        Console.WriteLine(answer);
        answer.ShouldBe(956);
    }

    [Test]
    public async Task Part2()
    {
        var commands = await ParseInput();
        var a = commands["a"].Invoke();

        var resetCommands = await ParseInput();
        resetCommands["b"] = () => a;

        var answer = (int) resetCommands["a"].Invoke();
        Console.WriteLine(answer);
        answer.ShouldBe(40149);
    }

    private async Task<IDictionary<string, Func<ushort>>> ParseInput()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var lines = input.Split("\n");
        var commands = new Dictionary<string, Func<ushort>>();

        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            switch (parts[1])
            {
                case "->":
                    commands.Add(parts[2], () =>
                    {
                        var value = GetNumber(parts[0], commands);
                        commands[parts[2]] = () => value;
                        return value;
                    });
                    break;
                case "AND":
                    commands.Add(parts[4], () =>
                    {
                        var value = (ushort) (GetNumber(parts[0], commands) & GetNumber(parts[2], commands));
                        commands[parts[4]] = () => value;
                        return value;
                    });
                    break;
                case "OR":
                    commands.Add(parts[4], () =>
                    {
                        var value = (ushort) (GetNumber(parts[0], commands) | GetNumber(parts[2], commands));
                        commands[parts[4]] = () => value;
                        return value;
                    });
                    break;
                case "LSHIFT":
                    commands.Add(parts[4], () =>
                    {
                        var value = (ushort) (GetNumber(parts[0], commands) << int.Parse(parts[2]));
                        commands[parts[4]] = () => value;
                        return value;
                    });
                    break;
                case "RSHIFT":
                    commands.Add(parts[4], () =>
                    {
                        var value = (ushort) (GetNumber(parts[0], commands) >> int.Parse(parts[2]));
                        commands[parts[4]] = () => value;
                        return value;
                    });
                    break;
                default:
                    break;
            }

            if (parts[0] == "NOT")
            {
                commands.Add(parts[3], () =>
                {
                    var value = (ushort) ~GetNumber(parts[1], commands);
                    commands[parts[3]] = () => value;
                    return value;
                });
            }
        }

        return commands;
    }

    private static ushort GetNumber(string text, IReadOnlyDictionary<string, Func<ushort>> commands)
    {
        var number = ushort.TryParse(text, out var value);
        return number ? value : commands[text].Invoke();
    }
}
