using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Net;
using System.Diagnostics;

namespace DownloadProgressBar1
{
	public partial class minecraft : Form
	{
        string location = Path.GetTempPath();
		public minecraft()
		{
			InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker4.WorkerSupportsCancellation = true;
            backgroundWorker5.WorkerSupportsCancellation = true;
		}
        int z;
        private static void ExtractZip(string SourceFile, string TargetDir, string Password)
        {
            FastZip oZip = new FastZip();
            try
            {
                //判斷ZIP檔案是否存在
                if (File.Exists(SourceFile) == false)
                {
                    throw new Exception("要解壓縮的檔案【" + SourceFile + "】不存在，無法執行");
                }
                if (Password != "")
                {
                    oZip.Password = Password;
                }
                oZip.ExtractZip(SourceFile, TargetDir, "");
            }
            catch
            {
                throw;
            }
        }

        public static void ExtractZip(string SourceFile, string TargetDir)
        {
            ExtractZip(SourceFile, TargetDir, "");
        }

        private void x(string sUrlToReadFileFrom, string x)
        {
            int iLastIndex = sUrlToReadFileFrom.LastIndexOf('/');
            string sDownloadFileName = sUrlToReadFileFrom.Substring(iLastIndex + 1, (sUrlToReadFileFrom.Length - iLastIndex - 1));
            string sFilePathToWriteFileTo = location + "\\" + sDownloadFileName;
            // first, we need to get the exact size (in bytes) of the file we are downloading
            Uri url = new Uri(sUrlToReadFileFrom);
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            response.Close();
            // gets the size of the file in bytes
            Int64 iSize = response.ContentLength;

            // keeps track of the total bytes downloaded so we can update the progress bar
            Int64 iRunningByteTotal = 0;

            // use the webclient object to download the file
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                // open the file at the remote URL for reading
                using (System.IO.Stream streamRemote = client.OpenRead(new Uri(sUrlToReadFileFrom)))
                {
                    // using the FileStream object, we can write the downloaded bytes to the file system
                    using (Stream streamLocal = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // loop the stream and get the file into the byte buffer
                        int iByteSize = 0;
                        byte[] byteBuffer = new byte[iSize];
                        while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                        {
                            if (backgroundWorker1.CancellationPending || backgroundWorker2.CancellationPending || backgroundWorker3.CancellationPending || backgroundWorker4.CancellationPending || backgroundWorker5.CancellationPending)
                            {
                                break;
                            }
                            // write the bytes to the file system at the file path specified
                            streamLocal.Write(byteBuffer, 0, iByteSize);
                            iRunningByteTotal += iByteSize;

                            // calculate the progress out of a base "100"
                            double dIndex = (double)(iRunningByteTotal);
                            double dTotal = (double)byteBuffer.Length;
                            double dProgressPercentage = (dIndex / dTotal);
                            int iProgressPercentage = (int)(dProgressPercentage * 100);

                            // update the progress bar
                            if (x == "1")
                            {
                                backgroundWorker1.ReportProgress(iProgressPercentage);
                            }
                            if (x == "2")
                            {
                                backgroundWorker2.ReportProgress(iProgressPercentage);
                            }
                            if (x == "3")
                            {
                                backgroundWorker3.ReportProgress(iProgressPercentage);
                            }
                            if (x == "4")
                            {
                                backgroundWorker4.ReportProgress(iProgressPercentage);
                            }
                            if (x == "5")
                            {
                                backgroundWorker5.ReportProgress(iProgressPercentage);
                            }
                        }

                        // clean up the file stream
                        streamLocal.Close();
                    }

                    // close the connection to the remote server
                    streamRemote.Close();
                }
            }
        }

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
            if (!backgroundWorker1.CancellationPending)
            {
                x("http://downloads.sourceforge.net/project/partment/mc1.8.zip", "1");
                if (!backgroundWorker1.CancellationPending)
                {
                    string SourceFile = location + "\\mc1.8.zip";
                    ExtractZip(SourceFile, @"C:\", "");
                    z = 1;
                }
            }
            
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
		}

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (z == 1)
            {
                backgroundWorker2.RunWorkerAsync();
                z = 0;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!backgroundWorker2.CancellationPending)
            {
                x("http://downloads.sourceforge.net/project/partment/mcob1.zip", "2");
                if (!backgroundWorker2.CancellationPending)
                {
                    string SourceFile = location + "\\mcob1.zip";
                    ExtractZip(SourceFile, @"C:\mc\.minecraft\assets\objects", "");
                    z = 1;
                }
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (z == 1)
            {
                backgroundWorker3.RunWorkerAsync();
                z = 0;
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!backgroundWorker3.CancellationPending)
            {
                x("http://downloads.sourceforge.net/project/partment/mcob2.zip", "3");
                if (!backgroundWorker3.CancellationPending)
                {
                    string SourceFile = location + "\\mcob2.zip";
                    ExtractZip(SourceFile, @"C:\mc\.minecraft\assets\objects", "");
                    z = 1;
                }
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar3.Value = e.ProgressPercentage;
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (z == 1)
            {
                string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                WebClient wc = new WebClient();
                wc.DownloadFile("http://soside.tk/launcher1.8.lnk", des + "\\啟動Minecraft.lnk");
                MessageBox.Show("安裝完畢!將會開啟安裝目錄，已經在桌面上建立捷徑!", "完成");
                System.IO.File.Delete(location + "\\mc1.8TMI.zip");
                System.IO.File.Delete(location + "\\mc1.8.zip");
                System.IO.File.Delete(location + "\\mcob1.zip");
                System.IO.File.Delete(location + "\\mcob2.zip");
                progressBar1.Value = 0;
                progressBar2.Value = 0;
                progressBar3.Value = 0;
                label1.Visible = true;
                comboBox1.Visible = true;
                btnTestDownload.Visible = true;
                label2.Visible = false;
                button1.Visible = false;
                z = 0;
            }
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!backgroundWorker4.CancellationPending)
            {
                x("http://downloads.sourceforge.net/project/partment/mc1.8TMI.zip", "4");
                if (!backgroundWorker4.CancellationPending)
                {
                    string SourceFile = location + "\\mc1.8TMI.zip";
                    ExtractZip(SourceFile, @"C:\", "");
                    z = 1;
                }
            } 
        }

        private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (z == 1)
            {
                backgroundWorker2.RunWorkerAsync();
                z = 0;
            }
        }
        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!backgroundWorker5.CancellationPending)
            {
                x("http://downloads.sourceforge.net/project/partment/mc.zip", "5");
                if (!backgroundWorker5.CancellationPending)
                {
                    string SourceFile = location + "\\mc.zip";
                    ExtractZip(SourceFile, @"C:\", "");
                    z = 1;
                }
            }
        }

        private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (z == 1)
            {
                string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                WebClient wc = new WebClient();
                wc.DownloadFile("http://soside.tk/launcher.lnk", des + "\\啟動Minecraft.lnk");
                MessageBox.Show("安裝完畢!將會開啟安裝目錄，已經在桌面上建立捷徑~", "完成");
                System.IO.File.Delete(location + "\\mc.zip");
                progressBar1.Value = 0;
                label1.Visible = true;
                comboBox1.Visible = true;
                btnTestDownload.Visible = true;
                label2.Visible = false;
                button1.Visible = false;
                z = 0;
            }
        }

		private void btnTestDownload_Click(object sender, EventArgs e)
		{
            string version = comboBox1.Text;
            if (version != "")
            {
                string strFolderPath = @"C:\mc";
                DirectoryInfo DIFO = new DirectoryInfo(strFolderPath);
                if (DIFO.Exists)
                {
                    if (MessageBox.Show("之前已經有安裝過Minecraft\n按確定則刪除以前的版本，安裝您剛才選擇的版本\n(安裝前請先確定地圖檔、資源包都備份了)\n按取消則取消安裝", "覆蓋安裝", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        DIFO.Delete(true);
                    }
                    else
                    {
                        return;
                    }
                }
                label1.Visible = false;
                comboBox1.Visible = false;
                btnTestDownload.Visible = false;
                label2.Visible = true;
                button1.Visible = true;
                if (version == "1.8")
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                else if (version == "1.8(TMI)")
                {
                    backgroundWorker4.RunWorkerAsync();
                }
                else
                {
                    backgroundWorker5.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("請先選取版本", "錯誤");
            }
            
		}

        private void minecraft_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "確定取消安裝？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                //3.Cancel 取得或設定數值，表示是否應該取消事件。
                e.Cancel = true;
            }
            else
            {
                reset();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }


        private void reset()
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                //backgroundWorker1 = null;
            }
            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
                //backgroundWorker2 = null;
            }
            if (backgroundWorker3.IsBusy)
            {
                backgroundWorker3.CancelAsync();
                //backgroundWorker3 = null;
            }
            if (backgroundWorker4.IsBusy)
            {
                backgroundWorker4.CancelAsync();
                //backgroundWorker4 = null;
            }
            if (backgroundWorker5.IsBusy)
            {
                backgroundWorker5.CancelAsync();
                //backgroundWorker5 = null;
            }
            label1.Visible = true;
            comboBox1.Visible = true;
            btnTestDownload.Visible = true;
            label2.Visible = false;
            button1.Visible = false;
        }
	}
}
