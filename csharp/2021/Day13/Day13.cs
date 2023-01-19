namespace AdventOfCode2021.Day13
{
    public class Day13
    {
        private readonly AdventSession _session = new(2021, 13, "Transparent Origami");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var (coordinates, folds) = ParseInput(input);

            var answer = Reflect(coordinates, folds.Take(1)).Distinct().Count();

            _session.PrintAnswer(1, answer);
            answer.ShouldBe(729);
        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var (coordinates, folds) = ParseInput(input);
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
            _session.PrintAnswer(2, "");
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

        private static (IEnumerable<Coordinate>, IEnumerable<Fold>) ParseInput(string input)
        {
            var lines = input.Split("\n");

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
