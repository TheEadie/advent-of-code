namespace AdventOfCode2021.Day16
{
    public class Day16
    {
        private readonly AdventSession _session = new(2021, 16, "Packet Decoder");

        [OneTimeSetUp]
        public void SetUp()
        {
            _session.PrintHeading();
        }
        
        [Test]
        public async Task Part1()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var binary = ParseInput(input);
            var packets = ParseBinary(binary);

            var answer = AddVersions(packets);

            _session.PrintAnswer(1, answer);
            answer.ShouldBe(996);
        }

        [Test]
        public async Task Part2()
        {
            var input = await _session.Start("Puzzle Input.txt");
            var binary = ParseInput(input);
            var packets = ParseBinary(binary);

            var answer = Process(packets.First());

            _session.PrintAnswer(2, answer);
            answer.ShouldBe(96257984154);
        }

        private static int AddVersions(IEnumerable<Packet> packets)
        {
            return packets.Sum(packet => packet.Version + AddVersions(packet.SubPackets));
        }

        private static long Process(Packet packet)
        {
            var first = packet.SubPackets.ElementAtOrDefault(0);
            var second = packet.SubPackets.ElementAtOrDefault(1);

            return packet.Type switch
            {
                PacketType.Sum => packet.SubPackets.Sum(Process),
                PacketType.Product => packet.SubPackets.Select(Process).Aggregate((a, x) => a * x),
                PacketType.Minimum => packet.SubPackets.Min(Process),
                PacketType.Maximum => packet.SubPackets.Max(Process),
                PacketType.Literal => packet.Value!.Value,
                PacketType.GreaterThan => (Process(first) > Process(second)) ? 1 : 0,
                PacketType.LessThan => (Process(first) < Process(second)) ? 1 : 0,
                PacketType.EqualTo => (Process(first) == Process(second)) ? 1 : 0,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private IEnumerable<Packet> ParseBinary(BinaryStream binary)
        {
            var packets = new List<Packet>();
            while(binary.MorePackets())
            {
                packets.Add(GetPacket(binary));
            }
            return packets;
        }

        private IEnumerable<Packet> ParseBinary(BinaryStream binary, int numberOfPackets)
        {
            var packets = new List<Packet>();

            for (var i = 0; i < numberOfPackets; i++)
            {
                packets.Add(GetPacket(binary));
            }

            return packets;
        }

        private Packet GetPacket(BinaryStream binary)
        {
            var version = ToInt(binary.Grab(3));
            var type = (PacketType)ToInt(binary.Grab(3));

            if (type == PacketType.Literal)
            {
                var bits = new List<char>();
                var process = true;
                while (process)
                {
                    process = binary.GrabNext() == '1';
                    bits.AddRange(binary.Grab(4));
                }

                return new Packet(version, type, ToLong(bits), new List<Packet>());
            }
            else
            {
                var lengthType = binary.GrabNext();

                if (lengthType == '0') // 15 bits - total size of sub-packets
                {
                    var length = ToInt(binary.Grab(15));
                    var subPackets = ParseBinary(new BinaryStream(binary.Grab(length)));
                    return new Packet(version, type, null, subPackets);
                }
                else // 11 bits - number of sub-packets
                {
                    var number = ToInt(binary.Grab(11));
                    var subPackets = ParseBinary(binary, number);
                    return new Packet(version, type, null, subPackets);
                }
            }
        }

        private static int ToInt(IEnumerable<char> binary) => Convert.ToInt32(new string(binary.ToArray()), 2);
        private static long ToLong(IEnumerable<char> binary) => Convert.ToInt64(new string(binary.ToArray()), 2);

        private static BinaryStream ParseInput(string input)
        {
            var lines = input.Split("\n");

            var hexString = lines[0];

            var binaryString = string.Join(string.Empty,
                hexString.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            return new BinaryStream(binaryString.ToCharArray());
        }

        private record Packet(int Version, PacketType Type, long? Value, IEnumerable<Packet> SubPackets);

        private enum PacketType { Sum, Product, Minimum, Maximum, Literal, GreaterThan, LessThan, EqualTo }

        private class BinaryStream
        {
            private readonly char[] _bits;
            private int _pointer;

            public BinaryStream(char[] content)
            {
                _bits = content;
                _pointer = 0;
            }

            public char[] Grab(int numberOfBits)
            {
                var bits = _bits[_pointer..(_pointer + numberOfBits)];
                _pointer += numberOfBits;
                return bits;
            }

            public char GrabNext()
            {
                var bit = _bits[_pointer];
                _pointer += 1;
                return bit;
            }

            public bool MorePackets()
            {
                const int headerSize = 6;
                return _pointer < _bits.Length - headerSize;
            }
        }
    }
}
