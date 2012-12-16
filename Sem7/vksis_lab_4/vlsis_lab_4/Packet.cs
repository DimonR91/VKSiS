using System;
using System.Collections.Generic;
using System.Text;

namespace vksis_lab_4
{
    public class Packet
    {
        public List<bool> EncodedData { get; set; }
        public List<bool> DecodedData { get; set; }

        public string Message
        {
            get
            {
                var chars = new List<char>();
                for (int i = 24; i < DecodedData.Count - 8; i += 8)
                {
                    chars.Add((char)Convert.ToInt16(PacketHelper.BinDataToString(DecodedData.GetRange(i, 8)), 2));
                }
                var stringBuilder = new StringBuilder();
                foreach (char c in chars)
                {
                    if (c != 0)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder.ToString();
            }
        }

        public string StuffByte
        {
            get
            {
                return string.Format("{0:X}", Convert.ToInt16(PacketHelper.BinDataToString(DecodedData.GetRange(0, 8)), 2));
            }
        }

        public string DestinationAddress
        {
            get
            {
                return string.Format("{0:X}", Convert.ToInt16(PacketHelper.BinDataToString(DecodedData.GetRange(8, 8)), 2));
            }
        }

        public string SourceAddress
        {
            get
            {
                return string.Format("{0:X}", Convert.ToInt16(PacketHelper.BinDataToString(DecodedData.GetRange(16, 8)), 2));
            }
        }

        public string ControlCode
        {
            get
            {
                return string.Format("{0:X}", Convert.ToInt16(PacketHelper.BinDataToString(DecodedData.GetRange(DecodedData.Count - 8, 8)), 2));
            }
        }

        private readonly byte _stuffByte;

        public Packet(List<bool> data, byte stuffByte, bool isDataEncoded)
        {
            _stuffByte = stuffByte;
            if (isDataEncoded)
            {
                EncodedData = new List<bool>(data);
                DecodedData = DecodeDataForPacket(data, stuffByte);
            }
            else
            {
                DecodedData = new List<bool>(data);
                EncodedData = EncodeDataForPacket(data, stuffByte);
            }
        }

        public static List<bool> EncodeDataForPacket(List<bool> data, byte stuffByte)
        {
            List<bool> startByte = PacketHelper.GetBitsOfByte(stuffByte);
            List<bool> encodedData = new List<bool>(data);

            for (int i = 8; i < encodedData.Count - 7; i++)
            {
                if (PacketHelper.BinDataToString(encodedData.GetRange(i, 8)) == PacketHelper.BinDataToString(startByte))
                {
                    encodedData.Insert(i + 6, true);
                    i += 7;
                }
            }

            return encodedData;
        }

        public static List<bool> DecodeDataForPacket(List<bool> data, byte stuffByte)
        {
            List<bool> startByte = PacketHelper.GetBitsOfByte(stuffByte);

            List<bool> changeByte = new List<bool>(startByte);
            changeByte.Insert(startByte.Count - 1, true);

            List<bool> decodedData = new List<bool>(data);

            for (int i = decodedData.Count - 9; i >= 0; i--)
            {
                if (PacketHelper.BinDataToString(decodedData.GetRange(i, 9)) == PacketHelper.BinDataToString(changeByte))
                {
                    decodedData.RemoveRange(i, 9);
                    decodedData.InsertRange(i, startByte);
                }
            }

            return decodedData;
        }

        public static List<Packet> GetPackets(List<bool> data, byte stuffByte)
        {
            List<bool> startByte = PacketHelper.GetBitsOfByte(stuffByte);
            List<Packet> packets = new List<Packet>();
            int startIdxPacket = 0;
            bool wasDataStart = false;

            for (int i = 0; i < data.Count - 7; i++)
            {
                if (PacketHelper.BinDataToString(data.GetRange(i, 8)) == PacketHelper.BinDataToString(startByte))
                {
                    if (wasDataStart)
                    {
                        packets.Add(new Packet(data.GetRange(startIdxPacket, i - startIdxPacket), stuffByte, true));
                        startIdxPacket = i;
                    }
                    else
                    {
                        startIdxPacket = i;
                        wasDataStart = true;
                    }
                }
            }
            packets.Add(new Packet(data.GetRange(startIdxPacket, data.Count - startIdxPacket), stuffByte, true));

            return packets;
        }
    }
}