namespace AdventOfCode2022.Day02;

public class Day02
{
    [TestCase("Day02/Sample.txt", 15, TestName = "Day 02 - Part 1 - Sample")]
    [TestCase("Day02/Puzzle Input.txt", 13009, TestName = "Day 02 - Part 1 - Puzzle Input")]
    public void Part1(string inputFile, int expected)
    {
        var strategies = File.ReadAllLines(inputFile)
            .Select(x => x switch
            {
                "A X" => new Round(Hand.Rock, Hand.Rock),
                "B X" => new Round(Hand.Paper, Hand.Rock),
                "C X" => new Round(Hand.Scissors, Hand.Rock),
                "A Y" => new Round(Hand.Rock, Hand.Paper),
                "B Y" => new Round(Hand.Paper, Hand.Paper),
                "C Y" => new Round(Hand.Scissors, Hand.Paper),
                "A Z" => new Round(Hand.Rock, Hand.Scissors),
                "B Z" => new Round(Hand.Paper, Hand.Scissors),
                "C Z" => new Round(Hand.Scissors, Hand.Scissors),
                _ => throw new ArgumentOutOfRangeException($"Unknown input: {x}")
            });

        var answer = strategies.Select(ScoreRound).Sum();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    [TestCase("Day02/Sample.txt", 12, TestName = "Day 02 - Part 2 - Sample")]
    [TestCase("Day02/Puzzle Input.txt", 10398, TestName = "Day 02 - Part 2 - Puzzle Input")]
    public void Part2(string inputFile, int expected)
    {
        var strategies = File.ReadAllLines(inputFile)
            .Select(x => x switch
            {
                "A X" => new Round(Hand.Rock, Hand.Scissors),
                "B X" => new Round(Hand.Paper, Hand.Rock),
                "C X" => new Round(Hand.Scissors, Hand.Paper),
                "A Y" => new Round(Hand.Rock, Hand.Rock),
                "B Y" => new Round(Hand.Paper, Hand.Paper),
                "C Y" => new Round(Hand.Scissors, Hand.Scissors),
                "A Z" => new Round(Hand.Rock, Hand.Paper),
                "B Z" => new Round(Hand.Paper, Hand.Scissors),
                "C Z" => new Round(Hand.Scissors, Hand.Rock),
                _ => throw new ArgumentOutOfRangeException($"Unknown input: {x}")
            });

        var answer = strategies.Select(ScoreRound).Sum();

        Console.WriteLine($"{TestContext.CurrentContext.Test.Name} - {answer}");
        answer.ShouldBe(expected);
    }

    private static int ScoreRound(Round round)
    {
        var score = 0;

        score += round.Us switch
        {
            Hand.Rock => 1,
            Hand.Paper => 2,
            Hand.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException($"Unknown Hand: {round.Us}")
        };

        if (round.Opponent == round.Us)
            score += 3;

        if ((round.Us == Hand.Rock && round.Opponent == Hand.Scissors) ||
            (round.Us == Hand.Scissors && round.Opponent == Hand.Paper) ||
            (round.Us == Hand.Paper && round.Opponent == Hand.Rock))
        {
            score += 6;
        }

        return score;
    }

    private record Round(Hand Opponent, Hand Us);

    private enum Hand
    {
        Rock,
        Paper,
        Scissors
    }
}