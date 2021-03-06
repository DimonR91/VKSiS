﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vksis_lab_2
{
    public class PacketHelper
    {
        private readonly List<bool> _startBits;
        public readonly byte StuffByte;
        public readonly List<char> DataToSend;
        public readonly byte DestinationAddress;
        public readonly byte SourceAddress;
        public readonly byte ControlCode;
        public readonly int DataSizeInPacket;

        public PacketHelper(byte stuffByte, IEnumerable<char> data, int dataSizeInPacket, byte destinationAddress, byte sourceAddress, byte controlCode)
        {
            StuffByte = stuffByte;
            _startBits = GetBitsOfByte(stuffByte);
            DataToSend = data.ToList();
            DestinationAddress = destinationAddress;
            SourceAddress = sourceAddress;
            DataSizeInPacket = dataSizeInPacket;
            ControlCode = controlCode;
        }

        public List<Packet> GetPackets()
        {
            List<Packet> packets = new List<Packet>();
            int numberOfPackets = DataToSend.Count / DataSizeInPacket;
            if (DataToSend.Count % DataSizeInPacket != 0)
            {
                numberOfPackets++;
            }

            for (int i = 0; i < numberOfPackets; i++)
            {
                List<bool> packetData = new List<bool>();
                // flag
                packetData.AddRange(_startBits);
                // Destination Address
                packetData.AddRange(GetBitsOfByte(DestinationAddress));
                // Source Address
                packetData.AddRange(GetBitsOfByte(SourceAddress));
                // Data
                for (int j = 0; j < DataSizeInPacket; j++)
                {
                    if (i * DataSizeInPacket + j < DataToSend.Count)
                    {
                        packetData.AddRange(GetBitsOfByte((byte)DataToSend[i * DataSizeInPacket + j]));
                    }
                    else
                    {
                        packetData.AddRange(GetBitsOfByte(0));
                    }
                }
                // Control Code
                packetData.AddRange(GetBitsOfByte(ControlCode));

                packets.Add(new Packet(packetData, StuffByte, false));
            }

            return packets;
        }

        public static List<bool> GetBitsOfByte(byte b)
        {
            List<bool> bitList = new List<bool>();
            string bits = IntToBinaryString((int)b);
            foreach (char bit in bits.ToCharArray())
            {
                bitList.Add(bit == '0' ? false : true);
            }
            return bitList;
        }

        public static string IntToBinaryString(int num, int numWidth = 8)
        {
            List<char> resultList = new List<char>();
            char[] result = new char[numWidth];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = '0';
            }
            if (num == 1 || num == 0)
            {
                result[result.Length - 1] = num.ToString()[0];
            }
            else
            {
                int i = 1;
                while (!(num == 1 || num == 0))
                {
                    result[numWidth - i++] = (num % 2).ToString()[0];
                    num /= 2;
                }
                result[numWidth - i] = (num % 2).ToString()[0];
            }

            resultList.AddRange(result);
            StringBuilder binaryForm = new StringBuilder();
            foreach (var n in resultList)
            {
                binaryForm.Append(n);
            }
            return binaryForm.ToString();
        }

        public static string BinDataToString(List<bool> data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var bit in data)
            {
                stringBuilder.Append(bit == true ? '1' : '0');
            }
            return stringBuilder.ToString();
        }
    }
}
