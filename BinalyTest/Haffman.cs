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
    public class Haffman
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
                this.IsLeaf = true;
                this.Value = value;
                this.Frequency = frequency;
            }

            /// <summary>
            /// 子ノードを持つ節を生成するためのコンストラクタ
            /// </summary>
            /// <param name="left">左子ノード</param>
            /// <param name="right">右子ノード</param>
            public HaffmanNode(HaffmanNode left, HaffmanNode right)
            {
                this.IsLeaf = false;
                // 節の出現頻度は、左右ノードの出現頻度の和
                this.Frequency = left.Frequency + right.Frequency;
                // 左右のノード情報を代入
                this.Left = left;
                this.Right = right;
            }

            /// <summary>
            /// ダミーノードのコンストラクタ
            /// </summary>
            public HaffmanNode(HaffmanNode root)
            {
                this.IsLeaf = false;
                // 左右子ノードはルート
                this.Left = root;
                this.Right = root;
            }
        }

        /// <summary>
        /// byteの中のバイト別出現回数を集計
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static int[] GetFrequency(byte[] bytes)
        {
            // 頻度配列(引数=Byteの値)
            // 要素数256個の配列を作り、各要素の値は0
            int[] frequency = Enumerable.Repeat(0, byte.MaxValue + 1).ToArray();
            // 頻度配列のb番目要素の値を加算
            foreach (byte b in bytes) frequency[b]++;
            return frequency;
        }

        /// <summary>
        /// ハフマン木生成
        /// </summary>
        /// <param name="freq">頻度配列</param>
        /// <returns>ハフマン木ルートノード</returns>
        private static HaffmanNode GenerateHaffmanTree(int[] freq)
        {
            // ノードリスト
            var nodeList = new List<HaffmanNode>();
            for (int i = 0; i <= byte.MaxValue; i++)
            {
                // 頻度0であれば、ノード追加処理はいらないので次の値へ
                if (freq[i] == 0) continue;
                // ノードリスト最後尾にノード追加
                nodeList.Add(new HaffmanNode((byte)i, freq[i]));
            }

            // ノードリストの中身がのこり1件になるまでループ
            while (1 < nodeList.Count)
            {
                // 先頭に頻度最小値が来るようにソート
                // 1ループで1つノードが増えるので、どうしても毎ループでソートが必要
                nodeList = nodeList.OrderBy(e => e.Frequency).ToList();

                // 出現頻度最小のノード⇒左子ノード
                HaffmanNode left = nodeList[0];
                // 出現頻度が2番目に少ないノード⇒右子ノード
                HaffmanNode right = nodeList[1];
                // 枝にしたノードをリストから消去
                nodeList.RemoveAt(0);
                nodeList.RemoveAt(0);

                // 左右子ノードの親ノードを生成してリストに追加
                nodeList.Add(new HaffmanNode(left, right));
            }
            // キューに残っているノードがハフマン木のルート(最上部)
            HaffmanNode root = nodeList[0];

            // 1種類のデータのみ出現する場合は、ダミーノードを用意
            if (root.IsLeaf) return new HaffmanNode(root);
            return root;
        }

        /// <summary>
        /// ハフマンコード辞書を作成
        /// </summary>
        /// <param name="node">ハフマン木ルートノード</param>
        /// <returns>ハフマンコード辞書</returns>
        private static Dictionary<byte, bool[]> GetHaffmanCodeTable(HaffmanNode node)
        {
            Dictionary<byte, bool[]> dictionaly = new Dictionary<byte, bool[]>();
            for (int i = 0; i <= byte.MaxValue; i++)
            {
                byte target = (byte)i;
                dictionaly[target] = GenerateHaffmanCode(node, target);
            }
            return dictionaly;
        }

        /// <summary>
        /// ハフマンコードを作成
        /// </summary>
        /// <param name="node"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static bool[] GenerateHaffmanCode(HaffmanNode node, byte target, List<bool> codeBits = null)
        {
            // 初回呼び出しならbitリスト初期化
            if (codeBits == null) codeBits = new List<bool>();
            // 末端のノード & 現在ノードと一致
            if (node.IsLeaf && node.Value == target) return codeBits.ToArray();
            // 左側探索
            if (node.Left != null)
            {
                // 左側なので0:falseをbitリストに追加
                codeBits.Add(false);
                var result = GenerateHaffmanCode(node.Left, target, codeBits);
                if (result != null) return result;

                // 見つからないならbitリスト追加取り消し
                codeBits.RemoveAt(codeBits.Count - 1);
            }
            // 右側探索
            if (node.Right != null)
            {
                // 左側なので0:falseをbitリストに追加
                codeBits.Add(false);
                var result = GenerateHaffmanCode(node.Right, target, codeBits);
                if (result != null) return result;

                // 見つからないならbitリスト追加取り消し
                codeBits.RemoveAt(codeBits.Count - 1);
            }
            // 見つからないならnullを返す
            return null;
        }

        /// <summary>
        /// ハフマン圧縮
        /// </summary>
        /// <param name="data">圧縮前データ</param>
        /// <returns>圧縮後データ</returns>
        internal static byte[] Encode(byte[] data)
        {
            // byteの中のバイト別出現回数を集計
            // ハフマン木を作成
            HaffmanNode root = GenerateHaffmanTree(GetFrequency(data));
            // ハフマンコード表を作成
            Dictionary<byte, bool[]> haffmanDict = GetHaffmanCodeTable(root);
            // ハフマン符号化(匿名メソッド使用)
            var haffmanCode = data.Select(b => { return haffmanDict[b]; });
            return data;
        }
    }
}
