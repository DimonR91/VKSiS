using System;
using System.Collections.Generic;
using System.Linq;

namespace vksis_lab_2
{
    public static class HammingCode
    {
        static public List<bool> EncodeData(List<bool> data)
        {
            var encodedData = new List<bool>(data.AsEnumerable());
            int N = data.Count;
            if(N > 0 && N % 2 == 0)
            {
                IEnumerable<int> degrees = GetDegrees(N);
                foreach (var degree in degrees)
                {
                    encodedData.Insert(degree - 1, false);
                }

                var controlBits = new List<bool>();
                foreach(var degree in degrees)
                {
                    int i = 0;
                    for(int j = degree - 1; j < encodedData.Count; j += 2 * degree)
                    {
                        for (int k = 0; k < degree && (j + k) < encodedData.Count; k++)
                        {
                            if (encodedData[j + k] == true)
                            {
                                i++;
                            }
                        }
                    }

                    controlBits.Add(i % 2 == 1);
                }

                for (int i = 0; i < degrees.Count(); i++)
                {
                    encodedData[degrees.ElementAt(i) - 1] = controlBits[i];
                }
            }
            return encodedData;
        }

        static private IEnumerable<int> GetDegrees(int N)
        {
            var maxDegree = (int)Math.Log(N, 2);
            var degrees = new List<int>();
            do
            {
                degrees.Add((int)Math.Pow(2, maxDegree));
                maxDegree--;
            } while (maxDegree >= 0);
            degrees.Reverse();
            return degrees;
        }

        static public List<bool> DecodeData(List<bool> data)
        {
            var dataToDecode = new List<bool>(data);

            int N = (int) Math.Log(dataToDecode.Count, 2);
            var degrees = new List<int>(GetDegrees((int)Math.Pow(2, N)).Reverse());
            foreach (var degree in degrees)
            {
                dataToDecode.RemoveAt(degree - 1);
            }

            bool flagError = false;
            List<bool> checkedData = EncodeData(dataToDecode);
            for (int i = 0; i < dataToDecode.Count; i++)
            {
                if (data[i] != checkedData[i])
                {
                    flagError = true;
                    break;
                }
            }
            
            if(flagError)
            {
                dataToDecode = CorrectData(data, checkedData);
                foreach (var degree in degrees)
                {
                    dataToDecode.RemoveAt(degree - 1);
                }
            }
            return dataToDecode;
        }

        static private List<bool> CorrectData(List<bool> dataToCorrect, List<bool> checkedData)
        {
            int errorPosition = 0;
            for(int i = 1; i <= dataToCorrect.Count; i++)
            {
                if(dataToCorrect[i - 1] != checkedData[i - 1])
                {
                    errorPosition += i;
                }
            }
            dataToCorrect[errorPosition - 1] = !dataToCorrect[errorPosition - 1];
            return dataToCorrect;
        }

        static public int? GetErrorPosition(List<bool> data)
        {
            var dataToDecode = new List<bool>(data);

            int N = (int) Math.Log(dataToDecode.Count, 2);
            var degrees = new List<int>(GetDegrees((int)Math.Pow(2, N)).Reverse());
            foreach (var degree in degrees)
            {
                dataToDecode.RemoveAt(degree - 1);
            }

            List<bool> checkedData = EncodeData(dataToDecode);
            int? errorPosition = null;
            for(int i = 1; i <= data.Count; i++)
            {
                if(data[i - 1] != checkedData[i - 1])
                {
                    if(errorPosition == null)
                    {
                        errorPosition = 0;
                    }
                    errorPosition += i;
                }
            }

            return errorPosition;
        }
    }
}
