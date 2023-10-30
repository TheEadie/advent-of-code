using System.Text;

namespace AdventOfCode2021.Day18;

public class Day18
{
    private readonly AdventSession _session = new(2021, 18, "Snailfish");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var snailFishNumbers = ParseInput(input).ToList();

        var total = snailFishNumbers.Aggregate(Add);

        var answer = GetMagnitude(total);
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(4111);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var lines = input.Split("\n");

        var sums = new List<long>();
        foreach (var first in lines)
        {
            foreach (var second in lines)
            {
                var magnitude = GetMagnitude(Add(ParseSnailFishNumber(first), ParseSnailFishNumber(second)));
                sums.Add(magnitude);
            }
        }

        var answer = sums.Max();
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(4917);
    }

    private SnailFishNumber Add(SnailFishNumber one, SnailFishNumber two)
    {
        var newNumber = new SnailFishNumber(one, two);
        one.Parent = newNumber;
        two.Parent = newNumber;

        Reduce(newNumber);
        return newNumber;
    }

    private void Reduce(SnailFishNumber number)
    {
        var more = true;
        while (more)
        {
            more = Explode(number);
            if (!more)
            {
                more = Split(number);
            }
        }

    }

    private long GetMagnitude(SnailFishNumber number) => number.Value ?? 3 * GetMagnitude(number.Left!) + 2 * GetMagnitude(number.Right!);

    private bool Split(SnailFishNumber root)
    {
        // Get the first number > 9
        var numberToSplit = GetFirstGreaterThanNine(root);
        if (numberToSplit == null)
        {
            return false;
        }

        // Change to a pair
        var value = numberToSplit.Value!.Value;

        var left = value / 2;
        var right = value % 2 == 0 ? value / 2 : value / 2 + 1;

        numberToSplit.Left = new SnailFishNumber(left)
        {
            Parent = numberToSplit
        };
        numberToSplit.Right = new SnailFishNumber(right)
        {
            Parent = numberToSplit
        };
        numberToSplit.Value = null;

        return true;
    }

    private bool Explode(SnailFishNumber root)
    {
        // Get the first pair at depth 4
        var numberToExplode = GetNextNumberToExplode(root);

        if (numberToExplode == null)
        {
            return false;
        }

        // Find left
        var left = GetLeft(numberToExplode);
        // increment
        if (left != null)
        {
            left.Value += numberToExplode.Left!.Value;
        }
        // Find right
        var right = GetRight(numberToExplode);
        // increment
        if (right != null)
        {
            right.Value += numberToExplode.Right!.Value;
        }
        // set to 0

        numberToExplode.Left = null;
        numberToExplode.Right = null;
        numberToExplode.Value = 0;

        return true;
    }

    private SnailFishNumber? GetLeft(SnailFishNumber element)
    {
        var start = GoUpTillCanGoLeft(element);
        return start == null ? null : DFSRight(start.Left!, x => x.Value != null);
    }

    private SnailFishNumber? GetRight(SnailFishNumber element)
    {
        var start = GoUpTillCanGoRight(element);
        return start == null ? null : DFSLeft(start.Right!, x => x.Value != null);
    }

    private static SnailFishNumber? DFSRight(SnailFishNumber root, Func<SnailFishNumber, bool> found)
    {
        var queue = new Stack<SnailFishNumber>();
        queue.Push(root);

        while (queue.Count > 0)
        {
            var number = queue.Pop();

            if (found(number))
            {
                return number;
            }

            if (number.Left != null)
            {
                queue.Push(number.Left);
            }

            if (number.Right != null)
            {
                queue.Push(number.Right);
            }
        }

        return null;
    }

    private static SnailFishNumber? DFSLeft(SnailFishNumber root, Func<SnailFishNumber, bool> found)
    {
        var queue = new Stack<SnailFishNumber>();
        queue.Push(root);

        while (queue.Count > 0)
        {
            var number = queue.Pop();

            if (found(number))
            {
                return number;
            }

            if (number.Right != null)
            {
                queue.Push(number.Right);
            }

            if (number.Left != null)
            {
                queue.Push(number.Left);
            }
        }

        return null;
    }

    private static SnailFishNumber? GoUpTillCanGoLeft(SnailFishNumber element)
    {
        while (element.Parent != null)
        {
            var parent = element.Parent;

            if (parent.Left != null && parent.Left != element)
            {
                return parent;
            }

            element = parent;
        }

        return null;
    }

    private static SnailFishNumber? GoUpTillCanGoRight(SnailFishNumber element)
    {
        while (element.Parent != null)
        {
            var parent = element.Parent;

            if (parent.Right != null && parent.Right != element)
            {
                return parent;
            }

            element = parent;
        }

        return null;
    }

    private SnailFishNumber? GetNextNumberToExplode(SnailFishNumber root)
    {
        var queue = new Stack<(SnailFishNumber, int)>();

        queue.Push((root, 0));
        while (queue.Count > 0)
        {
            var (number, depth) = queue.Pop();

            if (IsPair(number) && depth > 3)
            {
                return number;
            }

            if (number.Right != null)
            {
                queue.Push((number.Right, depth + 1));
            }

            if (number.Left != null)
            {
                queue.Push((number.Left, depth + 1));
            }
        }

        return null;
    }

    private SnailFishNumber? GetFirstGreaterThanNine(SnailFishNumber root) => DFSLeft(root, x => x.Value > 9);

    private bool IsPair(SnailFishNumber number) => number.Left?.Value != null && number.Right?.Value != null;

    private static IEnumerable<SnailFishNumber> ParseInput(string input) => input.Split("\n").Select(ParseSnailFishNumber);

    private static SnailFishNumber ParseSnailFishNumber(string input)
    {
        var current = new SnailFishNumber(null, null);

        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case '[':
                    {
                        var temp = new SnailFishNumber(null, null)
                        {
                            Parent = current
                        };
                        current!.Left = temp;
                        current = temp;
                        break;
                    }
                case ',' when input[i + 1] == '[':
                    {
                        var temp = new SnailFishNumber(null, null)
                        {
                            Parent = current
                        };
                        current!.Right = temp;
                        current = temp;
                        i++;
                        break;
                    }
                case ']':
                    current = current!.Parent;
                    break;
                case ',':
                    {
                        var temp = new SnailFishNumber(int.Parse(input[i + 1].ToString()))
                        {
                            Parent = current
                        };
                        current!.Right = temp;
                        i++;
                        break;
                    }
                default:
                    {
                        var temp = new SnailFishNumber(int.Parse(input[i].ToString()))
                        {
                            Parent = current
                        };
                        current!.Left = temp;
                        break;
                    }
            }
        }

        return current!.Left ?? throw new ArgumentException("Invalid input");
    }

    private class SnailFishNumber
    {
        public SnailFishNumber? Parent { get; set; }
        public SnailFishNumber? Left { get; set; }
        public SnailFishNumber? Right { get; set; }
        public int? Value { get; set; }

        public SnailFishNumber(SnailFishNumber? left, SnailFishNumber? right)
        {
            Left = left;
            Right = right;
            Value = null;
        }

        public SnailFishNumber(int value)
        {
            Left = null;
            Right = null;
            Value = value;
        }

        public override string ToString()
        {
            var value = new StringBuilder();

            if (Left != null)
            {
                _ = value.Append("[" + Left + ",");
            }

            if (Value != null)
            {
                _ = value.Append(Value);
            }

            if (Right != null)
            {
                _ = value.Append(Right + "]");
            }

            return value.ToString();

        }
    }
}
