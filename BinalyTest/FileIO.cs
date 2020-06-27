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
            return false;
        }
    }
}
