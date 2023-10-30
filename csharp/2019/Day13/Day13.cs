using System.Collections.Concurrent;

namespace AdventOfCode2019.Day13;

public class Day13
{
    private readonly AdventSession _session = new(2019, 13, "Care Package");

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
        var screen = new Screen(emulator.Output);

        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        var emulatorTask = emulator.RunAsync(token);
        var screenTask = screen.RunAsync(token);

        await emulatorTask;

        while (!emulator.Output.IsEmpty)
        {
            // wait to process everything
        }

        cancellationTokenSource.Cancel();

        var answer = screen.Tiles.Count(x => x.Value == 2);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(329);
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
        emulator.Memory[0] = 2;

        var screen = new Screen(emulator.Output);

        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        var emulatorTask = emulator.RunAsync(token);
        var screenTask = screen.RunAsync(token);

        var aiTask = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                var ballTile = screen.Tiles.Values.SingleOrDefault(x => x == 4);
                if (ballTile == 0)
                {
                    continue;
                }

                var ballXPosition = screen.Tiles.Single(x => x.Value == 4).Key.X;

                var paddleTile = screen.Tiles.Values.SingleOrDefault(x => x == 3);
                if (paddleTile == 0)
                {
                    continue;
                }

                var paddleXPosition = screen.Tiles.Single(x => x.Value == 3).Key.X;

                var distance = ballXPosition - paddleXPosition;
                var move = distance > 0 ? 1 : distance < 0 ? -1 : 0;
                var peek = emulator.Inputs.TryPeek(out var currentTop);

                if (peek && currentTop == move)
                {
                    continue;
                }

                emulator.Inputs.Enqueue(move);

                await Task.Delay(1, token);
            }
        }, token);

        await emulatorTask;

        while (!emulator.Output.IsEmpty)
        {
            // wait to process everything
        }

        cancellationTokenSource.Cancel();

        var answer = screen.SegmentDisplay;
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(15973);
    }

    private class Screen
    {
        private readonly ConcurrentQueue<long> _videoMemory;
        public ConcurrentDictionary<Coordinate, long> Tiles { get; }
        public long SegmentDisplay { get; private set; }

        public Screen(ConcurrentQueue<long> videoMemory)
        {
            _videoMemory = videoMemory;
            Tiles = new ConcurrentDictionary<Coordinate, long>();
            SegmentDisplay = 0;
        }

        public Task RunAsync(CancellationToken cancellationToken) =>
            Task.Run(() => Run(cancellationToken), cancellationToken);

        private void Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_videoMemory.Count < 3)
                {
                    // wait for more input
                }

                _ = _videoMemory.TryDequeue(out var x);
                _ = _videoMemory.TryDequeue(out var y);
                _ = _videoMemory.TryDequeue(out var value);

                if (x == -1 && y == 0)
                {
                    SegmentDisplay = value;
                }

                var position = new Coordinate((int) x, (int) y);
                if (Tiles.ContainsKey(position))
                {
                    Tiles[position] = value;
                }
                else
                {
                    _ = Tiles.TryAdd(position, value);
                }
            }
        }

        public void Draw()
        {
            for (var y = 0; y < Tiles.Keys.Max(x => x.Y); y++)
            {
                for (var x = 0; x < Tiles.Keys.Max(x => x.X); x++)
                {
                    Console.Write(Tiles[new Coordinate(x, y)]);
                }
                Console.WriteLine();
            }
        }

        public record Coordinate(int X, int Y);
    }
}
