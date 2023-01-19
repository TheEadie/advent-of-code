using AdventOfCode2021.Utils;

namespace AdventOfCode2021
{
    public class Day13
    {
        [Test]
        public void Part1()
        {
            var (coordinates, folds) = ParseInput();

            var answer = Reflect(coordinates, folds.Take(1)).Distinct().Count();

            Console.WriteLine(answer);
            answer.ShouldBe(729);
        }

        [Test]
        public void Part2()
        {
            var (coordinates, folds) = ParseInput();
            var answer = Reflect(coordinates, folds).Distinct().ToList();

            for (var y = 0; y <= answer.Max(c => c.Y); y++)
            {
                for (var x = 0; x <= answer.Max(c => c.X); x++)
                {
                    Console.Write(answer.Contains(new Coordinate(x, y)) ? "*" : " ");
                }
                Console.Write(Environment.NewLine);
            }

            // RGZLBHFP
            answer.Count.ShouldBe(100);
        }

        private static IEnumerable<Coordinate> Reflect(IEnumerable<Coordinate> coordinates, IEnumerable<Fold> folds)
        {
            return folds.Aggregate(coordinates, Reflect);
        }

        private static IEnumerable<Coordinate> Reflect(IEnumerable<Coordinate> coordinates, Fold fold)
        {
            var newCoordinates = new List<Coordinate>();

            foreach (var coordinate in coordinates)
            {
                var xDiff = fold.Along == 'x' ? coordinate.X - fold.Position : 0;
                var yDiff = fold.Along == 'y' ? coordinate.Y - fold.Position : 0;

                newCoordinates.Add(coordinate);
                newCoordinates.Add(new Coordinate(coordinate.X - 2*xDiff, coordinate.Y - 2*yDiff));
            }

            newCoordinates.RemoveAll(x => (fold.Along == 'x' ? (x.X) : (x.Y)) > fold.Position);

            return newCoordinates;
        }

        private static (IEnumerable<Coordinate>, IEnumerable<Fold>) ParseInput()
        {
            var graph = new Dictionary<string, IList<string>>();
            var lines = File.ReadAllLines("Day13.txt");

            var coordinates = lines.TakeWhile(x => x != "")
                .Select(x => x.Split(','))
                .Select(x => new Coordinate(int.Parse(x[0]), int.Parse(x[1])));

            var folds = lines.SkipWhile(x => x != "").Skip(1)
                .Select(x => x.Split(' '))
                .Select(x => x[2].Split('='))
                .Select(x => new Fold(char.Parse(x[0]), int.Parse(x[1])));


            return (coordinates, folds);
        }

        private record Fold(char Along, int Position);

    }
}
