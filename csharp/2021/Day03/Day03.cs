namespace AdventOfCode2021.Day03;

public class Day03
{
    private readonly AdventSession _session = new(2021, 3, "Binary Diagnostic");

    [OneTimeSetUp]
    public void SetUp() => _session.PrintHeading();

    [Test]
    public async Task Part1()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var binary = ParseInput(input);
        var gammaString = string.Empty;

        for (var position = 0; position < binary[0].Length; position++)
        {
            var ones = NumberOfBits(binary, '1', position);
            var zeros = NumberOfBits(binary, '0', position);
            gammaString += ones > zeros ? '1' : '0';
        }

        var gammaBinary = new Binary(gammaString);

        var gamma = gammaBinary.ToInt();
        var epsilon = gammaBinary.Invert().ToInt();

        var answer = gamma * epsilon;
        _session.PrintAnswer(1, answer);
        answer.ShouldBe(3429254);
    }

    [Test]
    public async Task Part2()
    {
        var input = await _session.Start("Puzzle Input.txt");
        var oxygenInput = ParseInput(input).ToList();

        for (var position = 0; position < oxygenInput[0].Length; position++)
        {
            var ones = NumberOfBits(oxygenInput, '1', position);
            var zeros = NumberOfBits(oxygenInput, '0', position);
            _ = oxygenInput.RemoveAll(x => x.GetBit(position) == (ones >= zeros ? '0' : '1'));

            if (oxygenInput.Count == 1)
            {
                break;
            }
        }

        var oxygen = oxygenInput[0].ToInt();

        var co2Input = ParseInput(input).ToList();

        for (var position = 0; position < co2Input[0].Length; position++)
        {
            var ones = NumberOfBits(co2Input, '1', position);
            var zeros = NumberOfBits(co2Input, '0', position);
            _ = co2Input.RemoveAll(x => x.GetBit(position) == (ones >= zeros ? '1' : '0'));

            if (co2Input.Count == 1)
            {
                break;
            }
        }

        var co2 = co2Input[0].ToInt();

        var answer = oxygen * co2;
        _session.PrintAnswer(2, answer);
        answer.ShouldBe(5410338);
    }

    private static Binary[] ParseInput(string input)
    {
        var lines = input.Split("\n");
        var characters = lines.Select(x => new Binary(x)).ToArray();
        return characters;
    }

    private static int NumberOfBits(IEnumerable<Binary> input, char bit, int position) => input.Select(x => x.GetBit(position)).Count(x => x == bit);
}
