using System;
using System.Linq;

namespace AdventOfCode2021.Utils
{
    public class Binary
    {
        private readonly char[] _bits;

        public Binary(string binaryString)
        {
            _bits = binaryString.ToCharArray();
        }

        public int Length => _bits.Length;

        public char GetBit(int position) => _bits[position];

        public int ToInt() => Convert.ToInt32(new string(_bits), 2);

        public override string ToString()
        {
            return new(_bits);
        }
    }

    public static class BinaryUtils
    {
        public static Binary Invert(Binary input) =>
            new Binary(new string(Enumerable.Range(0, input.Length).Select((x) => input.GetBit(x) == '1' ? '0' : '1').ToArray()));
    }
}
