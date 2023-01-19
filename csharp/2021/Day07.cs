namespace AdventOfCode2021
{
    public class Day07
    {
        [Test]
        public void Part1()
        {
            var numbers = ParseInput();

            var median = numbers.OrderBy(x => x).ElementAt(numbers.Count / 2);
            var fuelUsed = numbers.Sum(number => Math.Abs(number - median));

            Console.WriteLine(fuelUsed);
            fuelUsed.ShouldBe(323647);
        }

        [Test]
        public void Part2()
        {
            var numbers = ParseInput();

            var mean = numbers.Sum() / (decimal) numbers.Count;
            var candidates = new[] {(int)Math.Floor(mean), (int)Math.Ceiling(mean)};

            var total = candidates.Min(x => numbers.Sum(number => GetFuelUse(Math.Abs(number - x))));

            Console.WriteLine(total);
            total.ShouldBe(87640209);
        }

        private static int GetFuelUse(int distance)
        {
            return distance * (distance + 1) / 2;
        }

        private static List<int> ParseInput()
        {
            var lines = File.ReadAllLines("Day07.txt");
            return lines[0].Split(",").Select(int.Parse).ToList();
        }
    }
}
