using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DownloadProgressBar1;

namespace Test
{
    public partial class 主程式 : Form
    {
        public 主程式()
        {
            InitializeComponent();
            t.Interval = 1000;  //in milliseconds
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
        }

        Timer t = new Timer();

        string version = "R1.6.3";

        private void 主程式_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "確定關閉程式？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                //3.Cancel 取得或設定數值，表示是否應該取消事件。
                e.Cancel = true;
            }
            else
            {
                string location = Path.GetTempPath();
                System.IO.File.Delete(location + "\\mc1.8TMI.zip");
                System.IO.File.Delete(location + "\\mc1.8.zip");
                System.IO.File.Delete(location + "\\mcob1.zip");
                System.IO.File.Delete(location + "\\mcob2.zip");
                System.IO.File.Delete(location + "\\mc.zip");
                System.Environment.Exit(System.Environment.ExitCode);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool  a = checkBox1.Checked;
            bool  b = checkBox2.Checked;
            if (a)
            {
                label2.Text = "你的狀態 : 無恥";
                if (b)
                {
                    label2.Text = "你的狀態 : 變態加無恥";
                }
            }
            else
            {
                if (b)
                {
                    label2.Text = "你的狀態 : 變態";
                }
                else
                {
                    label2.Text = "你的狀態 : 無";
                }
            }
        }

        private void 主程式_Load(object sender, EventArgs e)
        {
            t.Interval = 1000;  //in milliseconds
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
            UpdateCheck.RunWorkerAsync();
            StreamWriter sw = new StreamWriter(@Application.StartupPath + "\\version.txt");
            sw.WriteLine(version);            // 寫入文字
            sw.Close();           
            System.Diagnostics.Process[] pros =
                System.Diagnostics.Process.GetProcessesByName(
                 System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            if (pros.Length > 1)
            {
                MessageBox.Show("此程式已經在執行".ToString(), "偵測");
                Application.Exit();
                return;
            }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            //get current time
            int hh = DateTime.Now.Hour;
            int mm = DateTime.Now.Minute;
            int ss = DateTime.Now.Second;

            //time
            string time = "";

            //padding leading zero
            if (hh < 10)
            {
                time += "0" + hh;
            }
            else
            {
                time += hh;
            }
            time += ":";

            if (mm < 10)
            {
                time += "0" + mm;
            }
            else
            {
                time += mm;
            }
            time += ":";

            if (ss < 10)
            {
                time += "0" + ss;
            }
            else
            {
                time += ss;
            }
            button5.Text = "當前時間:"+time;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            minecraft myDialog = new minecraft();
            DialogResult dialogResult = myDialog.ShowDialog(this);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            bool a = checkBox2.Checked;
            bool b = checkBox1.Checked;
            if (a)
            {
                label2.Text = "你的狀態 : 變態";
                if (b)
                {
                    label2.Text = "你的狀態 : 變態加無恥";
                }
            }
            else
            {
                if (b)
                {
                    label2.Text = "你的狀態 : 無恥";
                }
                else
                {
                    label2.Text = "你的狀態 : 無";
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://partment.ga");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Process notePad = new Process();
            // FileName 是要執行的檔案
            notePad.StartInfo.FileName = "Patcher.exe";
            notePad.Start();
            Application.Exit();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("程式版本 : "+version+"\n作者 : 嘎泓\n授權 : 此程式使用MIT授權條款，詳見官方網站\n此程序只做學術性研究".ToString(), "關於此程式");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.igg-games.com/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://partment.ga/update");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "確定關閉程式？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                //3.Cancel 取得或設定數值，表示是否應該取消事件。
                return;
            }
            else
            {
                Application.Exit();
            }
        }

        string updatecheckresult;

        private void UpdateCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            WebRequest myRequest = WebRequest.Create(@"https://partment.ga/mod?mod=getLatestVersion");
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream());
            updatecheckresult = sr.ReadToEnd();
        }

        private void UpdateCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (updatecheckresult != version)
            {
                button9.Text = "有新版本可供更新";
                button9.BackColor = System.Drawing.Color.Red;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("還在開發唷~","聊天室");
        }
    }
}
