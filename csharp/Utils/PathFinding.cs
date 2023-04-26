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
        var costSoFar = new Dictionary<TNode, int>
        {
            [start] = 0
        };

        queue.Enqueue(start, getDistance(start));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (isGoal(current))
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
                if (!costSoFar.ContainsKey(neighbour) || tentativeGScore < costSoFar[neighbour])
                {
                    cameFrom[neighbour] = current;
                    costSoFar[neighbour] = tentativeGScore;
                    queue.Enqueue(neighbour, tentativeGScore + getDistance(neighbour));
                }
            }
        }

        return (int.MaxValue, new List<TNode>());
    }
}
