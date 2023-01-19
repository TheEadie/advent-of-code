namespace AdventOfCode2021.Day11
{
    public class Day11
    {
        private readonly AdventSession _session = new(2021, 11, "Dumbo Octopus");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var answer = Run(ParseInput(input)).Take(100).Sum();
            _session.PrintAnswer(1, answer);
            answer.ShouldBe(1757);
        }
        
        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var answer = Run(ParseInput(input)).TakeWhile(x => x != 100).Count() + 1;
            _session.PrintAnswer(2, answer);
            answer.ShouldBe(422);
        }

        private static IEnumerable<int> Run(Dictionary<Coordinate, int> octopuses)
        {
            while (true)
            {
                var queue = new Queue<Coordinate>();
                var done = new List<Coordinate>();

                foreach (var pos in octopuses.Keys)
                {
                    octopuses[pos]++;
                    if (octopuses[pos] > 9)
                    {
                        queue.Enqueue(pos);
                    }
                }

                while (queue.Any())
                {
                    var pos = queue.Dequeue();
                    if (done.Contains(pos))
                        continue;

                    done.Add(pos);

                    foreach (var neighbour in GetNeighbours(pos, octopuses))
                    {
                        octopuses[neighbour]++;
                        if (octopuses[neighbour] > 9)
                        {
                            queue.Enqueue(neighbour);
                        }
                    }
                }

                foreach (var pos in done)
                {
                    octopuses[pos] = 0;
                }

                yield return done.Count;
            }
        }

        private static IEnumerable<Coordinate> GetNeighbours(Coordinate input, IReadOnlyDictionary<Coordinate, int> map)
        {
            var testCoordinates = new List<Coordinate>
            {
                new(input.X - 1, input.Y - 1),
                new(input.X - 1, input.Y),
                new(input.X - 1, input.Y + 1),
                new(input.X, input.Y - 1),
                new(input.X, input.Y + 1),
                new(input.X + 1, input.Y - 1),
                new(input.X + 1, input.Y),
                new(input.X + 1, input.Y + 1)
            };

            return testCoordinates.Where(map.ContainsKey);
        }

        private static Dictionary<Coordinate, int> ParseInput(string input)
        {
            var map = new Dictionary<Coordinate, int>();
            var lines = input.Split("\n");
            var rows = lines.Select(line => line.ToCharArray().Select(x => int.Parse(x.ToString())).ToList()).ToList();

            var sizeX = rows.Count;
            var sizeY = rows[0].Count();
            var heightMap = new int[sizeX, sizeY];

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
