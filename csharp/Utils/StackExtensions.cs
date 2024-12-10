namespace Utils;

public static class StackExtensions
{
    public static Stack<T> PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            stack.Push(item);
        }

        return stack;
    }
}
