using System;
using System.Collections.Generic;
using System.ComponentModel;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace Test
{
    public partial class minecraft : Form
    {
        public minecraft()
        {
            InitializeComponent();
        }
        private void minecraft_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "確定取消安裝？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
            {
                //3.Cancel 取得或設定數值，表示是否應該取消事件。
                e.Cancel = true;
                return;

            }
            string location = Application.StartupPath;
            Process notePad = new Process();
            // FileName 是要執行的檔案
            notePad.StartInfo.FileName = location + "\\Test.exe";
            notePad.Start();
            System.Environment.Exit(System.Environment.ExitCode);
        }
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

        /*public void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                }
                so.Close();
                st.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }*/

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string location = Application.StartupPath;
            // the URL to download the file from
            string sUrlToReadFileFrom = "http://cloud.soside.tk/mc1.8.zip";
            // the path to write the file to
            int iLastIndex = sUrlToReadFileFrom.LastIndexOf('/');
            string sDownloadFileName = sUrlToReadFileFrom.Substring(iLastIndex + 1, (sUrlToReadFileFrom.Length - iLastIndex - 1));
            string textBoxDestinationFolder = "D:\\";
            string sFilePathToWriteFileTo = textBoxDestinationFolder + "\\" + sDownloadFileName;

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
                            // write the bytes to the file system at the file path specified
                            streamLocal.Write(byteBuffer, 0, iByteSize);
                            iRunningByteTotal += iByteSize;

                            // calculate the progress out of a base "100"
                            double dIndex = (double)(iRunningByteTotal);
                            double dTotal = (double)byteBuffer.Length;
                            double dProgressPercentage = (dIndex / dTotal);
                            int iProgressPercentage = (int)(dProgressPercentage * 100);

                            // update the progress bar
                            backgroundWorker2.ReportProgress(iProgressPercentage);
                        }

                        // clean up the file stream
                        streamLocal.Close();
                    }

                    // close the connection to the remote server
                    streamRemote.Close();
                }
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("File download complete");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strFolderPath = @"C:\mc";
            DirectoryInfo DIFO = new DirectoryInfo(strFolderPath);
            string version = comboBox1.Text;
            if (version != "")
            {
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
                else
                {
                    Console.WriteLine("No Exist");
                }
                if (version == "1.8")
                {
                    //this.FormBorderStyle = FormBorderStyle.None;
                    button1.Visible = false;
                    label1.Visible = true;
                    label2.Visible = false;
                    comboBox1.Visible = false;
                    backgroundWorker2.RunWorkerAsync();
                    MessageBox.Show("安裝完畢!將會開啟安裝目錄，已經在桌面上建立捷徑!", "完成");
                    /*string location = Application.StartupPath;
                    button1.Visible = false;
                    string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    WebClient wc = new WebClient();
                    wc.DownloadFile("http://soside.tk/launcher1.8.lnk", des + "\\啟動Minecraft.lnk");
                    MessageBox.Show("安裝完畢!將會開啟安裝目錄，已經在桌面上建立捷徑!", "完成");
                    System.Diagnostics.Process.Start(@"C:\mc");
                    Application.Exit();
                    System.IO.File.Delete(location + "\\mc1.8.zip");
                    System.IO.File.Delete(location + "\\mcob1.zip");
                    System.IO.File.Delete(location + "\\mcob2.zip");
                    Process notePad = new Process();
                    // FileName 是要執行的檔案
                    notePad.StartInfo.FileName = location + "\\Test.exe";
                    notePad.Start();*/
                }
                if (version == "1.8(TMI)")
                {
                    //this.FormBorderStyle = FormBorderStyle.None;
                    button1.Visible = false;
                    label1.Visible = true;
                    label2.Visible = false;
                    comboBox1.Visible = false;
                    string location = Application.StartupPath;
                    //DownloadFile("http://cloud.soside.tk/mc1.8TMI.zip", location + "\\mc1.8.zip", progressBar1);
                    //DownloadFile("http://cloud.soside.tk/mcob1.zip", location + "\\mcob1.zip", progressBar2);
                    //DownloadFile("http://cloud.soside.tk/mcob2.zip", location + "\\mcob2.zip", progressBar3);
                    string SourceFile = location + "\\mc1.8.zip";
                    ExtractZip(SourceFile, @"C:\", "");
                    string SourceFile1 = location + "\\mcob1.zip";
                    ExtractZip(SourceFile1, @"C:\mc\.minecraft\assets\objects", "");
                    string SourceFile2 = location + "\\mcob2.zip";
                    ExtractZip(SourceFile2, @"C:\mc\.minecraft\assets\objects", "");
                    button1.Visible = false;
                    string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    WebClient wc = new WebClient();
                    wc.DownloadFile("http://soside.tk/launcher1.8.lnk", des + "\\啟動Minecraft.lnk");
                    MessageBox.Show("安裝完畢!將會開啟安裝目錄，已經在桌面上建立捷徑!", "完成");
                    System.Diagnostics.Process.Start(@"C:\mc");
                    Application.Exit();
                    System.IO.File.Delete(location + "\\mc1.8.zip");
                    System.IO.File.Delete(location + "\\mcob1.zip");
                    System.IO.File.Delete(location + "\\mcob2.zip");
                    Process notePad = new Process();
                    // FileName 是要執行的檔案
                    notePad.StartInfo.FileName = location + "\\Test.exe";
                    notePad.Start();
                }
                if (version == "1.7.2")
                {
                    //this.FormBorderStyle = FormBorderStyle.None;
                    button1.Visible = false;
                    label1.Visible = true;
                    label2.Visible = false;
                    comboBox1.Visible = false;
                    string location = Application.StartupPath;
                    //DownloadFile("http://cloud.soside.tk/mc.zip", location + "\\mc.zip", progressBar1);
                    string SourceFile = location + "\\mc.zip";
                    ExtractZip(SourceFile, @"C:\", "");
                    progressBar1.Visible = false;
                    button1.Visible = false;
                    string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    WebClient wc = new WebClient();
                    wc.DownloadFile("http://soside.tk/launcher.lnk", des + "\\啟動Minecraft.lnk");
                    MessageBox.Show("安裝完畢!將會開啟安裝目錄，已經在桌面上建立捷徑!", "完成");
                    System.Diagnostics.Process.Start(@"C:\mc");
                    Application.Exit();
                    System.IO.File.Delete(location + "\\mc.zip");
                    Process notePad = new Process();
                    // FileName 是要執行的檔案
                    notePad.StartInfo.FileName = location + "\\Test.exe";
                    notePad.Start();
                }
            }
            else
            {
                MessageBox.Show("請先選取版本", "錯誤");
            }
        }

        private object backgroundWorker1()
        {
            throw new NotImplementedException();
        }

    }
}
