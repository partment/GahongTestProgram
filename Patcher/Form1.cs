using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Patcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] pros =
                System.Diagnostics.Process.GetProcessesByName(
                 System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            if (pros.Length > 1)
            {
                System.Environment.Exit(System.Environment.ExitCode);
                return;
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            download myDialog = new download();
            DialogResult dialogResult = myDialog.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process notePad = new Process();
            // FileName 是要執行的檔案
            notePad.StartInfo.FileName = "Test.exe";
            notePad.Start();
        }

        string version;
        string result;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StreamReader sa = new StreamReader(@Application.StartupPath + "\\version.txt");
            while (!sa.EndOfStream)
            {               // 每次讀取一行，直到檔尾
                version = sa.ReadLine();            // 讀取文字到 line 變數
            }
            sa.Close();
            WebRequest myRequest = WebRequest.Create(@"https://partment.ga/mod?mod=getLatestVersion");
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream());
            result = sr.ReadToEnd();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            webBrowser1.Visible = true;
            label1.Text = "最新版本：" + result + " 目前版本：" + version;
            if (result != version)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Text = "最新版";
            }
        }
    }
}
