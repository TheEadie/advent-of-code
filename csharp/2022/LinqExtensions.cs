namespace AdventOfCode2022;

public static class LinqExtensions
{
    public static IEnumerable<IEnumerable<T>> Window<T>(this IEnumerable<T> source, int size)
    {
        for (var i = 0; i < source.Count(); i++)
        {
            yield return source.Skip(i).Take(size);
        }
    }

    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> data, Func<T, bool> predicate)
    {
        foreach (var item in data)
        {
            yield return item;
            if (predicate(item))
                break;
        }
    }
}