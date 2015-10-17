using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using DownloadProgressBar1;
using System.Security.Cryptography;

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

        string version = "R1.6.4";

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

        private void 主程式_Load(object sender, EventArgs e)
        {
            t.Interval = 1000;  //in milliseconds
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();
            UpdateCheck.RunWorkerAsync();
            PatcherCheck.RunWorkerAsync();
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

        private void PatcherCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = Application.StartupPath+"\\Patcher.exe";
            string des = Application.StartupPath;
            if (File.Exists(path))
            {
                string md5 = string.Empty;
                HashAlgorithm algorithm = MD5.Create();
                byte[] Hash = algorithm.ComputeHash(File.ReadAllBytes(path));
                foreach (byte b in Hash)
                {
                    md5 += b.ToString("X2");
                }
                WebRequest myRequest = WebRequest.Create(@"https://partment.ga/mod?mod=getPatcherMD5");
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream());
                string patchermd5 = sr.ReadToEnd();
                if (md5 != patchermd5)
                {
                    button9.Text = "正在處理...";
                    button9.Enabled = false;
                    WebClient wc = new WebClient();
                    wc.DownloadFile("http://sourceforge.net/projects/partment/files/Patcher.exe", des + "\\Patcher.exe");
                    button9.Text = "檢查更新";
                    button9.Enabled = true;
                }
            }else
            {
                button9.Text = "正在處理...";
                button9.Enabled = false;
                WebClient wc = new WebClient();
                wc.DownloadFile("http://sourceforge.net/projects/partment/files/Patcher.exe", des + "\\Patcher.exe");
                button9.Text = "檢查更新";
                button9.Enabled = true;
            }
            
        }
    }
}
