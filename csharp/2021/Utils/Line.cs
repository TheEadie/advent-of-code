namespace AdventOfCode2021.Utils
{
    public class Line
    {
        public Coordinate Start { get; }
        public Coordinate End { get; }

        public Line(Coordinate start, Coordinate end)
        {
            Start = start;
            End = end;
        }

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
}
