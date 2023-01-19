namespace AdventOfCode2021
{
    public class Day12
    {
        [Test]
        public void Part1()
        {
            var graph = ParseInput();

            var paths = GetPaths("start", graph, new List<string> {"start"});

            var answer = paths.Count();
            Console.WriteLine(answer);
            answer.ShouldBe(3802);
        }

        private IEnumerable<IEnumerable<string>> GetPaths(string next, Dictionary<string, IList<string>> graph, IList<string> pathSoFar)
        {
            var paths = new List<IEnumerable<string>>();

            if (next == "end")
                return new List<IEnumerable<string>>() { pathSoFar };

            foreach (var node in graph[next])
            {
                if (pathSoFar.Where(x => x == x.ToLower()).Contains(node))
                {
                    continue;
                }
                var route = new List<string>();
                route.AddRange(pathSoFar);
                route.Add(node);
                paths.AddRange(GetPaths(node, graph, route));
            }

            return paths;
        }

        [Test]
        public void Part2()
        {
            var graph = ParseInput();
            var paths = GetPathsPart2("start", graph, new List<string> {"start"});

            var answer = paths.Count();
            Console.WriteLine(answer);
            answer.ShouldBe(99448);
        }
        
        private IEnumerable<IEnumerable<string>> GetPathsPart2(string next, Dictionary<string, IList<string>> graph, IList<string> pathSoFar)
        {
            var paths = new List<IEnumerable<string>>();

            if (next == "end")
                return new List<IEnumerable<string>>() { pathSoFar };

            foreach (var node in graph[next])
            {
                if (InvalidMove(node, pathSoFar))
                {
                    continue;
                }
                var route = new List<string>();
                route.AddRange(pathSoFar);
                route.Add(node);
                paths.AddRange(GetPathsPart2(node, graph, route));
            }

            return paths;
        }

        private bool InvalidMove(string node, IList<string> pathSoFar)
        {
            if (node == "start")
                return true;
            
            var visitedTwoSmallCaves = pathSoFar
                .Where(x => x == x.ToLower())
                .GroupBy(x => x)
                .SingleOrDefault(x => x.Count() == 2);

            if (visitedTwoSmallCaves is not null)
            {
                if (visitedTwoSmallCaves.Key == node)
                    return true;
                return pathSoFar.Where(x => x == x.ToLower()).Contains(node);
            }

            return false;

        }

        private static Dictionary<string, IList<string>> ParseInput()
        {
            var graph = new Dictionary<string, IList<string>>();
            var lines = File.ReadAllLines("Day12.txt");
            var connections = lines.Select(x => x.Split('-')).Select(x => new {Source = x[0], Target = x[1]});

            foreach (var connection in connections)
            {
                if (!graph.Keys.Contains(connection.Source))
                {
                    graph.Add(connection.Source, new List<string>());
                }

                graph[connection.Source].Add(connection.Target);

                if (!graph.Keys.Contains(connection.Target))
                {
                    graph.Add(connection.Target, new List<string>());
                }

                graph[connection.Target].Add(connection.Source);
            }

            return graph;
        }
    }
}
