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

        private static int AddVersions(IEnumerable<Packet> packets)
        {
            return packets.Sum(packet => packet.Version + AddVersions(packet.SubPackets));
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
            var type = (typeInt == 4) ? PacketType.Literal : PacketType.Operator;


            long? value = null;
            var subPackets = new List<Packet>();

            switch (type)
            {
                case PacketType.Literal:

                    var bits = new List<char>();
                    var process = true;
                    while (process)
                    {
                        process = binary[pointer] == '1';
                        bits.AddRange(binary[(pointer + 1)..(pointer + 5)]);
                        pointer += 5;
                    }

                    value = Convert.ToInt64(new string(bits.ToArray()), 2);

                    break;
                case PacketType.Operator:

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

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Packet(version, type, value, subPackets);
        }

        [Test]
        public void Part2()
        {

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

        private enum PacketType { Literal, Operator }
    }
}
