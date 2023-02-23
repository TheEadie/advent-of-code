namespace Utils;

public record Coordinate(int X, int Y);

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
    public static Vector Up => new(0, -1);
    public static Vector Down => new(0, 1);
    public static Vector Left => new(-1, 0);
    public static Vector Right => new(1, 0);
}