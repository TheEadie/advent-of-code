
namespace AdventOfCode2015.Day09;

public class Day09
{
    private readonly AdventSession _session = new(2015, 9, "All in a Single Night");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 605)]
    [TestCase("Puzzle Input.txt", 117)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var allRoutes = ParseRoutes(input);
        var answer = GetRoutesThatVisitAllCities(allRoutes)
            .Min(x => x.TotalDistance);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 982)]
    [TestCase("Puzzle Input.txt", 909)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);

        var allRoutes = ParseRoutes(input);
        var answer = GetRoutesThatVisitAllCities(allRoutes)
            .Max(x => x.TotalDistance);

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<PossibleRoute> GetRoutesThatVisitAllCities(IReadOnlyCollection<Route> allRoutes)
    {
        var found = new List<PossibleRoute>();

        var cities = allRoutes.Select(x => x.From).Distinct().ToList();

        foreach (var city in cities)
        {
            var toVisit = new Queue<PossibleRoute>();
            toVisit.Enqueue(new PossibleRoute(city, city, 0));

            while (toVisit.Count > 0)
            {
                var next = toVisit.Dequeue();

                if (cities.All(next.Visited.Contains))
                {
                    found.Add(next);
                    continue;
                }

                var nextRoutes = allRoutes
                    .Where(x => x.From == next.Current)
                    .Where(route => !next.Visited.Contains(route.To))
                    .Select(route =>
                        new PossibleRoute(
                            route.To,
                            next.Visited + route.To,
                            next.TotalDistance + route.Distance))
                    .ToList();

                foreach (var nextRoute in nextRoutes)
                {
                    toVisit.Enqueue(nextRoute);
                }
            }
        }

        return found;
    }

    private record PossibleRoute(string Current, string Visited, int TotalDistance);

    private static IReadOnlyCollection<Route> ParseRoutes(string input)
    {
        return input
            .Split("\n")
            .Select(line => line.Split(" "))
            .Select(parts =>
                new Route(
                    parts[0],
                    parts[2],
                    int.Parse(parts[4])))
            .SelectMany(route => new[]
            {
                new Route(route.From, route.To, route.Distance),
                new Route(route.To, route.From, route.Distance)
            })
            .ToList();
    }

    private record Route(string From, string To, int Distance);
}
