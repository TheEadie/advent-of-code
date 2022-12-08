namespace AdventOfCode2022;

public class Day08
{
    [TestCase("data/08 - Sample.txt", 21, TestName = "Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 1763, TestName = "Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var answer = 0;

        for (var x = 0; x < input.GetLength(0); x++)
        {
            for (var y = 0; y < input.GetLength(1); y++)
            {
                if (ClearViewUp(input, x, y) ||
                    ClearViewDown(input, x, y) ||
                    ClearViewLeft(input, x, y) ||
                    ClearViewRight(input, x, y))
                {
                    answer++;
                }
            }
        }

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }

    private static bool ClearViewUp(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        for (var i = 1; i <= y; i++)
        {
            if (input[x, y - i] >= tree)
            {
                return false;
            }
        }

        return true;
    }
    
    private static bool ClearViewDown(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        for (var i = 1; i < input.GetLength(1) - y; i++)
        {
            if (input[x, y + i] >= tree)
            {
                return false;
            }
        }

        return true;
    }
    
    private static bool ClearViewLeft(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        for (var i = 1; i <= x; i++)
        {
            if (input[x - i, y] >= tree)
            {
                return false;
            }
        }

        return true;
    }
    
    private static bool ClearViewRight(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        for (var i = 1; i < input.GetLength(0) - x; i++)
        {
            if (input[x + i, y] >= tree)
            {
                return false;
            }
        }

        return true;
    }

    [TestCase("data/08 - Sample.txt", 8, TestName = "Sample")]
    [TestCase("data/08 - Puzzle Input.txt", 671160, TestName = "Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var input = ParseInput(File.ReadAllText(inputFile));

        var scores = new List<int>();

        for (var x = 0; x < input.GetLength(0); x++)
        {
            for (var y = 0; y < input.GetLength(1); y++)
            {
                scores.Add(TreesUp(input, x, y) *
                           TreesDown(input, x, y) *
                           TreesLeft(input, x, y) *
                           TreesRight(input, x, y));
            }
        }
        
        var answer = scores.Max();

        Console.WriteLine(answer);
        answer.ShouldBe(expected);
    }
    
    private static int TreesUp(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        var treesSeen = 0;
        for (var i = 1; i <= y; i++)
        {
            treesSeen++;
            if (input[x, y - i] >= tree)
            {
                return treesSeen;
            }
        }

        return treesSeen;
    }
    
    private static int TreesDown(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        var treesSeen = 0;
        for (var i = 1; i < input.GetLength(1) - y; i++)
        {
            treesSeen++;
            if (input[x, y + i] >= tree)
            {
                return treesSeen;
            }
        }

        return treesSeen;
    }
    
    private static int TreesLeft(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        var treesSeen = 0;
        for (var i = 1; i <= x; i++)
        {
            treesSeen++;
            if (input[x - i, y] >= tree)
            {
                return treesSeen;
            }
        }

        return treesSeen;
    }
    
    private static int TreesRight(int[,] input, int x, int y)
    {
        var tree = input[x, y];
        var treesSeen = 0;
        for (var i = 1; i < input.GetLength(0) - x; i++)
        {
            treesSeen++;
            if (input[x + i, y] >= tree)
            {
                return treesSeen;
            }
        }

        return treesSeen;
    }
    
    private int[,] ParseInput(string input)
    {
        var lines = input.Split("\n");

        var treeField = new int[lines[0].Length, lines.Length];
        var y = 0;

        foreach (var line in lines)
        {
            var x = 0;
            foreach (var tree in line.ToCharArray())
            {
                treeField[x, y] = int.Parse(tree.ToString());
                x++;
            }
            y++;
        }

        return treeField;
    }
}