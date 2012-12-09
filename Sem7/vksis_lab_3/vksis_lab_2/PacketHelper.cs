using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vksis_lab_2
{
    public class PacketHelper
    {
        public IEnumerable<char> Data { get; private set; }
        public int PacketNumber { get; set; }

        private readonly int _packetLength;

        public PacketHelper(IEnumerable<char> data, int packetLength)
        {
            Data = data;
            _packetLength = packetLength;
        }

        public List<Packet> GetPackets(bool flagMakeError)
        {
            var packets = new List<Packet>();
            PacketNumber = Data.Count() / _packetLength;
            if (Data.Count() % _packetLength != 0)
            {
                PacketNumber++;
            }

            for (int i = 0; i < PacketNumber; i++)
            {
                var packetData = new List<bool>();

                for (int j = 0; j < _packetLength; j++)
                {
                    if (i * _packetLength + j < Data.Count())
                    {
                        packetData.AddRange(GetBitsOfByte((byte)Data.ElementAt(i * _packetLength + j)));
                    }
                    else
                    {
                        packetData.AddRange(GetBitsOfByte(0));
                    }
                }

                packets.Add(new Packet(packetData, flagMakeError));
            }

            return packets;
        }

        public static List<bool> GetBitsOfByte(byte b)
        {
            var bitList = new List<bool>();
            string bits = IntToBinaryString(b);
            foreach (char bit in bits)
            {
                bitList.Add(bit != '0');
            }
            return bitList;
        }

        public static string IntToBinaryString(int num, int numWidth = 8)
        {
            var resultList = new List<char>();
            var result = new char[numWidth];
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
            var binaryForm = new StringBuilder();
            foreach (var n in resultList)
            {
                binaryForm.Append(n);
            }
            return binaryForm.ToString();
        }

        public static string BinDataToString(List<bool> data)
        {
            var stringBuilder = new StringBuilder();
            foreach (var bit in data)
            {
                stringBuilder.Append(bit ? '1' : '0');
            }
            return stringBuilder.ToString();
        }
    }
}
