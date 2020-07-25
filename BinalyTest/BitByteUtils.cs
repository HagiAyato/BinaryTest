using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinalyTest
{
    public static class Extentions
    {
        // bool[] => byte
        public static byte BitsToByte(this IEnumerable<bool> bits)
        {
            return bits.BitsToBytes().FirstOrDefault();
        }

        // bool[] => byte[]
        public static IEnumerable<byte> BitsToBytes(this IEnumerable<bool> bits)
        {
            int i = 0;
            byte result = 0;
            foreach (var bit in bits)
            {
                // 指定桁数について1を立てる
                if (bit) result |= (byte)(1 << 7 - i);

                if (i == 7)
                {
                    // 1バイト分で出力しビットカウント初期化
                    yield return result;
                    i = 0;
                    result = 0;
                }
                else
                {
                    i++;
                }
            }
            // 8ビットに足りない部分も出力
            if (i != 0) yield return result;
        }

        // byte[] => bool[]
        public static IEnumerable<bool> BytesToBits(this IEnumerable<byte> bytes)
        {
            foreach (var bt in bytes)
            {
                for (int i = 0; i < 8; i++)
                {
                    yield return ((bt >> 7 - i) & 1) == 1;
                }
            }
        }
    }

}
