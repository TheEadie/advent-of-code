namespace Utils;

public record Coordinate3D(long X, long Y, long Z)
{
    public static double StraightDistance(Coordinate3D start, Coordinate3D goal)
    {
        var xDiff = start.X - goal.X;
        var yDiff = start.Y - goal.Y;
        var zDiff = start.Z - goal.Z;
        return Math.Sqrt(xDiff * xDiff + yDiff * yDiff + zDiff * zDiff);
    }
}
