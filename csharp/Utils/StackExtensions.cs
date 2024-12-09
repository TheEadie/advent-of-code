namespace Utils;

public static class StackExtensions
{
    public static (ICollection<T> Rejected, T? Found) Pop<T>(this Stack<T> stack, Func<T, bool> predicate)
        where T : class
    {
        var rejected = new List<T>();
        while (!predicate(stack.Peek()))
        {
            rejected.Add(stack.Pop());
            if (stack.Count == 0)
            {
                return (rejected, null);
            }
        }

        return (rejected, stack.Pop());
    }

    public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            stack.Push(item);
        }
    }
}
