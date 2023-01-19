namespace AdventOfCode2021.Day15
{
    public class Day15
    {
        private readonly AdventSession _session = new(2021, 15, "Chiton");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var riskPlane = ParseInput(input);

            var start = riskPlane.First().Key;
            var goal = riskPlane.Last().Key;

            var answer = Run(start, goal, riskPlane);
            _session.PrintAnswer(1, answer);
            answer.ShouldBe(373);
        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var riskPlane = RepeatMap(ParseInput(input));

            var start = riskPlane.First().Key;
            var goal = riskPlane.Last().Key;

            var answer = Run(start, goal, riskPlane);
            _session.PrintAnswer(2, answer);
            answer.ShouldBe(2868);
        }

        private int DistanceToGoal(Coordinate current, Coordinate goal)
        {
            return (goal.X - current.X) + (goal.Y - current.Y);
        }

        private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, IReadOnlyDictionary<Coordinate, int> map)
        {
            var testCoordinates = new List<Coordinate>
            {
                new(input.X - 1, input.Y),
                new(input.X, input.Y - 1),
                new(input.X, input.Y + 1),
                new(input.X + 1, input.Y)
            };

            return testCoordinates.Where(map.ContainsKey);
        }

        private int Run(Coordinate start, Coordinate goal, Dictionary<Coordinate, int> riskPlane)
        {
            var queue = new PriorityQueue<Coordinate, int>();

            var cameFrom = new Dictionary<Coordinate, Coordinate>();
            var costSoFar = new Dictionary<Coordinate, int>
            {
                [start] = 0
            };

            var totalPath = new List<Coordinate> { goal };

            queue.Enqueue(start, DistanceToGoal(start, goal));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == goal)
                {
                    while (cameFrom.ContainsKey(current))
                    {
                        current = cameFrom[current];
                        totalPath = totalPath.Prepend(current).ToList();
                    }

                    break;
                }

                foreach (var neighbour in GetNeighbours(current, riskPlane))
                {
                    var tentativeGScore = costSoFar[current] + riskPlane[neighbour];
                    if (!costSoFar.ContainsKey(neighbour) || tentativeGScore < costSoFar[neighbour])
                    {
                        cameFrom[neighbour] = current;
                        costSoFar[neighbour] = tentativeGScore;
                        queue.Enqueue(neighbour, tentativeGScore + DistanceToGoal(neighbour, goal));
                    }
                }
            }

            var answer = costSoFar[goal];
            return answer;
        }

        private static Dictionary<Coordinate, int> RepeatMap(Dictionary<Coordinate, int> input)
        {
            var riskPlane = new Dictionary<Coordinate, int>();

            var width = input.MaxBy(x => x.Key.X).Key.X + 1;
            var height = input.MaxBy(x => x.Key.Y).Key.Y + 1;

            foreach (var (key, value) in input)
            {
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        var cell = new Coordinate(key.X + (x * width), key.Y + (y * height));

                        var newRisk = (value + x + y);
                        if (newRisk > 9)
                        {
                            newRisk = (newRisk % 10) + 1;
                        }

                        riskPlane.Add(cell, newRisk);
                    }
                }
            }

            return riskPlane;
        }

        private static Dictionary<Coordinate, int> ParseInput(string input)
        {
            var map = new Dictionary<Coordinate, int>();
            var lines = input.Split("\n");
            var rows = lines.Select(line => line.ToCharArray().Select(x => int.Parse(x.ToString())).ToList()).ToList();

            var sizeX = rows.Count;
            var sizeY = rows[0].Count();

            for (var x = 0; x < sizeX; x++)
            {
                for (var y = 0; y < sizeY; y++)
                {
                    map.Add(new Coordinate(x, y), rows[x][y]);
                }
            }

            return map;
        }
    }
}
