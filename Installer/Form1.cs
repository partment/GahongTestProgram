using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void ExtractZip(string SourceFile, string TargetDir)
        {
            FastZip oZip = new FastZip();
            try
            {
                //判斷ZIP檔案是否存在
                if (File.Exists(SourceFile) == false)
                {
                    throw new Exception("要解壓縮的檔案【" + SourceFile + "】不存在，無法執行");
                }
                oZip.ExtractZip(SourceFile, TargetDir, "");
            }
            catch
            {
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "安裝中";
            button1.Visible = false;
            button2.Visible = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string temp = Path.GetTempPath();
            string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            WebClient wc = new WebClient();
            wc.DownloadFile("http://downloads.sourceforge.net/project/partment/release.zip", temp + "\\release.zip");
            wc.DownloadFile("http://cloud.soside.tk/gahong.lnk", des + "\\嘎泓的測試程式集.lnk");
            ExtractZip(temp + "\\release.zip", "C:\\Gahong");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Process notePad = new Process();
            // FileName 是要執行的檔案
            notePad.StartInfo.FileName = "C:\\Gahong\\Test.exe";
            notePad.Start();
            Application.Exit();
        }
    }
}
