namespace AdventOfCode2022;

public class Day13
{
    [TestCase("data/13 - Sample.txt", 13, TestName = "Day 13 - Part 1 - Sample")]
    [TestCase("data/13 - Puzzle Input.txt", 6428, TestName = "Day 13 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var answer = File.ReadAllText(inputFile)
            .Split("\n\n")
            .Select((pair, i) =>
            {
                var lines = pair.Split("\n");
                return new { index = i, left = ParseElement(lines[0]), right = ParseElement(lines[1]) };
            })
            .Where(x => new ElementComparer().Compare(x.left, x.right) < 0)
            .Select(x => x.index + 1)
            .Sum();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("data/13 - Sample.txt", 140, TestName = "Day 13 - Part 2 - Sample")]
    [TestCase("data/13 - Puzzle Input.txt", 22464, TestName = "Day 13 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var two = new Element(new List<Element> { new(2) });
        var six = new Element(new List<Element> { new(6) });

        var input = File.ReadAllLines(inputFile)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(ParseElement)
            .Append(two)
            .Append(six)
            .ToList()
            .Order(new ElementComparer())
            .ToList();

        var indexOfTwo = input.TakeWhile(x => new ElementComparer().Compare(x, two) != 0).Count() + 1;
        var indexOfSix = input.TakeWhile(x => new ElementComparer().Compare(x, six) != 0).Count() + 1;

        var answer = indexOfTwo * indexOfSix;

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static Element ParseElement(string line)
    {
        var root = new Element(new List<Element>());
        var stack = new Stack<Element>();
        var current = root;

        for (var i = 0; i < line.Length; i++)
        {
            switch (line[i])
            {
                case '[':
                    {
                        var next = new Element(new List<Element>());
                        current.Add(next);
                        stack.Push(current);
                        current = next;
                        break;
                    }
                case ']':
                    current = stack.Pop();
                    break;
                default:
                    {
                        var contents = new string(line[i..].TakeWhile(x => x != ']' && x != '[').ToArray());
                        var numbers = contents
                            .Split(",")
                            .Where(x => !string.IsNullOrEmpty(x))
                            .Select(int.Parse);

                        foreach (var num in numbers)
                        {
                            current.Add(new Element(num));
                        }

                        i += contents.Length - 1;
                        break;
                    }
            }
        }

        return root.GetList()[0];
    }

    private class Element : OneOf<IList<Element>, int>
    {
        public Element(IList<Element> list) : base(list) { }
        public Element(int value) : base(value) { }

        public bool IsList() => Which == 1;
        public IList<Element> GetList() => IsList() ? One! : throw new ArgumentException();
        public bool IsNumber() => Which == 2;
        public int GetNumber() => IsNumber() ? Two : throw new ArgumentException();

        public void Add(Element toAdd)
        {
            if (IsList())
            {
                One!.Add(toAdd);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return Which switch
            {
                1 => "[" + string.Join(",", One!) + "]",
                2 => Two.ToString(),
                _ => "Error!"
            };
        }
    }

    private class ElementComparer : IComparer<Element>
    {
        public int Compare(Element? x, Element? y)
        {
            if (x is null || y is null)
            {
                throw new NotSupportedException();
            }

            if (x.IsNumber() && y.IsNumber())
            {
                var inOne = x.GetNumber();
                var inTwo = y.GetNumber();

                if (inOne < inTwo)
                    return -1;
                if (inOne > inTwo)
                    return 1;

                return 0;
            }

            if (x.IsList() && y.IsList())
            {
                var listOne = x.GetList();
                var listTwo = y.GetList();

                for (var i = 0; i < listOne.Count; i++)
                {
                    if (i >= listTwo.Count)
                    {
                        return 1;
                    }

                    var inOne = listOne[i];
                    var inTwo = listTwo[i];

                    var isInCorrectOrder = Compare(inOne, inTwo);
                    if (isInCorrectOrder != 0)
                        return isInCorrectOrder;
                }

                if (listTwo.Count > listOne.Count)
                {
                    return -1;
                }
            }

            if (x.IsNumber() && y.IsList())
            {
                var isInCorrectOrder = Compare(new Element(new List<Element> { x }), y);
                if (isInCorrectOrder != 0)
                    return isInCorrectOrder;
            }

            if (x.IsList() && y.IsNumber())
            {
                var isInCorrectOrder = Compare(x, new Element(new List<Element> { y }));
                if (isInCorrectOrder != 0)
                    return isInCorrectOrder;

            }

            return 0;
        }
    }

    private class OneOf<T1, T2>
    {
        protected readonly T1? One;
        protected readonly T2? Two;
        protected readonly int Which;

        protected OneOf(T1 value)
        {
            Which = 1;
            One = value;
        }

        protected OneOf(T2 value)
        {
            Which = 2;
            Two = value;
        }
    }
}