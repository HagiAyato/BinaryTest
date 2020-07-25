using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinalyTest
{
    /// <summary>
    /// ランレングス圧縮、解凍クラス<br/>
    /// 参考というか引用元<br/>
    /// https://algoful.com/Archive/Algorithm/RLE
    /// </summary>
    class RunLength
    {

        /// <summary>
        /// 文字と連続数から圧縮結果文字列を生成
        /// </summary>
        /// <param name="b">1バイトデータ</param>
        /// <param name="length">長さ</param>
        /// <returns></returns>
        private static byte[] GetRunLength(byte b, int length)
        {
            List<byte> result = new List<byte>();
            // 残り要出力byte数
            int leave = length;
            do
            {
                // 出力(長さはlnegthと255の小さい方)
                result.Add(b);
                result.Add((byte)Math.Min(length, byte.MaxValue));
                // byte.MaxValue減算
                leave -= byte.MaxValue;
            } while (0 < leave);
            return result.ToArray();
        }

        /// <summary>
        /// ランレングス圧縮
        /// </summary>
        /// <param name="bytes">圧縮前データ</param>
        /// <returns>圧縮後データ</returns>
        public static byte[] Encode(byte[] bytes)
        {
            List<byte> result = new List<byte>();

            int length = 0;
            byte b = 0;

            // 処理本体
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i == 0)
                {
                    // 1文字目を保持
                    length = 1;
                    b = bytes[0];
                }
                else if (bytes[i] == b)
                {
                    // 直前文字と同じならカウントアップ
                    length++;
                }
                // 不一致になったら出力
                if (bytes[i] != b)
                {
                    result.AddRange(GetRunLength(b, length));

                    // 文字データ更新
                    // 1文字目を保持
                    length = 1;
                    b = bytes[i];
                }
            }
            return result.ToArray();
        }
    }
}
