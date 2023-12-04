using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day04;

public class Day04
{
    // https://adventofcode.com/2023/day/4
    private readonly AdventSession _session = new(2023, 04, "Scratchcards");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 13)]
    [TestCase("Puzzle Input.txt", 21_158)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var cards = Parse(input);

        var answer = cards.Select(x => x.Numbers.Where(n => x.WinningNumbers.Contains(n)))
            .Select(x => x.Count())
            .Where( x=> x > 0)
            .Select(x => Math.Pow(2, x - 1))
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 30)]
    [TestCase("Puzzle Input.txt", 6_050_769)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var cards = Parse(input);

        var cardsWithMatches =
            cards.Select(x => (x.Id, NumOfMatches: x.Numbers.Count(n => x.WinningNumbers.Contains(n))))
                .ToList();

        var copies = cardsWithMatches.ToDictionary(x => x.Id, _ => 1);

        foreach (var card in cardsWithMatches)
        {
            foreach (var id in Enumerable.Range(card.Id + 1, card.NumOfMatches))
            {
                copies[id] += copies[card.Id];
            }
        }

        var answer = copies.Values.Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private static IEnumerable<ScratchCard> Parse(string input) =>
        input.Split("\n")
            .Select(x =>
                {
                    var match = Regex.Match(x.Split(":")[0], @"\d+");
                    var id = int.Parse(match.Value);
                    var sections = x.Split(":")[1].Split("|");
                    var winningNumbers = Regex.Matches(sections[0], @"\d+").Select(x => int.Parse(x.Value));
                    var numbers = Regex.Matches(sections[1], @"\d+").Select(x => int.Parse(x.Value));
                    return new ScratchCard(id, winningNumbers.ToList(), numbers.ToList());
                })
            .ToList();

    private record ScratchCard(int Id, IReadOnlyCollection<int> WinningNumbers, IReadOnlyCollection<int> Numbers);
}
