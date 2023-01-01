namespace AdventOfCode2022;

public class Day17
{
    [TestCase("data/17 - Sample.txt", 3068, TestName = "Day 17 - Part 1 - Sample")]
    [TestCase("data/17 - Puzzle Input.txt", 3193, TestName = "Day 17 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = File.ReadAllText(inputFile)
            .ToCharArray()
            .Select(x => x == '<' ? Direction.Left : Direction.Right);

        var answer = Drop(input, 2022);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/17 - Sample.txt", 1_514_285_714_288, TestName = "Day 17 - Part 2 - Sample")]
    [TestCase("data/17 - Puzzle Input.txt", 1_577_650_429_835, TestName = "Day 17 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, long expected)
    {
        var input = File.ReadAllText(inputFile)
            .ToCharArray()
            .Select(x => x == '<' ? Direction.Left : Direction.Right);

        var answer = Drop(input, 1_000_000_000_000);

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static long Drop(IEnumerable<Direction> input, long numberToDrop)
    {
        static bool CheckCollision(IEnumerable<Coordinate> block, IEnumerable<Coordinate> map)
        {
            foreach (var position in block)
            {
                if (position.X < 0 || position.X > 6 || map.Contains(position))
                    return true;
            }

            return false;
        }

        var blocks = new List<List<Coordinate>>()
        {
            new List<Coordinate> { new Coordinate(0, 0),
                                    new Coordinate(1, 0),
                                    new Coordinate(2, 0),
                                    new Coordinate(3, 0) },
            new List<Coordinate> { new Coordinate(1, 0),
                                    new Coordinate(0, 1),
                                    new Coordinate(1, 1),
                                    new Coordinate(2, 1),
                                    new Coordinate(1, 2) },
            new List<Coordinate> { new Coordinate(0, 0),
                                    new Coordinate(1, 0),
                                    new Coordinate(2, 0),
                                    new Coordinate(2, 1),
                                    new Coordinate(2, 2) },
            new List<Coordinate> { new Coordinate(0, 0),
                                    new Coordinate(0, 1),
                                    new Coordinate(0, 2),
                                    new Coordinate(0, 3) },
            new List<Coordinate> { new Coordinate(0, 0),
                                    new Coordinate(0, 1),
                                    new Coordinate(1, 0),
                                    new Coordinate(1, 1) }
        };

        var nextBlock = 0;
        var map = new List<Coordinate>
        {
            new Coordinate(0, 0),
            new Coordinate(1, 0),
            new Coordinate(2, 0),
            new Coordinate(3, 0),
            new Coordinate(4, 0),
            new Coordinate(5, 0),
            new Coordinate(6, 0)
        };

        var currentBlock = blocks[0].Select(x => new Coordinate(x.X + 2, x.Y + 4));
        var lines = new Dictionary<(string, string), int>();

        var howManyBlocks = 0;
        var blocksBeforeRepeatingSection = 0;
        var heightDiffs = new List<long>();
        long lastHeight = 0;

        while (true)
        {
            var currentY = map.Max(x => x.Y);
            var line = map.Where(x => x.Y >= currentY - 10).Select(x => new Coordinate(x.X, currentY - x.Y));
            var lineString = new string(Enumerable.Range(0, 70)
                .Select(x => line.Contains(new Coordinate(x % 7, x / 7)) ? '#' : '.')
                .ToArray());
            var block = currentBlock.Select(x => new Coordinate(x.X, currentY + 4 - x.Y));
            var blockString = new string(Enumerable.Range(0, 70)
                .Select(x => block.Contains(new Coordinate(x % 7, x / 7)) ? '#' : '.')
                .ToArray());

            if (lines.TryGetValue((blockString, lineString), out var value))
            {
                //TestContext.Progress.WriteLine($"Found! {blockString} - {lineString}");
                blocksBeforeRepeatingSection = value;
                break;
            }
            else
            {
                //TestContext.Progress.WriteLine($"{blockString} - {lineString}");
                lines.Add((blockString, lineString), howManyBlocks);
            }

            foreach (var direction in input)
            {
                var potentialBlockX = currentBlock;
                switch (direction)
                {
                    case Direction.Left:
                        potentialBlockX = potentialBlockX.Select(x => new Coordinate(x.X - 1, x.Y));
                        break;
                    case Direction.Right:
                        potentialBlockX = potentialBlockX.Select(x => new Coordinate(x.X + 1, x.Y));
                        break;
                }

                if (CheckCollision(potentialBlockX, map))
                {
                    potentialBlockX = currentBlock;
                }

                var potentialBlockY = potentialBlockX.Select(x => new Coordinate(x.X, x.Y - 1));

                if (CheckCollision(potentialBlockY, map))
                {
                    map.AddRange(potentialBlockX);
                    nextBlock = (nextBlock + 1) % blocks.Count;
                    var height = map.Max(x => x.Y);
                    currentBlock = blocks[nextBlock].ConvertAll(x => new Coordinate(x.X + 2, x.Y + height + 4));
                    howManyBlocks++;

                    if (howManyBlocks == numberToDrop)
                    {
                        return height;
                    }

                    heightDiffs.Add(height - lastHeight);
                    lastHeight = height;
                }
                else
                {
                    currentBlock = potentialBlockY;
                }
            }
        }

        var lookup = heightDiffs.ToArray()[blocksBeforeRepeatingSection..];
        var stillToGo = numberToDrop - blocksBeforeRepeatingSection;
        var noOfBlocks = stillToGo / lookup.Length;
        stillToGo -= noOfBlocks * lookup.Length;

        return lastHeight + ((noOfBlocks - 1) * lookup.Sum(x => x)) + lookup.Take((int)stillToGo).Sum(x => x);
    }

    private enum Direction { Left, Right }
    private record Coordinate(long X, long Y);
}