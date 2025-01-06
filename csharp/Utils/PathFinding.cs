namespace Utils;

public static class PathFinding
{
    public static (int, IEnumerable<TNode> path) AStar<TNode>(
        TNode start,
        Func<TNode, bool> isGoal,
        Func<TNode, IEnumerable<TNode>> getNeighbours,
        Func<TNode, TNode, int> getCost,
        Func<TNode, int> getDistance)
        where TNode : IEquatable<TNode>
    {
        var queue = new PriorityQueue<TNode, int>();

        var cameFrom = new Dictionary<TNode, TNode>();
        var costSoFar = new Dictionary<TNode, int> { [start] = 0 };

        queue.Enqueue(start, getDistance(start));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (isGoal(current) && cameFrom.Count != 0)
            {
                var totalPath = new List<TNode> { current };

                var track = current;
                while (cameFrom.ContainsKey(track))
                {
                    track = cameFrom[track];
                    totalPath = totalPath.Prepend(track).ToList();
                }

                return (costSoFar[current], totalPath);
            }

            foreach (var neighbour in getNeighbours(current))
            {
                var tentativeGScore = costSoFar[current] + getCost(current, neighbour);
                if (costSoFar.TryGetValue(neighbour, out var value) && tentativeGScore >= value)
                {
                    continue;
                }

                cameFrom[neighbour] = current;
                costSoFar[neighbour] = tentativeGScore;
                queue.Enqueue(neighbour, tentativeGScore + getDistance(neighbour));
            }
        }

        return (int.MaxValue, new List<TNode>());
    }

    public static IEnumerable<(int cost, IEnumerable<TNode> path)> AStarMultiplePaths<TNode>(
        TNode start,
        Func<TNode, bool> isGoal,
        Func<TNode, IEnumerable<TNode>> getNeighbours,
        Func<TNode, TNode, int> getCost,
        Func<TNode, int> getDistance)
        where TNode : IEquatable<TNode>
    {
        var queue = new PriorityQueue<TNode, int>();

        var cameFrom = new Dictionary<TNode, HashSet<TNode>>();
        var costSoFar = new Dictionary<TNode, int> { [start] = 0 };

        queue.Enqueue(start, getDistance(start));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (isGoal(current) && cameFrom.Count != 0)
            {
                var cost = costSoFar[current];
                var queueResult = new Queue<List<TNode>>();
                queueResult.Enqueue([current]);

                while (queueResult.Count > 0)
                {
                    var currentResult = queueResult.Dequeue();
                    var toCheck = currentResult.Last();
                    if (!cameFrom.TryGetValue(toCheck, out var value))
                    {
                        yield return (cost, currentResult);
                    }
                    else
                    {
                        foreach (var item in value)
                        {
                            queueResult.Enqueue(currentResult.Append(item).ToList());
                        }
                    }
                }

                continue;
            }

            foreach (var neighbour in getNeighbours(current))
            {
                var tentativeGScore = costSoFar[current] + getCost(current, neighbour);
                var currentBestCost = costSoFar.TryGetValue(neighbour, out var value);
                if (currentBestCost && tentativeGScore > value)
                {
                    continue;
                }

                if (currentBestCost && tentativeGScore == value)
                {
                    cameFrom[neighbour].Add(current);
                }
                else
                {
                    cameFrom[neighbour] = [current];
                }

                costSoFar[neighbour] = tentativeGScore;
                queue.Enqueue(neighbour, tentativeGScore + getDistance(neighbour));
            }
        }
    }
}
