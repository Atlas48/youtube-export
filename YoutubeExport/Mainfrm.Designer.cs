namespace YoutubeExport
{
    partial class Mainfrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainfrm));
            this.sstrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkSaveHtml = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.chkSaveFile = new System.Windows.Forms.CheckBox();
            this.sstrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // sstrip
            // 
            this.sstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.sstrip.Location = new System.Drawing.Point(0, 584);
            this.sstrip.Name = "sstrip";
            this.sstrip.Size = new System.Drawing.Size(833, 22);
            this.sstrip.TabIndex = 1;
            this.sstrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkSaveFile);
            this.panel1.Controls.Add(this.chkSaveHtml);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtUrl);
            this.panel1.Controls.Add(this.cmdRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(833, 118);
            this.panel1.TabIndex = 2;
            // 
            // chkSaveHtml
            // 
            this.chkSaveHtml.AutoSize = true;
            this.chkSaveHtml.Location = new System.Drawing.Point(12, 87);
            this.chkSaveHtml.Name = "chkSaveHtml";
            this.chkSaveHtml.Size = new System.Drawing.Size(505, 17);
            this.chkSaveHtml.TabIndex = 4;
            this.chkSaveHtml.Text = "Save .html Playlist ( Experimental - Slow Saves all Favorites in one Page with em" +
                "bedded Thumbnails. )";
            this.chkSaveHtml.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(529, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter Playlist url  here (http://www.youtube.com/playlist?list=FLGYr00JNLYnkz3agk" +
                "sMszxA)";
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(12, 38);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(809, 20);
            this.txtUrl.TabIndex = 2;
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRefresh.Location = new System.Drawing.Point(734, 10);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(87, 23);
            this.cmdRefresh.TabIndex = 1;
            this.cmdRefresh.Text = "Refresh";
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtResults);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 118);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(833, 466);
            this.panel2.TabIndex = 4;
            // 
            // txtResults
            // 
            this.txtResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResults.Enabled = false;
            this.txtResults.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.txtResults.Location = new System.Drawing.Point(0, 0);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(833, 466);
            this.txtResults.TabIndex = 4;
            this.txtResults.WordWrap = false;
            // 
            // chkSaveFile
            // 
            this.chkSaveFile.AutoSize = true;
            this.chkSaveFile.Location = new System.Drawing.Point(12, 64);
            this.chkSaveFile.Name = "chkSaveFile";
            this.chkSaveFile.Size = new System.Drawing.Size(183, 17);
            this.chkSaveFile.TabIndex = 5;
            this.chkSaveFile.Text = "Save .txt ( tab Delimited text file. )";
            this.chkSaveFile.UseVisualStyleBackColor = true;
            // 
            // Mainfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 606);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.sstrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mainfrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Youtube Export";
            this.Load += new System.EventHandler(this.Mainfrm_Load);
            this.sstrip.ResumeLayout(false);
            this.sstrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip sstrip;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdRefresh;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.CheckBox chkSaveHtml;
        private System.Windows.Forms.CheckBox chkSaveFile;
    }
}

