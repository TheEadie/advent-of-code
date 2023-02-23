namespace AdventOfCode2019.Day05
{
    public class Day05
    {
        private readonly AdventSession _session = new(2019, 5, "Sunny with a Chance of Asteroids");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var program = input
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            var emulator = new IntCode.IntCode(program);
            
            emulator.Inputs.Enqueue(1);

            await emulator.RunAsync(CancellationToken.None);

            foreach (var value in emulator.Output)
            {
                Console.WriteLine(value);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(16225258);
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
            
            emulator.Inputs.Enqueue(5);

            await emulator.RunAsync(CancellationToken.None);

            foreach (var value in emulator.Output)
            {
                Console.WriteLine(value);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(2808771);
        }
    }
}
