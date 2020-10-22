namespace AutoBackupTool
{
    partial class Form1
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.btnFindFiles = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnExploreFolder = new System.Windows.Forms.Button();
            this.fbdFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnExploreSaveFolder = new System.Windows.Forms.Button();
            this.txtSaveFolderPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fbdSaveFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnExploreTestFolder = new System.Windows.Forms.Button();
            this.txtTestFolderPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTestSave = new System.Windows.Forms.Button();
            this.fbdTestFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.bw1 = new System.ComponentModel.BackgroundWorker();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFindFiles
            // 
            this.btnFindFiles.Location = new System.Drawing.Point(264, 31);
            this.btnFindFiles.Name = "btnFindFiles";
            this.btnFindFiles.Size = new System.Drawing.Size(134, 23);
            this.btnFindFiles.TabIndex = 0;
            this.btnFindFiles.Text = "Find All Files And Folders";
            this.btnFindFiles.UseVisualStyleBackColor = true;
            this.btnFindFiles.Click += new System.EventHandler(this.btnFindFiles_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Location = new System.Drawing.Point(35, 31);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(173, 20);
            this.txtFolderPath.TabIndex = 1;
            this.txtFolderPath.Text = "C:\\svn";
            // 
            // btnExploreFolder
            // 
            this.btnExploreFolder.Location = new System.Drawing.Point(214, 31);
            this.btnExploreFolder.Name = "btnExploreFolder";
            this.btnExploreFolder.Size = new System.Drawing.Size(30, 23);
            this.btnExploreFolder.TabIndex = 2;
            this.btnExploreFolder.Text = "...";
            this.btnExploreFolder.UseVisualStyleBackColor = true;
            this.btnExploreFolder.Click += new System.EventHandler(this.btnExploreFolder_Click);
            // 
            // btnExploreSaveFolder
            // 
            this.btnExploreSaveFolder.Location = new System.Drawing.Point(214, 82);
            this.btnExploreSaveFolder.Name = "btnExploreSaveFolder";
            this.btnExploreSaveFolder.Size = new System.Drawing.Size(30, 23);
            this.btnExploreSaveFolder.TabIndex = 4;
            this.btnExploreSaveFolder.Text = "...";
            this.btnExploreSaveFolder.UseVisualStyleBackColor = true;
            this.btnExploreSaveFolder.Click += new System.EventHandler(this.btnExploreSaveFolder_Click);
            // 
            // txtSaveFolderPath
            // 
            this.txtSaveFolderPath.Location = new System.Drawing.Point(35, 82);
            this.txtSaveFolderPath.Name = "txtSaveFolderPath";
            this.txtSaveFolderPath.Size = new System.Drawing.Size(173, 20);
            this.txtSaveFolderPath.TabIndex = 3;
            this.txtSaveFolderPath.Text = "D:\\TestBackup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "What To Save";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Where To Save";
            // 
            // btnExploreTestFolder
            // 
            this.btnExploreTestFolder.Location = new System.Drawing.Point(214, 175);
            this.btnExploreTestFolder.Name = "btnExploreTestFolder";
            this.btnExploreTestFolder.Size = new System.Drawing.Size(30, 23);
            this.btnExploreTestFolder.TabIndex = 8;
            this.btnExploreTestFolder.Text = "...";
            this.btnExploreTestFolder.UseVisualStyleBackColor = true;
            this.btnExploreTestFolder.Click += new System.EventHandler(this.btnExploreTestFolder_Click);
            // 
            // txtTestFolderPath
            // 
            this.txtTestFolderPath.Location = new System.Drawing.Point(35, 175);
            this.txtTestFolderPath.Name = "txtTestFolderPath";
            this.txtTestFolderPath.Size = new System.Drawing.Size(173, 20);
            this.txtTestFolderPath.TabIndex = 7;
            this.txtTestFolderPath.Text = "D:\\TestUnzip";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Test If Save Is OK";
            // 
            // btnTestSave
            // 
            this.btnTestSave.Location = new System.Drawing.Point(264, 175);
            this.btnTestSave.Name = "btnTestSave";
            this.btnTestSave.Size = new System.Drawing.Size(134, 23);
            this.btnTestSave.TabIndex = 10;
            this.btnTestSave.Text = "Test Save";
            this.btnTestSave.UseVisualStyleBackColor = true;
            this.btnTestSave.Click += new System.EventHandler(this.btnTestSave_Click);
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(99, 322);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(565, 23);
            this.pb1.TabIndex = 11;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(111, 297);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(24, 13);
            this.lblProgress.TabIndex = 12;
            this.lblProgress.Text = "0/0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.pb1);
            this.Controls.Add(this.btnTestSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExploreTestFolder);
            this.Controls.Add(this.txtTestFolderPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExploreSaveFolder);
            this.Controls.Add(this.txtSaveFolderPath);
            this.Controls.Add(this.btnExploreFolder);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.btnFindFiles);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFindFiles;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnExploreFolder;
        private System.Windows.Forms.FolderBrowserDialog fbdFolder;
        private System.Windows.Forms.Button btnExploreSaveFolder;
        private System.Windows.Forms.TextBox txtSaveFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog fbdSaveFolder;
        private System.Windows.Forms.Button btnExploreTestFolder;
        private System.Windows.Forms.TextBox txtTestFolderPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTestSave;
        private System.Windows.Forms.FolderBrowserDialog fbdTestFolder;
        private System.ComponentModel.BackgroundWorker bw1;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.Label lblProgress;
    }
}

