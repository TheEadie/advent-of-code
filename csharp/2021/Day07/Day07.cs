namespace AdventOfCode2021.Day07
{
    public class Day07
    {
        private readonly AdventSession _session = new(2021, 7, "The Treachery of Whales");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var numbers = ParseInput(input);

            var median = numbers.OrderBy(x => x).ElementAt(numbers.Count / 2);
            var fuelUsed = numbers.Sum(number => Math.Abs(number - median));

            _session.PrintAnswer(1, fuelUsed);
            fuelUsed.ShouldBe(323647);
        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var numbers = ParseInput(input);

            var mean = numbers.Sum() / (decimal) numbers.Count;
            var candidates = new[] {(int)Math.Floor(mean), (int)Math.Ceiling(mean)};

            var total = candidates.Min(x => numbers.Sum(number => GetFuelUse(Math.Abs(number - x))));

            _session.PrintAnswer(2, total);
            total.ShouldBe(87640209);
        }

        private static int GetFuelUse(int distance)
        {
            return distance * (distance + 1) / 2;
        }

        private static List<int> ParseInput(string input)
        {
            var lines = input.Split("\n");
            return lines[0].Split(",").Select(int.Parse).ToList();
        }
    }
}
