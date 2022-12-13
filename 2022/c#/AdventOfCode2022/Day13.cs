using System.Collections;

namespace AdventOfCode2022;

public class Day13
{
    [TestCase("data/13 - Sample.txt", 13, TestName = "Sample")]
    [TestCase("data/13 - Puzzle Input.txt", 6428, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));
        var indices = new List<int>();
        var index = 0;
        
        foreach (var (one, two) in input)
        {
            index++;
            if (IsInCorrectOrder(one, two)!.Value)
            {
                indices.Add(index);
            }
        }
        
        var answer = indices.Sum();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    [TestCase("data/13 - Sample.txt", 140, TestName = "Part 2 - Sample")]
    [TestCase("data/13 - Puzzle Input.txt", 22464, TestName = "Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = File.ReadAllLines(inputFile)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(ParseLine)
            .ToList();
        input.Add(new Element(new List<Element> {new(2)}));
        input.Add(new Element(new List<Element> {new(6)}));

        input.Sort(new ElementComparer());

        var index1 = input.TakeWhile(x =>
        {
            if (!x.IsList()) return true;
            var elementOne = x.GetList().FirstOrDefault();
            if (elementOne is not null && elementOne.IsNumber())
            {
                return elementOne.GetNumber() != 2;
            }
            return true;
        }).Count() + 1;
        
        var index2 = input.TakeWhile(x =>
        {
            if (!x.IsList()) return true;
            var elementOne = x.GetList().FirstOrDefault();
            if (elementOne is not null && elementOne.IsNumber())
            {
                return elementOne.GetNumber() != 6;
            }
            return true;
        }).Count() + 1;
        
        var answer = index1 * index2;

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private class ElementComparer : IComparer<Element>
    {
        public int Compare(Element? x, Element? y)
        {
            if (IsInCorrectOrder(x, y).Value)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    private static bool? IsInCorrectOrder(Element one, Element two)
    {
        if (one.IsNumber() && two.IsNumber())
        {
            var inOne = one.GetNumber();
            var inTwo = two.GetNumber();
            
            if (inOne < inTwo)
                return true;
            if (inOne > inTwo)
                return false;

            return null;
        }

        if (one.IsList() && two.IsList())
        {
            var listOne = one.GetList();
            var listTwo = two.GetList();

            for (var i = 0; i < listOne.Count; i++)
            {
                if (i >= listTwo.Count)
                {
                    return false;
                }
                
                var inOne = listOne[i];
                var inTwo = listTwo[i];

                var isInCorrectOrder = IsInCorrectOrder(inOne, inTwo);
                if (isInCorrectOrder is not null)
                    return isInCorrectOrder;
            }

            if (listTwo.Count > listOne.Count)
            {
                return true;
            }
        }

        if (one.IsNumber() && two.IsList())
        {
            var isInCorrectOrder = IsInCorrectOrder(new Element(new List<Element> {one}), two);
            if (isInCorrectOrder is not null)
                return isInCorrectOrder;
        }
        
        if (one.IsList() && two.IsNumber())
        {
            var isInCorrectOrder = IsInCorrectOrder(one, new Element(new List<Element> {two}));
            if (isInCorrectOrder is not null)
                return isInCorrectOrder;
            
        }

        return null;
    }

    private IEnumerable<(Element, Element)> ParseInput(string input)
    {
        (Element, Element) ParsePair(string pair)
        {
            var lines = pair.Split("\n");

            return (ParseLine(lines[0]), ParseLine(lines[1]));
        }

        return input.Split("\n\n").Select(ParsePair);
    }

    private Element ParseLine(string line)
    {
        var root = new Element(new List<Element>());
        var stack = new Stack<Element>();
        var current = root;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '[')
            {
                var next = new Element(new List<Element>());
                current.Add(next);
                stack.Push(current);
                current = next;
            }
            else if (line[i] == ']')
            {
                current = stack.Pop();
            }
            else
            {
                var contents = new string(line[i..].TakeWhile(x => x != ']' && x != '[').ToArray());
                var numbers = contents?
                    .Split(",")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(int.Parse);

                if (numbers != null)
                {
                    foreach (var num in numbers)
                    {
                        current.Add(new Element(num));
                    }
                }

                i += contents.Length - 1;
            }
                
        }

        return root.GetList()[0];
    }

    private class Element : OneOf<IList<Element>, int>
    {
        public Element(IList<Element> list) : base(list) {}
        public Element(int value) : base(value) {}

        public bool IsList() => Which == 1;
        public IList<Element> GetList() => One;
        public bool IsNumber() => Which == 2;
        public int GetNumber() => Two;
        
        public void Add(Element toAdd) => One.Add(toAdd);

        public override string ToString()
        {
            switch (Which)
            {
                case 1:
                    return "[" + string.Join(",", One) + "]";
                case 2:
                    return Two.ToString();
            }

            return "";
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