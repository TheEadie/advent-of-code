namespace Utils;

public static class LinqExtensions
{
    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int size)
    {
        var enumerable = source.ToList();
        for (var i = 0; i < enumerable.Count; i++)
        {
            yield return enumerable.Skip(i).Take(size);
        }
    }

    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> data, Func<T, bool> predicate)
    {
        foreach (var item in data)
        {
            yield return item;

            if (predicate(item))
            {
                break;
            }
        }
    }

    public static IEnumerable<T[]> Permutate<T>(this IEnumerable<T> source)
    {
        var array = source.ToArray();
        return PermutateInner(array, Enumerable.Empty<T>());

        IEnumerable<T[]> PermutateInner(IReadOnlyCollection<T> reminder, IEnumerable<T> prefix) =>
            reminder.Count == 0
                ? new[] { prefix.ToArray() }
                : reminder.SelectMany(
                    (c, i) => PermutateInner(
                        reminder.Take(i).Concat(reminder.Skip(i + 1)).ToArray(),
                        prefix.Append(c)));
    }
}
