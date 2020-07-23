using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinalyTest
{
    /// <summary>
    /// ハフマン暗号化、複合クラス<br/>
    /// 参考というか引用元<br/>
    /// https://algoful.com/Archive/Algorithm/HaffmanEncoding
    /// </summary>
    public class Huffman
    {
        /// <summary>
        /// ハフマン木ノード用クラス
        /// </summary>
        public class HaffmanNode
        {
            public bool IsLeaf { get; set; }
            public byte Value { get; set; }
            public int Frequency { get; set; }
            public HaffmanNode Left { get; set; }
            public HaffmanNode Right { get; set; }

            /// <summary>
            /// 末端ノード用のコンストラクタ
            /// </summary>
            /// <param name="value">ノードの値</param>
            /// <param name="frequency">出現頻度</param>
            public HaffmanNode(byte value, int frequency)
            {
                // データと出現頻度を代入
                this.Value = value;
                this.Frequency = frequency;
                this.IsLeaf = true;
            }

            /// <summary>
            /// 子ノードを持つ節を生成するためのコンストラクタ
            /// </summary>
            /// <param name="left">左子ノード</param>
            /// <param name="right">右子ノード</param>
            public HaffmanNode(HaffmanNode left, HaffmanNode right)
            {
                // 左右のノード情報を代入
                this.Left = left;
                this.Right = right;
                // 節の出現頻度は、左右ノードの出現頻度の和
                this.Frequency = left.Frequency + right.Frequency;
                this.IsLeaf = false;
            }

            /// <summary>
            /// 空のコンストラクタ(不使用?)
            /// </summary>
            public HaffmanNode() { }
        }

        /// <summary>
        /// byteの中のバイト別出現回数を集計
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private int[] getFrequency(byte[] bytes)
        {
            // 頻度配列(引数=Byteの値)
            // 要素数256個の配列を作り、各要素の値は0
            int[] frequency = Enumerable.Repeat(0, byte.MaxValue + 1).ToArray();
            // 頻度配列のb番目要素の値を加算
            foreach (byte b in bytes) frequency[b]++;
            return frequency;
        }
    }
}
