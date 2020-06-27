using System;
using System.Windows.Forms;

namespace BinalyTest
{
    public partial class Form1 : Form
    {
        // radiobutton配列
        private RadioButton[] modeRadioBtns = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            modeRadioBtns = new RadioButton[2];
            modeRadioBtns[0] = radioButton1;
            modeRadioBtns[1] = radioButton2;
        }

        /// <summary>
        /// 読み込みファイル名選択ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            string selectedPath = textBox1.Text;
            if (FileIO.openFileSelect(textBox1.Text, ref selectedPath)) textBox1.Text = selectedPath;
            button1.Enabled = true;
        }

        /// <summary>
        /// 書き込みファイル名選択ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            string selectedPath = textBox2.Text;
            if (FileIO.saveFileSelect(textBox2.Text, ref selectedPath)) textBox2.Text = selectedPath;
            button2.Enabled = true;
        }

        /// <summary>
        /// 変換ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            // 選択されたradiobuttonのindexを取得
            int index = -1;
            for (int i = 0; i < modeRadioBtns.Length; i++)
            {
                if (modeRadioBtns[i].Checked)
                {
                    index = i;
                    break;
                }
            }
            FileIO.FileConvert(textBox1.Text, textBox2.Text, index);
            button3.Enabled = true;
        }
    }
}
