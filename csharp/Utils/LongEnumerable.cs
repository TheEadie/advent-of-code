namespace Utils;

public static class LongEnumerable
{
    public static IEnumerable<long> Range(long start, long length)
    {
        for (var i = start; i < start + length; i++)
        {
            yield return i;
        }
    }
}
