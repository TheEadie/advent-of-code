using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace AdventOfCode2021
{
    public class Day16
    {
        [Test]
        public void Part1()
        {
            var binary = ParseInput();

            var pointer = 0;
            var packets = ParseBinary(binary.ToArray(), ref pointer, 1);

            var answer = AddVersions(packets);

            Console.WriteLine(answer);
            answer.ShouldBe(996);
        }

        [Test]
        public void Part2()
        {
            var binary = ParseInput();
            var pointer = 0;
            var packets = ParseBinary(binary.ToArray(), ref pointer, 1);

            var answer = Process(packets.First());

            Console.WriteLine(answer);
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

        private IEnumerable<Packet> ParseBinary(char[] binary, int pointer)
        {
            while(pointer < binary.Length)
            {
                yield return GetPacket(binary, ref pointer);
            }
        }

        private IEnumerable<Packet> ParseBinary(char[] binary, ref int pointer, int numberOfPackets)
        {
            var packets = new List<Packet>();

            for (var i = 0; i < numberOfPackets; i++)
            {
                packets.Add(GetPacket(binary, ref pointer));
            }

            return packets;
        }

        private Packet GetPacket(char[] binary, ref int pointer)
        {
            var versionBits = binary[pointer..(pointer + 3)];
            pointer += 3;
            var typeBits = binary[(pointer)..(pointer + 3)];
            pointer += 3;

            var version = Convert.ToInt32(new string(versionBits), 2);
            var typeInt = Convert.ToInt32(new string(typeBits), 2);
            var type = typeInt switch
            {
                0 => PacketType.Sum,
                1 => PacketType.Product,
                2 => PacketType.Minimum,
                3 => PacketType.Maximum,
                4 => PacketType.Literal,
                5 => PacketType.GreaterThan,
                6 => PacketType.LessThan,
                7 => PacketType.EqualTo,
                _ => throw new ArgumentException()
            };


            long? value = null;
            var subPackets = new List<Packet>();

            if (type == PacketType.Literal)
            {
                var bits = new List<char>();
                var process = true;
                while (process)
                {
                    process = binary[pointer] == '1';
                    bits.AddRange(binary[(pointer + 1)..(pointer + 5)]);
                    pointer += 5;
                }

                value = Convert.ToInt64(new string(bits.ToArray()), 2);
            }
            else
            {
                var lengthType = binary[pointer];
                pointer++;

                if (lengthType == '0') // 15 bits - total size of sub-packets
                {
                    var lengthBits = binary[pointer..(pointer + 15)];
                    pointer += 15;
                    var length = Convert.ToInt32(new string(lengthBits), 2);
                    subPackets.AddRange(ParseBinary(binary[pointer..(pointer + length)], 0));
                    pointer += length;
                }
                else // 11 bits - number of sub-packets
                {
                    var numberBits = binary[pointer..(pointer + 11)];
                    pointer += 11;
                    var number = Convert.ToInt32(new string(numberBits), 2);
                    subPackets.AddRange(ParseBinary(binary, ref pointer, number));
                }
            }

            return new Packet(version, type, value, subPackets);
        }


        private static IEnumerable<char> ParseInput()
        {
            var lines = File.ReadAllLines("Day16.txt");

            var hexString = lines[0];

            var binaryString = string.Join(string.Empty,
                hexString.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            return binaryString.ToCharArray();
        }

        private record Packet(int Version, PacketType Type, long? Value, IEnumerable<Packet> SubPackets);

        private enum PacketType { Sum, Product, Minimum, Maximum, Literal, GreaterThan, LessThan, EqualTo }
    }
}
