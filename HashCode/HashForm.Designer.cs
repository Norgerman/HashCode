namespace HashCode
{
    partial class HashForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                this.omd5.Clear();
                this.osha1.Clear();
                this.dialog.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HashForm));
            this.MD5 = new System.Windows.Forms.TextBox();
            this.Path = new System.Windows.Forms.TextBox();
            this.Lpath = new System.Windows.Forms.Label();
            this.Browser = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.Start = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.Stop = new System.Windows.Forms.Button();
            this.Lmd5 = new System.Windows.Forms.Label();
            this.Lsha1 = new System.Windows.Forms.Label();
            this.Lcrc32 = new System.Windows.Forms.Label();
            this.SHA1 = new System.Windows.Forms.TextBox();
            this.CRC32 = new System.Windows.Forms.TextBox();
            this.Umd5 = new System.Windows.Forms.TextBox();
            this.Usha1 = new System.Windows.Forms.TextBox();
            this.Ucrc32 = new System.Windows.Forms.TextBox();
            this.Cmd5 = new System.Windows.Forms.Button();
            this.Csha1 = new System.Windows.Forms.Button();
            this.Ccrc32 = new System.Windows.Forms.Button();
            this.Rmd5 = new System.Windows.Forms.Label();
            this.Rsha1 = new System.Windows.Forms.Label();
            this.Rcrc32 = new System.Windows.Forms.Label();
            this.Copy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MD5
            // 
            this.MD5.Location = new System.Drawing.Point(160, 25);
            this.MD5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MD5.Name = "MD5";
            this.MD5.ReadOnly = true;
            this.MD5.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MD5.Size = new System.Drawing.Size(592, 31);
            this.MD5.TabIndex = 0;
            this.MD5.TabStop = false;
            this.MD5.WordWrap = false;
            // 
            // Path
            // 
            this.Path.Location = new System.Drawing.Point(160, 440);
            this.Path.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Size = new System.Drawing.Size(592, 31);
            this.Path.TabIndex = 0;
            this.Path.TabStop = false;
            // 
            // Lpath
            // 
            this.Lpath.AutoSize = true;
            this.Lpath.Location = new System.Drawing.Point(64, 446);
            this.Lpath.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Lpath.Name = "Lpath";
            this.Lpath.Size = new System.Drawing.Size(56, 25);
            this.Lpath.TabIndex = 2;
            this.Lpath.Text = "Path";
            // 
            // Browser
            // 
            this.Browser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Browser.Location = new System.Drawing.Point(768, 438);
            this.Browser.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Browser.Name = "Browser";
            this.Browser.Size = new System.Drawing.Size(78, 42);
            this.Browser.TabIndex = 0;
            this.Browser.Text = "...";
            this.Browser.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Browser.UseVisualStyleBackColor = true;
            this.Browser.Click += new System.EventHandler(this.Browser_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All files(*.*)|*.*";
            this.openFileDialog.Title = "OpenFile";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(192, 569);
            this.Start.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(150, 44);
            this.Start.TabIndex = 7;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(70, 510);
            this.progressBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(776, 27);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            // 
            // Stop
            // 
            this.Stop.Enabled = false;
            this.Stop.Location = new System.Drawing.Point(574, 569);
            this.Stop.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(150, 44);
            this.Stop.TabIndex = 8;
            this.Stop.Text = "Cancel";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Lmd5
            // 
            this.Lmd5.AutoSize = true;
            this.Lmd5.Location = new System.Drawing.Point(64, 60);
            this.Lmd5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Lmd5.Name = "Lmd5";
            this.Lmd5.Size = new System.Drawing.Size(57, 25);
            this.Lmd5.TabIndex = 7;
            this.Lmd5.Text = "MD5";
            // 
            // Lsha1
            // 
            this.Lsha1.AutoSize = true;
            this.Lsha1.Location = new System.Drawing.Point(64, 192);
            this.Lsha1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Lsha1.Name = "Lsha1";
            this.Lsha1.Size = new System.Drawing.Size(67, 25);
            this.Lsha1.TabIndex = 8;
            this.Lsha1.Text = "SHA1";
            // 
            // Lcrc32
            // 
            this.Lcrc32.AutoSize = true;
            this.Lcrc32.Location = new System.Drawing.Point(64, 323);
            this.Lcrc32.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Lcrc32.Name = "Lcrc32";
            this.Lcrc32.Size = new System.Drawing.Size(81, 25);
            this.Lcrc32.TabIndex = 9;
            this.Lcrc32.Text = "CRC32";
            // 
            // SHA1
            // 
            this.SHA1.Location = new System.Drawing.Point(160, 162);
            this.SHA1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.SHA1.Name = "SHA1";
            this.SHA1.ReadOnly = true;
            this.SHA1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SHA1.Size = new System.Drawing.Size(592, 31);
            this.SHA1.TabIndex = 0;
            this.SHA1.TabStop = false;
            this.SHA1.WordWrap = false;
            // 
            // CRC32
            // 
            this.CRC32.Location = new System.Drawing.Point(158, 290);
            this.CRC32.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.CRC32.Name = "CRC32";
            this.CRC32.ReadOnly = true;
            this.CRC32.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CRC32.Size = new System.Drawing.Size(592, 31);
            this.CRC32.TabIndex = 0;
            this.CRC32.TabStop = false;
            this.CRC32.WordWrap = false;
            // 
            // Umd5
            // 
            this.Umd5.Location = new System.Drawing.Point(160, 75);
            this.Umd5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Umd5.Name = "Umd5";
            this.Umd5.Size = new System.Drawing.Size(590, 31);
            this.Umd5.TabIndex = 1;
            // 
            // Usha1
            // 
            this.Usha1.Location = new System.Drawing.Point(158, 212);
            this.Usha1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Usha1.Name = "Usha1";
            this.Usha1.Size = new System.Drawing.Size(590, 31);
            this.Usha1.TabIndex = 3;
            // 
            // Ucrc32
            // 
            this.Ucrc32.Location = new System.Drawing.Point(158, 340);
            this.Ucrc32.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Ucrc32.Name = "Ucrc32";
            this.Ucrc32.Size = new System.Drawing.Size(590, 31);
            this.Ucrc32.TabIndex = 5;
            // 
            // Cmd5
            // 
            this.Cmd5.Enabled = false;
            this.Cmd5.Location = new System.Drawing.Point(768, 25);
            this.Cmd5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Cmd5.Name = "Cmd5";
            this.Cmd5.Size = new System.Drawing.Size(150, 44);
            this.Cmd5.TabIndex = 2;
            this.Cmd5.Text = "Check";
            this.Cmd5.UseVisualStyleBackColor = true;
            this.Cmd5.Click += new System.EventHandler(this.Cmd5_Click);
            // 
            // Csha1
            // 
            this.Csha1.Enabled = false;
            this.Csha1.Location = new System.Drawing.Point(768, 162);
            this.Csha1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Csha1.Name = "Csha1";
            this.Csha1.Size = new System.Drawing.Size(150, 44);
            this.Csha1.TabIndex = 4;
            this.Csha1.Text = "Check";
            this.Csha1.UseVisualStyleBackColor = true;
            this.Csha1.Click += new System.EventHandler(this.Csha1_Click);
            // 
            // Ccrc32
            // 
            this.Ccrc32.Enabled = false;
            this.Ccrc32.Location = new System.Drawing.Point(766, 290);
            this.Ccrc32.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Ccrc32.Name = "Ccrc32";
            this.Ccrc32.Size = new System.Drawing.Size(150, 44);
            this.Ccrc32.TabIndex = 6;
            this.Ccrc32.Text = "Check";
            this.Ccrc32.UseVisualStyleBackColor = true;
            this.Ccrc32.Click += new System.EventHandler(this.Ccrc32_Click);
            // 
            // Rmd5
            // 
            this.Rmd5.AutoSize = true;
            this.Rmd5.Location = new System.Drawing.Point(776, 88);
            this.Rmd5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Rmd5.Name = "Rmd5";
            this.Rmd5.Size = new System.Drawing.Size(0, 25);
            this.Rmd5.TabIndex = 10;
            // 
            // Rsha1
            // 
            this.Rsha1.AutoSize = true;
            this.Rsha1.Location = new System.Drawing.Point(776, 225);
            this.Rsha1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Rsha1.Name = "Rsha1";
            this.Rsha1.Size = new System.Drawing.Size(0, 25);
            this.Rsha1.TabIndex = 10;
            // 
            // Rcrc32
            // 
            this.Rcrc32.AutoSize = true;
            this.Rcrc32.Location = new System.Drawing.Point(776, 354);
            this.Rcrc32.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Rcrc32.Name = "Rcrc32";
            this.Rcrc32.Size = new System.Drawing.Size(0, 25);
            this.Rcrc32.TabIndex = 10;
            // 
            // Copy
            // 
            this.Copy.Enabled = false;
            this.Copy.Location = new System.Drawing.Point(386, 569);
            this.Copy.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Copy.Name = "Copy";
            this.Copy.Size = new System.Drawing.Size(150, 44);
            this.Copy.TabIndex = 11;
            this.Copy.Text = "Copy";
            this.Copy.UseVisualStyleBackColor = true;
            this.Copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // HashForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(958, 637);
            this.Controls.Add(this.Copy);
            this.Controls.Add(this.Rcrc32);
            this.Controls.Add(this.Rsha1);
            this.Controls.Add(this.Rmd5);
            this.Controls.Add(this.Ccrc32);
            this.Controls.Add(this.Csha1);
            this.Controls.Add(this.Cmd5);
            this.Controls.Add(this.Ucrc32);
            this.Controls.Add(this.Usha1);
            this.Controls.Add(this.Umd5);
            this.Controls.Add(this.Lcrc32);
            this.Controls.Add(this.Lsha1);
            this.Controls.Add(this.Lmd5);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.Lpath);
            this.Controls.Add(this.Path);
            this.Controls.Add(this.CRC32);
            this.Controls.Add(this.SHA1);
            this.Controls.Add(this.MD5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "HashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HashCode";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.HashForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.HashForm_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MD5;
        private System.Windows.Forms.TextBox Path;
        private System.Windows.Forms.Label Lpath;
        private System.Windows.Forms.Button Browser;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Label Lmd5;
        private System.Windows.Forms.Label Lsha1;
        private System.Windows.Forms.Label Lcrc32;
        private System.Windows.Forms.TextBox SHA1;
        private System.Windows.Forms.TextBox CRC32;
        private System.Windows.Forms.TextBox Umd5;
        private System.Windows.Forms.TextBox Usha1;
        private System.Windows.Forms.TextBox Ucrc32;
        private System.Windows.Forms.Button Cmd5;
        private System.Windows.Forms.Button Csha1;
        private System.Windows.Forms.Button Ccrc32;
        private System.Windows.Forms.Label Rmd5;
        private System.Windows.Forms.Label Rsha1;
        private System.Windows.Forms.Label Rcrc32;
        private System.Windows.Forms.Button Copy;
    }
}

