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
            public HaffmanNode()
            {

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
            if (root.IsLeaf) return new HaffmanNode() { IsLeaf = false, Left = root, Right = root };
            // 左右子ノードはルート
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

        /// ハフマンコードを作成
        /// </summary>
        /// <param name="node">ハフマン木ルートノード</param>
        /// <param name="target">コードを作成したいデータ</param>
        /// <param name="codeBits">現在のハフマンコード</param>
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
                bool[] result = GenerateHaffmanCode(node.Left, target, codeBits);
                if (result != null) return result;

                // 見つからないならbitリスト追加取り消し
                codeBits.RemoveAt(codeBits.Count - 1);
            }
            // 右側探索
            if (node.Right != null)
            {
                // 右側なので1:trueをbitリストに追加
                codeBits.Add(true);
                bool[] result = GenerateHaffmanCode(node.Right, target, codeBits);
                if (result != null) return result;

                // 見つからないならbitリスト追加取り消し
                codeBits.RemoveAt(codeBits.Count - 1);
            }
            // 見つからないならnullを返す
            return null;
        }

        /// <summary>
        /// ハフマン木自体をビット化
        /// </summary>
        /// <param name="root">ハフマン木ルートノード</param>
        /// <returns></returns>
        private static bool[] ConvertHaffmanTreeToBits(HaffmanNode root)
        {
            List<bool> bits = new List<bool>();

            // スタックを使用して幅優先探索
            Stack<HaffmanNode> stack = new Stack<HaffmanNode>();
            stack.Push(root);

            // stackが空になる=ビット化完了まで繰り返し
            while (0 < stack.Count)
            {
                HaffmanNode node = stack.Pop();
                if (node.IsLeaf)
                {
                    // 葉ノードはtrue出力
                    bits.Add(true);
                    // 実データを8bit出力
                    for (int i = 0; i < 8; i++)
                    {
                        bits.Add((node.Value >> 7 - i & 1) == 1);
                    }
                }
                else
                {
                    // 葉ノードでないならfalse出力
                    bits.Add(false);
                    // 右左の順番で子ノードを積む
                    if (node.Right != null) stack.Push(node.Right);
                    if (node.Left != null) stack.Push(node.Left);
                }
            }

            return bits.ToArray();
        }

        /// <summary>
        /// ハフマン木再構成
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        private static HaffmanNode RegenerateHaffmanTree(ref IEnumerable<bool> bits)
        {
            // 最初のビットはtrue(=葉ノードの印)か
            if (bits.First())
            {
                // 葉ノードの印を飛ばして実データ取得
                byte symbol = bits.Skip(1).BitsToByte();

                // 実データ読み飛ばし処理
                bits = bits.Skip(9);
                return new HaffmanNode() { Value = symbol, IsLeaf = true };
            }
            else
            {
                // 葉ノードの印の読み飛ばし処理
                bits = bits.Skip(1);
                return new HaffmanNode(RegenerateHaffmanTree(ref bits), RegenerateHaffmanTree(ref bits));
            }
        }

        /// <summary>
        /// ハフマン木データ、符号化したデータを結合
        /// </summary>
        /// <param name="treeBits">木コード</param>
        /// <param name="haffmanCodes">データコード</param>
        /// <returns>結合後データ</returns>
        private static IEnumerable<byte> GetAllBytes(bool[] treeBits, IEnumerable<bool[]> haffmanCodes)
        {
            int i = 0;
            // 出力byte
            byte result = 0;
            // 木コード
            foreach (bool bit in treeBits)
            {
                // 指定桁数について1を立てる
                if (bit) result |= (byte)(1 << 7 - i);
                // i=7⇒1byte分出力完了
                if (i == 7)
                {
                    // 戻り値に1bit追加
                    yield return result;
                    i = 0;
                    result = 0;
                }
                else
                {
                    i++;
                }
            }
            // データコード
            foreach (bool[] bits in haffmanCodes)
            {
                foreach (bool bit in bits)
                {
                    // 指定桁数について1を立てる
                    if (bit) result |= (byte)(1 << 7 - i);
                    // i=7⇒1byte分出力完了
                    if (i == 7)
                    {
                        // 戻り値に1bit追加
                        yield return result;
                        i = 0;
                        result = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            //8bitに足りない余りも出力
            if (i != 0) yield return result;
        }

        /// <summary>
        /// ハフマン圧縮
        /// </summary>
        /// <param name="data">圧縮前データ</param>
        /// <returns>圧縮後データ</returns>
        internal static byte[] Encode(byte[] data)
        {
            // 圧縮前データのサイズ確認(32bit=4byteの数値)
            byte[] size = BitConverter.GetBytes(data.Count());
            // byteの中のバイト別出現回数を集計
            // ハフマン木を作成
            HaffmanNode root = GenerateHaffmanTree(GetFrequency(data));
            // ハフマンコード表を作成
            Dictionary<byte, bool[]> haffmanDict = GetHaffmanCodeTable(root);
            // ハフマン符号化(匿名メソッド使用)
            IEnumerable<bool[]> haffmanCodes = data.Select(b => { return haffmanDict[b]; });
            // ハフマン木をbit化
            IEnumerable<bool> treeBits = ConvertHaffmanTreeToBits(root);
            // 書き込む全ビットデータからバイトデータ取得
            // sizeデータの後ろに取得したバイトデータを連結
            return size.Concat(GetAllBytes(treeBits.ToArray(), haffmanCodes).ToArray()).ToArray();
        }

        /// <summary>
        /// ハフマン解凍
        /// </summary>
        /// <param name="data">解凍前データ</param>
        /// <returns>解凍後データ</returns>
        internal static byte[] Decode(byte[] data)
        {
            // 圧縮済みデータ
            IEnumerable<bool> bits = data.Skip(4).BytesToBits();
            // 前4byte 圧縮前ファイルサイズ
            int size = BitConverter.ToInt32(data.Take(4).ToArray(), 0);

            // ハフマン木を再構成
            HaffmanNode root = RegenerateHaffmanTree(ref bits);
            // この時点でbitsデータから前方のハフマン木データが消えている
            
            // デコード処理本体
            // 処理済みバイト数カウンタ
            int cnt = 0;
            HaffmanNode node = root;
            List<byte> output = new List<byte>();
            foreach(bool bit in bits.ToArray())
            {
                if (node.IsLeaf)
                {
                    // 葉ノード到達
                    output.Add(node.Value);
                    //　先頭に戻る
                    node = root;
                    // カウンタが圧縮前ファイルサイズと一致したら処理終了
                    cnt++;
                    if (size <= cnt) break;
                }
                else
                {
                    // 葉ノードでない
                    if (bit)
                    {
                        // bitがtrueなら右の子ノードを探索
                        node = node.Right;
                    }
                    else
                    {
                        // bitがfalseなら左の子ノードを探索
                        node = node.Left;
                    }
                }
            }
            return output.ToArray();
        }
    }
}