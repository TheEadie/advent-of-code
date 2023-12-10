namespace AdventOfCode2023.Day07;

public class Day07
{
    // https://adventofcode.com/2023/day/7
    private readonly AdventSession _session = new(2023, 07, "Camel Cards");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [TestCase("Sample.txt", 6440)]
    [TestCase("Sample-2.txt", 6592)]
    [TestCase("Puzzle Input.txt", 250_120_186)]
    public async Task Part1(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var hands = Parse(input);

        var answer = hands.OrderBy(x => x, new HandComparer(false))
            .Select((x, i) => x.Bet * (i + 1))
            .Sum();

        _session.PrintAnswer(1, answer);
        answer.ShouldBe(expected);
    }

    [TestCase("Sample.txt", 5905)]
    [TestCase("Sample-2.txt", 6839)]
    [TestCase("Puzzle Input.txt", 250_665_248)]
    public async Task Part2(string inputFile, int expected)
    {
        var input = await _session.Start(inputFile);
        var hands = Parse(input);

        var answer = hands.OrderBy(x => x, new HandComparer(true))
            .Select((x, i) => x.Bet * (i + 1))
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private class HandComparer(bool joker) : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            var xType = GetType(x);
            var yType = GetType(y);

            if (xType != yType)
            {
                return xType.CompareTo(yType);
            }

            foreach (var (xCard, yCard) in x.Cards.Zip(y.Cards))
            {
                var xValue = GetValue(xCard);
                var yValue = GetValue(yCard);

                if (xValue == yValue)
                {
                    continue;
                }

                return xValue.CompareTo(yValue);
            }

            return 0;
        }

        public int GetValue(char card) => card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => joker ? 0 : 11,
            'T' => 10,
            _ => int.Parse(card.ToString())
        };

        public int GetType(Hand hand)
        {
            var jokers = joker ? hand.Cards.Count(x => x == 'J') : 0;
            var cards = joker ? hand.Cards.Where(x => x != 'J') : hand.Cards;
            var matches = cards.GroupBy(x => x).Select(x => x.Count()).ToList();
            return matches.Contains(5 - jokers) || jokers == 5 ? 6
                : matches.Contains(4 - jokers) ? 5
                : matches.Count(x => x == 2) == 2 && jokers == 1 ||
                  matches.Contains(3) && matches.Contains(2) ? 4
                : matches.Contains(3 - jokers) ? 3
                : matches.Count(x => x == 2) == 2 ? 2
                : matches.Contains(2 - jokers) ? 1
                : 0;
        }
    }

    private record Hand(IReadOnlyList<char> Cards, int Bet);

    private static IEnumerable<Hand> Parse(string input)
    {
        foreach (var hand in input.Split("\n"))
        {
            var cards = hand.Split(" ")[0].ToList();
            var bet = int.Parse(hand.Split(" ")[1].ToString());
            yield return new Hand(cards, bet);
        }
    }
}
