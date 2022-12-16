namespace AdventOfCode2022;

public class Day14
{
    [TestCase("data/14 - Sample.txt", 24, TestName = "Sample")]
    [TestCase("data/14 - Puzzle Input.txt", 858, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var cave = ParseInput(File.ReadAllText(inputFile));
        var start = new Coordinate(500, 0);
        var bottomOfCave = cave.Keys.Max(x => x.Y);
        
        var answer = DropSand(start, cave)
            .TakeWhile(x => x.Y < bottomOfCave)
            .Count();
        
        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/14 - Sample.txt", 93, TestName = "Part 2 - Sample")]
    [TestCase("data/14 - Puzzle Input.txt", 26845, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var cave = ParseInput(File.ReadAllText(inputFile));
        var start = new Coordinate(500, 0);
        
        var answer = DropSand(start, cave)
            .TakeUntil(x => x == start)
            .Count();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<Coordinate> DropSand(Coordinate start, IDictionary<Coordinate, Item> map)
    {
        var current = start;
        var maxDepth = map.Keys.Max(x => x.Y) + 1;

        while (true)
        {
            if (current.Y >= maxDepth)
            {
                yield return current;
                current = start;
            }
            
            var down = current with {Y = current.Y + 1};
            var downLeft = new Coordinate(current.X -1, current.Y + 1);
            var downRight = new Coordinate(current.X + 1, current.Y + 1);
            if (!map.ContainsKey(down))
            {
                map.Remove(current);
                map.Add(down, Item.Sand);
                current = down;
            }
            else if (!map.ContainsKey(downLeft))
            {
                map.Remove(current);
                map.Add(downLeft, Item.Sand);
                current = downLeft;
            }
            else if (!map.ContainsKey(downRight))
            {
                map.Remove(current);
                map.Add(downRight, Item.Sand);
                current = downRight;
            }
            else
            {
                yield return current;
                current = start;
            }
        }
    }

    private static IDictionary<Coordinate, Item> ParseInput(string input)
    {
        return input.Split("\n")
            .SelectMany(x =>
            {
                var coordinates = x.Split(" -> ")
                    .Select(s =>
                    {
                        var numbers = s.Split(",").Select(int.Parse).ToArray();
                        return new Coordinate(numbers[0], numbers[1]);
                    }).ToList();
                
                return coordinates
                    .Zip(coordinates.Skip(1))
                    .Select(c => new Line(c.First, c.Second));
            })
            .SelectMany(x => x.GetCoordinatesOnLine())
            .Distinct()
            .ToDictionary(x => x, _ => Item.Rock);
    }

    private enum Item
    {
        Rock,
        Sand
    }
}