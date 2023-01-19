namespace AdventOfCode2022.Day02;

public class Day02
{
    private readonly AdventSession _session = new(2022, 2, "Rock Paper Scissors");

    [OneTimeSetUp]
    public void SetUp()
    {
        _session.PrintHeading();
    }
    
    [TestCase("Sample.txt", 15)]
    [TestCase("Puzzle Input.txt", 13009)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        
        var strategies = input
            .Split("\n")
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

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 12)]
    [TestCase("Puzzle Input.txt", 10398)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var strategies = input
            .Split("\n")
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

        _session.PrintAnswer(2, answer);
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

        if (round is {Us: Hand.Rock, Opponent: Hand.Scissors} 
            or {Us: Hand.Scissors, Opponent: Hand.Paper} 
            or {Us: Hand.Paper, Opponent: Hand.Rock})
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