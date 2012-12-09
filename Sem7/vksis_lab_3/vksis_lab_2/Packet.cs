using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vksis_lab_2
{

    public class Packet
    {
        public List<bool> PacketData { get; private set; }

        public Packet(List<bool> data, bool flagMakeError)
        {
            PacketData = HammingCode.EncodeData(data);
            if (flagMakeError)
            {
                int pos = new Random().Next(0, 23);
                PacketData[pos] = !PacketData[pos];
            }
        }

        public string GetEncodedData()
        {
            var stringBuilder = new StringBuilder();
            foreach(var bit in PacketData)
            {
                stringBuilder.Append(bit ? '1' : '0');
            }
            return stringBuilder.ToString();
        }

        public string GetDecodedData()
        {
            var stringBuilder = new StringBuilder();
            List<bool> decodedData = HammingCode.DecodeData(PacketData);
            for(int i = 0; i < decodedData.Count / 8; i++)
            {
                int numCharToAdd = Convert.ToInt16(PacketHelper.BinDataToString(decodedData.GetRange(i*8, 8)), 2);
                if(numCharToAdd != 0)
                {
                    stringBuilder.Append((char)numCharToAdd);
                }
            }
            return stringBuilder.ToString();
        }

        public string GetErrorInfo()
        {
            int? errorPosition = HammingCode.GetErrorPosition(PacketData);
            if(errorPosition == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("Error - {0}", errorPosition);
            }
        }
    }
}