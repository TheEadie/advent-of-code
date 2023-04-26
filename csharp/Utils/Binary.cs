namespace Utils;

public class Binary
{
    private readonly char[] _bits;

    public Binary(string binaryString) => _bits = binaryString.ToCharArray();

    public Binary(char[] binaryCharArray) => _bits = binaryCharArray;

    public int Length => _bits.Length;

    public char GetBit(int position) => _bits[position];

    public int ToInt() => Convert.ToInt32(new string(_bits), 2);

    public Binary Invert() =>
        new(new string(Enumerable.Range(0, _bits.Length).Select(x => _bits[x] == '1' ? '0' : '1')
            .ToArray()));

    public override string ToString() => new(_bits);
}
