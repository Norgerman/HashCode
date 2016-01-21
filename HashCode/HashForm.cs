using CRC32;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

namespace HashCode
{
    public partial class HashForm : Form
    {
        private const int BUFFER_SIZE = 1048576;
        private DialogForm dialog;
        private delegate void Invokehandle();
        private string filename;
        private string MD5Value;
        private string SHA1Value;
        private string CRC32Value;
        private string timeString;
        private StringBuilder copytext;
        private FileStream input;
        private SHA1CryptoServiceProvider osha1 = new SHA1CryptoServiceProvider();
        private MD5CryptoServiceProvider omd5 = new MD5CryptoServiceProvider();
        private CRC32Provider ocrc32 = new CRC32Provider();

        public HashForm()
        {
            InitializeComponent();
            this.copytext = new StringBuilder();
            dialog = new DialogForm();
            dialog.Owner = this;
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            filename = this.openFileDialog.FileName;
            this.openFileDialog.FileName = "";
            this.Path.Text = filename;
        }

        private void Browser_Click(object sender, EventArgs e)
        {
            filename = "";
            this.Path.Text = "";
            this.openFileDialog.ShowDialog();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string temp;
            byte[] buffer = new byte[BUFFER_SIZE];
            int len;
            long tlen;
            long clen = 0;
            Stopwatch watch = new Stopwatch();

            input = new FileStream(filename, FileMode.Open, FileAccess.Read);
            tlen = input.Length;

            copytext.AppendFormat("File: {0}\r\n", filename);
            copytext.AppendFormat("Size: {0} Bytes\r\n", tlen);
            copytext.AppendFormat("Modified: {0}\r\n", File.GetLastWriteTime(filename));

            watch.Start();
            try
            {
                while ((len = input.Read(buffer, 0, BUFFER_SIZE)) > 0)
                {
                    if (!this.backgroundWorker.CancellationPending)
                    {
                        if (tlen > input.Position)
                        {
                            omd5.TransformBlock(buffer, 0, len, buffer, 0);
                            osha1.TransformBlock(buffer, 0, len, buffer, 0);
                            ocrc32.TransformBlock(buffer, 0, len);
                        }
                        else
                        {

                            omd5.TransformFinalBlock(buffer, 0, len);
                            osha1.TransformFinalBlock(buffer, 0, len);
                            ocrc32.TransformFinalBlock(buffer, 0, len);
                        }

                        clen += len;
                        backgroundWorker.ReportProgress((int)((double)clen / tlen * 100));
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (clen == 0)
                {
                    omd5.TransformFinalBlock(new byte[0], 0, len);
                    osha1.TransformFinalBlock(new byte[0], 0, len);
                    ocrc32.TransformFinalBlock(new byte[0], 0, len);
                    backgroundWorker.ReportProgress(100);
                }

                watch.Stop();
                copytext.AppendFormat("Time used: {0} ms\r\n", watch.ElapsedMilliseconds);
                timeString = $"{watch.ElapsedMilliseconds} ms";

                temp = BitConverter.ToString(omd5.Hash);
                temp = temp.Replace("-", "");
                MD5Value = temp;
                copytext.AppendFormat("MD5: {0}\r\n", MD5Value);

                temp = BitConverter.ToString(osha1.Hash);
                temp = temp.Replace("-", "");
                SHA1Value = temp;
                copytext.AppendFormat("SHA1: {0}\r\n", SHA1Value);

                CRC32Value = string.Format("{0,8:X8}", ocrc32.Hash);
                copytext.AppendFormat("CRC32: {0}\r\n", CRC32Value);
            }
            catch (Exception ex)
            {
                this.Invoke(new Invokehandle(() =>
                {
                    this.dialog.Show(ex.Message);
                }));
            }
            finally
            {
                if (watch.IsRunning)
                    watch.Stop();
                input.Close();
                temp = null;
                buffer = null;
                input = null;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Invoke(new Invokehandle(() =>
            {
                this.progressBar.Value = e.ProgressPercentage;
            }));
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Invokehandle(() =>
            {
                if (e.Cancelled)
                {
                    this.dialog.Show("Cancelled manually!");
                }
                else
                {
                    this.MD5.Text = MD5Value;
                    this.Cmd5.Enabled = true;

                    this.SHA1.Text = SHA1Value;
                    this.Csha1.Enabled = true;

                    this.CRC32.Text = CRC32Value;
                    this.Ccrc32.Enabled = true;

                    this.Copy.Enabled = true;

                    this.tb_time.Text = timeString;
                }

                omd5.Initialize();
                osha1.Initialize();
                ocrc32.Initialize();

                this.Start.Enabled = true;
                this.Stop.Enabled = false;
                this.Browser.Enabled = true;
            }));
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (filename != null && filename.Length > 0)
            {
                this.progressBar.Value = 0;

                this.Browser.Enabled = false;

                MD5Value = "";
                this.MD5.Text = "";
                this.Umd5.Text = "";
                this.Rmd5.Text = "";

                SHA1Value = "";
                this.SHA1.Text = "";
                this.Usha1.Text = "";
                this.Rsha1.Text = "";

                CRC32Value = "";
                this.CRC32.Text = "";
                this.Ucrc32.Text = "";
                this.Rcrc32.Text = "";

                this.Start.Enabled = false;
                this.Stop.Enabled = true;

                this.Cmd5.Enabled = false;
                this.Csha1.Enabled = false;
                this.Ccrc32.Enabled = false;

                this.timeString = "";
                this.tb_time.Text = "";

                copytext.Clear();
                this.Copy.Enabled = false;

                this.backgroundWorker.RunWorkerAsync();
            }
            else
            {
                this.dialog.Show("Please select a file!");
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            this.backgroundWorker.CancelAsync();
        }

        private void Cmd5_Click(object sender, EventArgs e)
        {
            string temp = this.Umd5.Text;

            if (temp != null && temp.Length > 0)
            {
                this.Rmd5.Text = (string.Equals(temp, MD5Value, StringComparison.OrdinalIgnoreCase).ToString().ToUpper());
            }
            else
            {
                this.dialog.Show("Please input a MD5 value!");
            }
        }

        private void Csha1_Click(object sender, EventArgs e)
        {
            string temp = this.Usha1.Text;

            if (temp != null && temp.Length > 0)
            {
                this.Rsha1.Text = (string.Equals(temp, SHA1Value, StringComparison.OrdinalIgnoreCase).ToString().ToUpper());
            }
            else
            {
                this.dialog.Show("Please input a SHA1 value!");
            }
        }

        private void Ccrc32_Click(object sender, EventArgs e)
        {
            string temp = this.Ucrc32.Text;

            if (temp != null && temp.Length > 0)
            {
                this.Rcrc32.Text = (string.Equals(temp, CRC32Value, StringComparison.OrdinalIgnoreCase)).ToString().ToUpper();
            }
            else
            {
                this.dialog.Show("Please input a CRC32 value!");
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(copytext.ToString(), true);

            this.dialog.Show("Copy to clipboard succeed!");
        }

        private void HashForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void HashForm_DragDrop(object sender, DragEventArgs e)
        {
            this.filename = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (File.GetAttributes(this.filename) == FileAttributes.Directory)
            {
                this.dialog.Show("Not a File");
                this.filename = "";
                this.Path.Text = "";
            }
            else
            {
                this.Path.Text = this.filename;
            }

        }

    }
}
