using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace YoutubeExport
{

    public partial class Mainfrm : Form
    {
        Support support = new Support();
        BackgroundWorker m_oWorker;

        public Mainfrm()
        {
            //Bitmap bmp = YoutubeExport.Properties.Resources.youtube;
            //this.Icon = Icon.FromHandle(bmp.GetHicon());
            InitializeComponent();
        }
        private void Mainfrm_Load(object sender, EventArgs e)
        {
            txtUrl.Text = "http://www.youtube.com/playlist?list=FLGYr00JNLYnkz3agksMszxA";
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            tbControl.TabPages.Clear();
            sstrip.Items[0].Text = "";
            sstrip.Items[1].Text = "";
            cmdRefresh.Enabled = false;
            cmdCancel.Enabled = true;
            txtUrl.Enabled = false;
            chkSaveHtml.Enabled = false;
            chkSaveFile.Enabled = false;
            tbControl.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            m_oWorker = new BackgroundWorker();

            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            m_oWorker.ProgressChanged += new ProgressChangedEventHandler (m_oWorker_ProgressChanged);
            m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler (m_oWorker_RunWorkerCompleted);
            m_oWorker.WorkerReportsProgress = true;
            m_oWorker.WorkerSupportsCancellation = true;

            m_oWorker.RunWorkerAsync(txtUrl.Text);
        }
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            if (m_oWorker.IsBusy)
            {
                m_oWorker.CancelAsync();
            }
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                sstrip.Items[0].Text = "Cancelled.";
            }

            else if (e.Error != null)
            {
                sstrip.Items[0].Text = e.Error.Message;
            }
            else
            {
                List<YoutubePlaylist> ytPlaylists = (List<YoutubePlaylist>) e.Result;

                //List<TextBox> txtResults = new List<TextBox>();

                foreach (YoutubePlaylist ytPlaylist in ytPlaylists)
                {
                    TextBox txtBox;

                    txtBox = new TextBox();
                    txtBox.ScrollBars = ScrollBars.Both;
                    txtBox.Multiline = true;
                    txtBox.Dock = DockStyle.Fill;
                    txtBox.WordWrap = false;
                    txtBox.Font = new Font("Courier", 10);

                    txtBox.Text = ytPlaylist.Data;
                    //txtResults.Find(item => item.Text == strPlayListName)

                    if (ytPlaylist.Title!=null && ytPlaylist.Data != null)
                    {
                        //txtResults.Add(txtBox);

                        tbControl.TabPages.Add(ytPlaylist.Title, ytPlaylist.Title);
                        tbControl.TabPages[ytPlaylist.Title].Controls.Add(txtBox);
                    }
                }
            }

            this.Cursor = Cursors.Default; 
            cmdRefresh.Enabled= true;
            txtUrl.Enabled=true;
            chkSaveHtml.Enabled=true;
            chkSaveFile.Enabled=true;
            tbControl.Enabled=true;
            cmdCancel.Enabled = false;
        }
        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                reportProgress rpProgress = (reportProgress)e.UserState;

                sstrip.Items[0].Text = rpProgress.Status;
                sstrip.Items[1].Text = rpProgress.Page;
            }
        }
        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string strMainURL = e.Argument.ToString();

            e.Result = support.DoExport(e.Argument.ToString(), m_oWorker, chkSaveFile.Checked, chkSaveHtml.Checked);
        }
    }
}

