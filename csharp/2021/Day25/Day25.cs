namespace AdventOfCode2021.Day25;

public class Day25
{
    private readonly AdventSession _session = new(2021, 25, "Sea Cucumber");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var previous = new SeaFloor(0, 0);
        var current = ParseInput(input);

        var answer = 0;

        while (!previous.Equals(current))
        {
            previous = current;
            current = MoveEast(current);
            current = MoveSouth(current);
            answer++;
        }

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(504);
    }

    private static SeaFloor ParseInput(string input)
    {
        var lines = input.Split("\n");
        var rows = lines.Select(line => line.ToCharArray().ToList()).ToList();

        var sizeX = rows.Count;
        var sizeY = rows[0].Count();

        var seaFloor = new SeaFloor(sizeY, sizeX);

        for (var x = 0; x < sizeX; x++)
        {
            for (var y = 0; y < sizeY; y++)
            {
                SeaCucumber? cucumber = rows[x][y] switch
                {
                    '>' => SeaCucumber.East,
                    'v' => SeaCucumber.South,
                    _ => null
                };

                seaFloor.AddCucumber(cucumber, x, y);
            }
        }

        return seaFloor;
    }

    private static SeaFloor MoveEast(SeaFloor previous)
    {
        var width = previous.Width;
        var height = previous.Height;

        var next = new SeaFloor(width, height);

        for (var x = 0; x < height; x++)
        {
            for (var y = 0; y < width; y++)
            {
                var seaCucumber = previous.GetSeaCucumber(x, y);

                switch (seaCucumber)
                {
                    case SeaCucumber.East:
                        if (previous.GetSeaCucumber(x, y + 1) == null)
                        {
                            next.AddCucumber(SeaCucumber.East, x, y + 1);
                        }
                        else
                        {
                            next.AddCucumber(SeaCucumber.East, x, y);
                        }
                        break;
                    case SeaCucumber.South:
                        next.AddCucumber(SeaCucumber.South, x, y);
                        break;
                    default:
                        break;
                }
            }
        }

        return next;
    }

    private static SeaFloor MoveSouth(SeaFloor previous)
    {
        var width = previous.Width;
        var height = previous.Height;

        var next = new SeaFloor(width, height);

        for (var x = 0; x < height; x++)
        {
            for (var y = 0; y < width; y++)
            {
                var seaCucumber = previous.GetSeaCucumber(x, y);

                switch (seaCucumber)
                {
                    case SeaCucumber.South:
                        if (previous.GetSeaCucumber(x + 1, y) == null)
                        {
                            next.AddCucumber(SeaCucumber.South, x + 1, y);
                        }
                        else
                        {
                            next.AddCucumber(SeaCucumber.South, x, y);
                        }
                        break;
                    case SeaCucumber.East:
                        next.AddCucumber(SeaCucumber.East, x, y);
                        break;
                    default:
                        break;
                }
            }
        }

        return next;
    }

    private class SeaFloor
    {
        private readonly SeaCucumber?[,] _cucumbers;
        public int Width => _cucumbers.GetLength(1);
        public int Height => _cucumbers.GetLength(0);

        public SeaFloor(int width, int height) => _cucumbers = new SeaCucumber?[height, width];

        public void AddCucumber(SeaCucumber? type, int x, int y)
        {
            x %= Height;
            y %= Width;
            _cucumbers[x, y] = type;
        }

        public SeaCucumber? GetSeaCucumber(int x, int y)
        {
            x %= Height;
            y %= Width;
            return _cucumbers[x, y];
        }

        public bool Equals(SeaFloor other)
        {
            return _cucumbers.Rank == other._cucumbers.Rank &&
                   Enumerable.Range(0, _cucumbers.Rank).All(dimension => _cucumbers.GetLength(dimension) == other._cucumbers.GetLength(dimension)) &&
                   _cucumbers.Cast<SeaCucumber?>().SequenceEqual(other._cucumbers.Cast<SeaCucumber?>());
        }
    }

    private enum SeaCucumber
    {
        East,
        South
    }
}
