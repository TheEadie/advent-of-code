namespace AdventOfCode2022;

public static class PathFinding
{
    public static (int, IEnumerable<TNode> path) AStar<TNode>(
        TNode start,
        TNode goal,
        Func<TNode, IEnumerable<TNode>> getNeighbours,
        Func<TNode, TNode, int> getCost,
        Func<TNode, TNode, int> getDistance)
        where TNode : IEquatable<TNode>
    {
        var queue = new PriorityQueue<TNode, int>();

        var cameFrom = new Dictionary<TNode, TNode>();
        var costSoFar = new Dictionary<TNode, int>
        {
            [start] = 0
        };

        var totalPath = new List<TNode> { goal };

        queue.Enqueue(start, getDistance(start, goal));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (EqualityComparer<TNode>.Default.Equals(current, goal))
            {
                break;
            }

            foreach (var neighbour in getNeighbours(current))
            {
                var tentativeGScore = costSoFar[current] + getCost(current, neighbour);
                if (!costSoFar.ContainsKey(neighbour) || tentativeGScore < costSoFar[neighbour])
                {
                    cameFrom[neighbour] = current;
                    costSoFar[neighbour] = tentativeGScore;
                    queue.Enqueue(neighbour, tentativeGScore + getDistance(neighbour, goal));
                }
            }
        }

        var track = goal;
        while (cameFrom.ContainsKey(track))
        {
            track = cameFrom[track];
            totalPath = totalPath.Prepend(track).ToList();
        }

        return costSoFar.TryGetValue(goal, out int value) ? (value, totalPath) : (int.MaxValue, totalPath);
    }
}