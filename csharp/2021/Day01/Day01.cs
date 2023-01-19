namespace AdventOfCode2021.Day01
{
    public class Day01
    {
        private readonly AdventSession _session = new(2021, 1, "Sonar Sweep");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var depths = ParseInput(input);

            var count = depths
                .Zip(depths.Skip(1), (prev, current) => current > prev)
                .Count(x => x);
            
            _session.PrintAnswer(1, count);
            count.ShouldBe(1292);
        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var depths = ParseInput(input);

            var slidingWindows = depths
                .Zip(depths.Skip(1), (first, second) => first + second)
                .Zip(depths.Skip(2), (sum, third) => sum + third)
                .ToArray();

            var count = slidingWindows
                .Zip(slidingWindows.Skip(1), (prev, current) => current > prev)
                .Count(x => x);

            _session.PrintAnswer(2, count);
            count.ShouldBe(1262);
        }

        private static int[] ParseInput(string input)
        {
            var lines = input.Split("\n");
            var depths = lines.Select(int.Parse).ToArray();
            return depths;
        }
    }
}