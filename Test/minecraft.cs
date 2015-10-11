using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;

namespace DownloadProgressBar1
{
	public partial class minecraft : Form
	{
        string location = Path.GetTempPath();
		public minecraft()
		{
			InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
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

        private void x(string sUrlToReadFileFrom)
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
                            if (backgroundWorker1.CancellationPending)
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
                                backgroundWorker1.ReportProgress(iProgressPercentage);
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
                x("http://sourceforge.net/projects/partment/files/pack.zip");
                if (!backgroundWorker1.CancellationPending)
                {
                    string SourceFile = location + "\\pack.zip";
                    Directory.CreateDirectory(@"C:\ptminecraft");
                    ExtractZip(SourceFile, @"C:\ptminecraft", "");
                    string des = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    WebClient wc = new WebClient();
                    wc.DownloadFile("http://sourceforge.net/projects/partment/files/minecraft.lnk", des + "\\啟動Minecraft.lnk");
                    MessageBox.Show("安裝完畢", "提示");
                }
            }
            
		}

		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
		}

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnTestDownload.Visible = true;
            label2.Visible = false;
            button1.Visible = false;
            progressBar1.Value = 0;

        }

		private void btnTestDownload_Click(object sender, EventArgs e)
		{
            string strFolderPath = @"C:\ptminecraft";
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
            btnTestDownload.Visible = false;
            label2.Visible = true;
            button1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
            
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
            reset();
        }


        private void reset()
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                //backgroundWorker1 = null;
            }
            btnTestDownload.Visible = true;
            label2.Visible = false;
            button1.Visible = false;
            progressBar1.Value = 0;
        }
    }
}
