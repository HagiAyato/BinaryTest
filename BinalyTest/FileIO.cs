using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinalyTest
{

    class FileIO
    {
        /// <summary>
        /// 開くファイル名の選択
        /// </summary>
        /// <param name="nowText">現在の選択中ファイル名</param>
        /// <returns>選択変更後の選択中ファイル名</returns>
        internal static string openFileSelect(string nowText)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) return dialog.FileName;
            // キャンセル時・例外時などは、今のテキストをそのまま返す
            return nowText;
        }

        /// <summary>
        /// 保存するファイル名の選択
        /// </summary>
        /// <param name="nowText">現在の選択中ファイル名</param>
        /// <returns>選択変更後の選択中ファイル名</returns>
        internal static string saveFileSelect(string nowText)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) return dialog.FileName;
            // キャンセル時・例外時などは、今のテキストをそのまま返す
            return nowText;
        }
    }
}
