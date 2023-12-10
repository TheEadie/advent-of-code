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

        var answer = hands.OrderBy(x => x, new HandComparer())
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

        var answer = hands.OrderBy(x => x, new HandComparerPart2())
            .Select((x, i) => x.Bet * (i + 1))
            .Sum();

        _session.PrintAnswer(2, answer);
        answer.ShouldBe(expected);
    }

    private class HandComparer : IComparer<Hand>
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

        public static int GetValue(char card) => card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => int.Parse(card.ToString())
        };

        public static HandType GetType(Hand hand)
        {
            var matches = hand.Cards.GroupBy(x => x).Select(x => x.Count()).ToList();
            return matches.Contains(5) ? HandType.FiveOfAKind
                : matches.Contains(4) ? HandType.FourOfAKind
                : matches.Contains(3) && matches.Contains(2) ? HandType.FullHouse
                : matches.Contains(3) ? HandType.ThreeOfAKind
                : matches.Count(x => x == 2) == 2 ? HandType.TwoPair
                : matches.Contains(2) ? HandType.Pair
                : 0;
        }
    }

    private class HandComparerPart2 : IComparer<Hand>
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

        public static int GetValue(char card) => card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 0,
            'T' => 10,
            _ => int.Parse(card.ToString())
        };

        public static HandType GetType(Hand hand)
        {
            var jokers = hand.Cards.Count(x => x == 'J');
            var matches = hand.Cards.Where(x => x != 'J').GroupBy(x => x).Select(x => x.Count()).ToList();
            return matches.Contains(5 - jokers) || jokers == 5 ? HandType.FiveOfAKind
                : matches.Contains(4 - jokers) ? HandType.FourOfAKind
                : matches.Count(x => x == 2) == 2 && jokers == 1 ||
                  matches.Contains(3) && matches.Contains(2) ? HandType.FullHouse
                : matches.Contains(3 - jokers) ? HandType.ThreeOfAKind
                : matches.Count(x => x == 2) == 2 ? HandType.TwoPair
                : matches.Contains(2 - jokers) ? HandType.Pair
                : HandType.HighCard;
        }
    }

    private enum HandType
    {
        HighCard = 0,
        Pair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
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
