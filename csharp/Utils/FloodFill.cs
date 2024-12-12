namespace Utils;

public static class Fill
{
    public static HashSet<HashSet<T>> GetAllSections<T>(
        IEnumerable<T> map,
        Func<T, IEnumerable<T>> getNeighbours,
        Func<T, T, bool> isMatch)
    {
        var sections = new HashSet<HashSet<T>>();
        var visited = new HashSet<T>();

        foreach (var item in map)
        {
            if (visited.Contains(item))
            {
                continue;
            }

            var section = FloodFill(item, getNeighbours, isMatch);
            sections.Add(section);
            visited.UnionWith(section);
        }

        return sections;
    }

    public static HashSet<T> FloodFill<T>(T start, Func<T, IEnumerable<T>> getNeighbours, Func<T, T, bool> isMatch)
    {
        var queue = new Queue<T>();
        var visited = new HashSet<T>();

        queue.Enqueue(start);
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current))
            {
                continue;
            }

            var neighbours = getNeighbours(current).Where(x => isMatch(current, x));

            foreach (var neighbour in neighbours)
            {
                if (!visited.Contains(neighbour))
                {
                    queue.Enqueue(neighbour);
                }
            }
        }

        return visited;
    }
}
