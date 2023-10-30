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
        var program = input
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        var emulator = new IntCode.IntCode(program);
        var emulatorTask = emulator.RunAsync(CancellationToken.None);

        var panels = new Dictionary<Coordinate, int>();
        var robot = new Robot();

        var cancellationSource = new CancellationTokenSource();
        var runRobotTask = Task.Run(() => RunRobot(cancellationSource.Token, robot, panels, emulator), cancellationSource.Token);

        await emulatorTask;
        cancellationSource.Cancel();

        var answer = panels.Count;
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(1930);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var program = input
            .Split(',')
            .Select(long.Parse)
            .ToArray();

        var emulator = new IntCode.IntCode(program);
        var emulatorTask = emulator.RunAsync(CancellationToken.None);

        var panels = new Dictionary<Coordinate, int> { { new Coordinate(0, 0), 1 } };
        var robot = new Robot();

        var cancellationSource = new CancellationTokenSource();
        var runRobotTask = Task.Run(() => RunRobot(cancellationSource.Token, robot, panels, emulator), cancellationSource.Token);

        await emulatorTask;
        cancellationSource.Cancel();

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

    private void RunRobot(CancellationToken cancellationToken, Robot robot, Dictionary<Coordinate, int> panels, IntCode.IntCode emulator)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var inputColour = GetPanelColour(robot.Location, panels);
            emulator.Inputs.Enqueue(inputColour);
            var outputColour = (int)emulator.WaitForOutput();
            var outputDirection = (int)emulator.WaitForOutput();
            UpdatePanelColour(robot.Location, panels, outputColour);
            robot.Move(outputDirection);
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

    private int GetPanelColour(Coordinate panel, Dictionary<Coordinate, int> map) => map.ContainsKey(panel) ? map[panel] : 0;

    private record Coordinate(int X, int Y);

    private class Robot
    {
        public Coordinate Location { get; private set; }
        private int _direction; // 0=UP, 1=Right, 2=Down, 3=Left

        public Robot()
        {
            Location = new Coordinate(0, 0);
            _direction = 0;
        }

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
