namespace AdventOfCode2021.Day10;

public class Day10
{
    private readonly AdventSession _session = new(2021, 10, "Syntax Scoring");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var lines = ParseInput(input);
        var answer = 0;

        foreach (var line in lines)
        {
            var stack = new Stack<char>();

            foreach (var bracket in line)
            {
                if (bracket is '(' or '[' or '{' or '<')
                {
                    stack.Push(bracket);
                }
                else
                {
                    var match = stack.Pop();
                    if (!IsClosingBracket(bracket, match))
                    {
                        answer += GetScore(bracket);
                        break;
                    }
                }
            }
        }

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(316851);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var lines = ParseInput(input);
        var scores = new List<long>();

        foreach (var line in lines)
        {
            var stack = new Stack<char>();
            var corrupted = false;

            foreach (var bracket in line)
            {
                if (bracket is '(' or '[' or '{' or '<')
                {
                    stack.Push(bracket);
                }
                else
                {
                    var match = stack.Pop();
                    if (!IsClosingBracket(bracket, match))
                    {
                        corrupted = true;
                        break;
                    }
                }
            }

            if (!corrupted)
            {
                long score = 0;
                foreach (var bracket in stack)
                {
                    score *= 5;
                    score += GetAutoCompleteScore(bracket);
                }

                scores.Add(score);
            }
        }

        var answer = scores.OrderBy(x => x).ElementAt(scores.Count / 2);

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(2182912364);
    }

    private static IEnumerable<char[]> ParseInput(string input)
    {
        var lines = input.Split("\n");
        return lines.Select(line => line.ToCharArray()).ToList();
    }

    private static bool IsClosingBracket(char bracket, char match)
    {
        return bracket switch
        {
            ')' => match == '(',
            ']' => match == '[',
            '}' => match == '{',
            '>' => match == '<',
            _ => false
        };
    }

    private static int GetScore(char bracket)
    {
        return bracket switch
        {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => 0
        };
    }

    private static int GetAutoCompleteScore(char bracket)
    {
        return bracket switch
        {
            '(' => 1,
            '[' => 2,
            '{' => 3,
            '<' => 4,
            _ => 0
        };
    }
}
