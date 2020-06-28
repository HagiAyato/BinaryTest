using System;
using System.IO;
using System.Windows.Forms;

namespace BinalyTest
{

    class FileIO
    {
        /// <summary>
        /// 開くファイル名の選択
        /// </summary>
        /// <param name="nowText">現在の選択中ファイル名</param>
        /// <param name="selectedPath">選択されたファイル名</param>
        /// <returns>true:選択OK false:キャンセル時・例外時など</returns>
        internal static bool openFileSelect(string nowText, ref string selectedPath)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            // 今選択しているファイル名を初期値とする
            dialog.FileName = nowText;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedPath = dialog.FileName;
                return true;
            }
            // キャンセル時・例外時など
            return false;
        }

        /// <summary>
        /// 保存するファイル名の選択
        /// </summary>
        /// <param name="nowText">現在の選択中ファイル名</param>
        /// <param name="selectedPath">選択されたファイル名</param>
        /// <returns>true:選択OK false:キャンセル時・例外時など</returns>
        internal static bool saveFileSelect(string nowText, ref string selectedPath)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            // 今選択しているファイル名を初期値とする
            dialog.FileName = nowText;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedPath = dialog.FileName;
                return true;
            }
            // キャンセル時・例外時など
            return false;
        }

        internal static bool FileConvert(string readFilePath, string writeFilePath, int mode)
        {
            // エラーチェック1：モード未選択
            if (mode == -1)
            {
                MessageBox.Show("モードを選択してください。");
                return false;
            }
            // エラーチェック2：読み込みファイルなし
            if (!File.Exists(readFilePath))
            {
                MessageBox.Show("読み込みファイルが存在しません。\n実在するファイルを選んでください。");
                return false;
            }
            // エラーチェック3：書き込みファイル名未指定
            if (string.IsNullOrEmpty(writeFilePath))
            {
                MessageBox.Show("書き込みファイル名が空です。\nファイル名・パスを入れてください。");
                return false;
            }
            // エラーチェック4：書き込みファイルの重複
            if (File.Exists(writeFilePath))
            {
                if (MessageBox.Show("既に同名ファイルがあります。置き換えますか？", "重複確認", MessageBoxButtons.OKCancel)
                    == DialogResult.Cancel) return false;
            }
            try
            {
                switch (mode)
                {
                    case 0:
                        // テキスト⇒テキスト
                        FileWriteStr(writeFilePath, FileReadStr(readFilePath));
                        break;
                    case 1:
                        // バイナリ⇒バイナリ
                        FileWriteBin(writeFilePath, FileReadBin(readFilePath));
                        break;
                    case 2:
                        // バイナリ⇒テキスト
                        FileWriteStr(writeFilePath, BitConverter.ToString(FileReadBin(readFilePath)).Replace("-", string.Empty));
                        break;
                    case 3:
                        // テキスト⇒バイナリ
                        FileWriteBin(writeFilePath, HexStrToBin(FileReadStr(readFilePath)));
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 16進数文字列(2×n桁)⇒バイナリ文字列変換
        /// </summary>
        /// <param name="hexStr">16進数文字列(2×n桁)</param>
        /// <returns>バイナリ文字列</returns>
        private static byte[] HexStrToBin(string hexStr)
        {
            // 引数nullなら例外発生
            if (hexStr == null) throw new ArgumentNullException();
            int len = hexStr.Length;
            // 入力文字列長が偶数でないなら例外発生
            if (len % 2 != 0) throw new FormatException();
            // 長さ0なら戻り値は空
            if (len == 0) return Array.Empty<byte>();
            byte[] data = new byte[len / 2];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
            }
            return data;
        }

        /// <summary>
        /// テキストファイル読み込み
        /// </summary>
        /// <param name="readFilePath">読み込むファイルのパス</param>
        /// <returns>読み込んだデータ</returns>
        private static string FileReadStr(string readFilePath)
        {
            string data = string.Empty;
            try
            {
                if (File.Exists(readFilePath))
                {
                    using (FileStream fileStream = new FileStream(readFilePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            data = reader.ReadToEnd();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return data;
        }

        /// <summary>
        /// テキストファイル書き込み
        /// </summary>
        /// <param name="writeFilePath">書き込みむファイルのパス</param>
        /// <param name="data">書き込むデータ</param>
        /// <returns>true:処理成功 false:処理失敗</returns>
        private static bool FileWriteStr(string writeFilePath, string data)
        {
            try
            {
                using (FileStream fileStream = new FileStream(writeFilePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// バイナリファイル読み込み
        /// </summary>
        /// <param name="readFilePath">読み込むファイルのパス</param>
        /// <returns>読み込んだデータ</returns>
        private static byte[] FileReadBin(string readFilePath)
        {
            byte[] data = null;
            try
            {
                if (File.Exists(readFilePath))
                {
                    using (FileStream fileStream = new FileStream(readFilePath, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(fileStream))
                        {
                            // 読み込み上限は2147483647Byte = 2.147483647GB
                            int len = (int)fileStream.Length;
                            data = new byte[len];
                            reader.Read(data, 0, len);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return data;
        }

        /// <summary>
        /// バイナリファイル書き込み
        /// </summary>
        /// <param name="writeFilePath">書き込みむファイルのパス</param>
        /// <param name="data">書き込むデータ</param>
        /// <returns>true:処理成功 false:処理失敗</returns>
        private static bool FileWriteBin(string writeFilePath, byte[] data)
        {
            try
            {
                using (FileStream fileStream = new FileStream(writeFilePath, FileMode.Create))
                {
                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                    {
                        writer.Write(data);
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
    }
}
