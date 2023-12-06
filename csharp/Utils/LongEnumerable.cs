namespace Utils;

public static class LongEnumerable
{
    public static IEnumerable<long> Range(long start, long length)
    {
        for (var i = 0L; i < start + length; i++)
        {
            yield return start + i;
        }
    }
}
