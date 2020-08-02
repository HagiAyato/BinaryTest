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
            for (int i = 1; i <= length; i++)
            {
                if (i % byte.MaxValue == 0)
                {
                    result.Add(b);
                    result.Add(byte.MaxValue);
                }
                else if (i == length)
                {
                    // 最終文字数時点で出力
                    result.Add(b);
                    result.Add((byte)(length % byte.MaxValue));
                }
            }

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
            // 最後の圧縮結果を出力
            result.AddRange(GetRunLength(b, length));
            // 圧縮後配列を作成
            byte[] resultArray = result.ToArray();
            // 圧縮前後で配列長が短い方を出力
            // 圧縮後が短い⇒先頭にbyte.MaxValue(=255(16bit))をつなげて返す。圧縮前が短いか同じ⇒先頭に0(16bit)をつなげて返す。
            return resultArray.Length < bytes.Length ? BitConverter.GetBytes(byte.MaxValue).Concat(resultArray).ToArray() : BitConverter.GetBytes(byte.MinValue).Concat(bytes).ToArray();
        }

        /// <summary>
        /// ランレングス解凍
        /// </summary>
        /// <param name="bytes">解凍前データ</param>
        /// <returns>解凍後データ</returns>
        public static byte[] Decode(byte[] bytes)
        {
            // 0-1番目の要素が0なら、1番目からbytes.Length-2個の要素をそのまま戻す。
            if (BitConverter.ToInt16(bytes.Take(2).ToArray(), 0) == 0) return bytes.ToList().GetRange(2, bytes.Length - 2).ToArray();
            // 0-1番目の要素が0でないなら解凍実行
            List<byte> result = new List<byte>();
            byte b = 0;
            // 0-1番目の要素はすでに見ているので、2番目の要素からループ
            for (int i = 2; i < bytes.Length; i++)
            {
                // 偶数番目には文字, 奇数番目にはデータ長
                if (i % 2 == 0)
                {
                    b = bytes[i];
                }
                else
                {
                    // データの長さ分バイトデータを作成し末尾に追加
                    result.AddRange(Enumerable.Repeat(b, bytes[i]));
                }
            }
            return result.ToArray();
        }
    }
}
