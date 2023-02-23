﻿namespace AdventOfCode2019.Day09
{
    public class Day09
    {
        private readonly AdventSession _session = new(2019, 9, "Sensor Boost");

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

            foreach (var output in emulator.Output)
            {
                Console.WriteLine(output);
            }

            var answer = emulator.Output.Last();
            answer.ShouldBe(2941952859);
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
            emulator.Inputs.Enqueue(2);

            await emulator.RunAsync(CancellationToken.None);

            foreach (var output in emulator.Output)
            {
                Console.WriteLine(output);
            }
 
            var answer = emulator.Output.Last();
            answer.ShouldBe(66113);
        }
    }
}