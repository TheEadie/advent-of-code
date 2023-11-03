namespace AdventOfCode2019.Day15;

public class Day15
{
    private readonly AdventSession _session = new(2019, 15, "Oxygen System");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Puzzle Input.txt", 258)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();
        var emulator = new IntCode.IntCode(program);

        var map = GenerateMap(emulator);
        var oxygenSystem = map.Single(x => x.Value == State.OxygenSystem).Key;

        var (answer, _) = PathFinding.AStar(
            new Coordinate(0, 0),
            x => x == oxygenSystem,
            x => GetNeighbours(x, map),
            (_, _) => 1,
            c => 0);

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Puzzle Input.txt", 372)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var program = input.Split(',').Select(long.Parse).ToArray();
        var emulator = new IntCode.IntCode(program);

        var map = GenerateMap(emulator);
        var oxygenSystem = map.Single(x => x.Value == State.OxygenSystem).Key;

        var queue = new Queue<Coordinate>();
        queue.Enqueue(oxygenSystem);
        var visited = new HashSet<Coordinate>();
        var distance = new Dictionary<Coordinate, int> { { oxygenSystem, 0 } };

        while (queue.Any())
        {
            var coordinate = queue.Dequeue();
            _ = visited.Add(coordinate);

            var toCheck = GetNeighbours(coordinate, map);
            foreach (var neighbour in toCheck)
            {
                if (visited.Contains(neighbour))
                {
                    continue;
                }

                queue.Enqueue(neighbour);
                distance[neighbour] = distance[coordinate] + 1;
            }
        }

        var answer = distance.Values.Max();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static Dictionary<Coordinate, State> GenerateMap(IntCode.IntCode emulator)
    {
        var map = new Dictionary<Coordinate, (State, IntCode.IntCode)>();

        var currentCoordinate = new Coordinate(0, 0);
        map[currentCoordinate] = (State.Floor, emulator);

        var queue = new Queue<Coordinate>();
        queue.Enqueue(currentCoordinate);
        var visited = new HashSet<Coordinate>();

        while (queue.Any())
        {
            var coordinate = queue.Dequeue();
            _ = visited.Add(coordinate);

            var testCoordinates = new List<(Coordinate, int)>
            {
                (coordinate with { Y = coordinate.Y - 1 }, 1),
                (coordinate with { Y = coordinate.Y + 1 }, 2),
                (coordinate with { X = coordinate.X - 1 }, 3),
                (coordinate with { X = coordinate.X + 1 }, 4)
            };

            emulator = map[coordinate].Item2;

            foreach (var (pos, i) in testCoordinates)
            {
                if (visited.Contains(pos))
                {
                    continue;
                }

                var nextEmulator = emulator.Clone();
                var (_, results) = nextEmulator.Run(i);
                var result = results.Single();
                switch (result)
                {
                    case 0:
                        map[pos] = (State.Wall, nextEmulator);
                        continue;
                    case 1:
                        map[pos] = (State.Floor, nextEmulator);
                        queue.Enqueue(pos);
                        break;
                    case 2:
                        map[pos] = (State.OxygenSystem, nextEmulator);
                        queue.Enqueue(pos);
                        break;
                    default:
                        break;
                }
            }
        }

        return map.ToDictionary(x => x.Key, x => x.Value.Item1);
    }

    private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, IDictionary<Coordinate, State> map)
    {
        var testCoordinates = new List<Coordinate>
        {
            input with { Y = input.Y - 1 },
            input with { Y = input.Y + 1 },
            input with { X = input.X - 1 },
            input with { X = input.X + 1 }
        };

        return testCoordinates.Where(x => map[x] != State.Wall);
    }

    private enum State
    {
        Floor,
        Wall,
        OxygenSystem
    }
}
