namespace Utils;

public record Coordinate(int X, int Y)
{
    public static Coordinate operator +(Coordinate left, Coordinate right) => new(left.X + right.X, left.Y + right.Y);

    public static Coordinate operator -(Coordinate left, Coordinate right) => new(left.X - right.X, left.Y - right.Y);

    public static Coordinate operator +(Coordinate left, Vector right) => new(left.X + right.X, left.Y + right.Y);

    public static Coordinate operator -(Coordinate left, Vector right) => new(left.X - right.X, left.Y - right.Y);

    public static int ManhattanDistance(Coordinate start, Coordinate goal)
    {
        var xDiff = start.X - goal.X;
        var yDiff = start.Y - goal.Y;
        return Math.Abs(xDiff) + Math.Abs(yDiff);
    }
}

public record Line(Coordinate Start, Coordinate End)
{
    public IEnumerable<Coordinate> GetCoordinatesOnLine()
    {
        var xDiff = End.X - Start.X;
        var yDiff = End.Y - Start.Y;
        var xMove = xDiff == 0 ? 0 : xDiff > 0 ? 1 : -1;
        var yMove = yDiff == 0 ? 0 : yDiff > 0 ? 1 : -1;

        var coordinates = new List<Coordinate>();
        var current = new Coordinate(Start.X, Start.Y);
        while (current != End)
        {
            coordinates.Add(current);
            current = new Coordinate(current.X + xMove, current.Y + yMove);
        }

        coordinates.Add(current);
        return coordinates;
    }
}

public record Vector(int X, int Y)
{
    public Vector(Coordinate from, Coordinate to)
        : this(to.X - from.X, to.Y - from.Y) { }

    public static Vector Up => new(0, -1);
    public static Vector Down => new(0, 1);
    public static Vector Left => new(-1, 0);
    public static Vector Right => new(1, 0);

    public static Vector UpLeft => new(-1, -1);
    public static Vector UpRight => new(1, -1);
    public static Vector DownLeft => new(-1, 1);
    public static Vector DownRight => new(1, 1);

    public Vector TurnLeft() => new(Y, -X);

    public Vector TurnRight() => new(-Y, X);

    public static IEnumerable<Vector> FourDirections() =>
    [
        Up,
        Down,
        Left,
        Right
    ];

    public static IEnumerable<Vector> DiagonalDirections() =>
    [
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    ];

    public static IEnumerable<Vector> EightDirections() =>
    [
        Up,
        UpLeft,
        UpRight,
        Down,
        DownLeft,
        DownRight,
        Left,
        Right
    ];
}
