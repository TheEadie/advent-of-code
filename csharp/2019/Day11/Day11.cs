namespace AdventOfCode2019.Day11;

public class Day11
{
    private readonly AdventSession _session = new(2019, 11, "Space Police");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var emulator = new IntCode.IntCode(program);
        var panels = new Dictionary<Coordinate, int>();
        RunRobot(panels, emulator);

        var answer = panels.Count;
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(1930);
    }


    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input.Split(',').Select(long.Parse).ToArray();

        var emulator = new IntCode.IntCode(program);
        var panels = new Dictionary<Coordinate, int> { { new Coordinate(0, 0), 1 } };

        RunRobot(panels, emulator);

        for (var y = 0; y > -6; y--)
        {
            for (var x = 0; x < 41; x++)
            {
                var panelColour = GetPanelColour(new Coordinate(x, y), panels);
                Console.Write(panelColour == 0 ? '.' : 'x');
            }

            Console.WriteLine();
        }

        const string answer = "PFKHECZU";
        _session.PrintAnswer(2, answer);
    }

    private void RunRobot(Dictionary<Coordinate, int> panels, IntCode.IntCode emulator)
    {
        var robot = new Robot();
        var inputColour = GetPanelColour(robot.Location, panels);
        var outputs = emulator.GetOutputs(inputColour, 2).ToArray();

        while (outputs.Any())
        {
            var outputColour = (int) outputs[0];
            var outputDirection = (int) outputs[1];
            UpdatePanelColour(robot.Location, panels, outputColour);
            robot.Move(outputDirection);
            inputColour = GetPanelColour(robot.Location, panels);
            outputs = emulator.GetOutputs(inputColour, 2).ToArray();
        }
    }

    private void UpdatePanelColour(Coordinate panel, Dictionary<Coordinate, int> map, int colour)
    {
        if (map.ContainsKey(panel))
        {
            map[panel] = colour;
            return;
        }

        map.Add(panel, colour);
    }

    private int GetPanelColour(Coordinate panel, Dictionary<Coordinate, int> map) =>
        map.TryGetValue(panel, out var value) ? value : 0;

    private record Coordinate(int X, int Y);

    private class Robot
    {
        public Coordinate Location { get; private set; } = new(0, 0);
        private int _direction = 0; // 0=UP, 1=Right, 2=Down, 3=Left

        public void Move(int input)
        {
            _direction += input == 0 ? -1 : 1;
            if (_direction < 0)
            {
                _direction += 4;
            }

            if (_direction > 3)
            {
                _direction -= 4;
            }

            Location = _direction switch
            {
                0 => Location with { Y = Location.Y + 1 },
                1 => Location with { X = Location.X + 1 },
                2 => Location with { Y = Location.Y - 1 },
                3 => Location with { X = Location.X - 1 },
                _ => Location
            };
        }
    }
}
