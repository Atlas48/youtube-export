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
using System.Text.RegularExpressions;

namespace YoutubeExport
{
    
    public partial class Mainfrm : Form
    {
        public Mainfrm()
        {
            InitializeComponent();
            //Bitmap bmp = YoutubeExport.Properties.Resources.youtube;
            //this.Icon = Icon.FromHandle(bmp.GetHicon());
        }

        private string GetBlockContent(ref string strSearch, string strBlockStart, string strBLockEnd, bool blnIncludeSearch = false)
        {
            int iBlockStart = 0;
            int iBlockEnd = 0;
            string strResult = "";

            try  
            {
                iBlockStart = strSearch.IndexOf(strBlockStart);

                if (iBlockStart != -1)
                {
                    iBlockEnd = strSearch.IndexOf(strBLockEnd, iBlockStart + strBlockStart.Length);

                    if (iBlockEnd != -1)
                    {
                        if (blnIncludeSearch)
                        {
                            strResult = strSearch.Substring(iBlockStart, iBlockEnd - iBlockStart + strBLockEnd.Length);
                            strSearch = strSearch.Substring(iBlockEnd + strBLockEnd.Length, strSearch.Length - (iBlockEnd + strBLockEnd.Length));
                        }
                        else
                        {
                            strResult = strSearch.Substring(iBlockStart + strBlockStart.Length, iBlockEnd - (strBlockStart.Length + iBlockStart));
                            strSearch = strSearch.Substring(iBlockEnd + strBLockEnd.Length, strSearch.Length - (iBlockEnd + strBLockEnd.Length));
                        }
                    }
                    else
                    {
                        strSearch = "";
                    }
                }
                else
                {
                    strSearch = "";
                }

            }
            catch //(Exception e)
            {
                strResult = "";
                strSearch = "";
            }

            return strResult;

        }

        private void Mainfrm_Load(object sender, EventArgs e)
        {
            txtUrl.Text = "http://www.youtube.com/playlist?list=FLGYr00JNLYnkz3agksMszxA";
        }

        private delegate void SetCtrlPropDelegate(Control control, string propertyName, object propertyValue);

        public static void SetCtrlProp(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetCtrlPropDelegate(SetCtrlProp), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }

        private delegate void SetCtrlPropDelegateCollection(Control control, object item, string propertyName, object propertyValue);

        public static void SetCtrlPropCollection(Control control, object item, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetCtrlPropDelegateCollection(SetCtrlPropCollection), new object[] { control, item, propertyName, propertyValue });
            }
            else
            {
                item.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, item, new object[] { propertyValue });
            }
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            txtResults.Text = "";
            sstrip.Items[0].Text = "";
            cmdRefresh.Enabled = false;
            txtUrl.Enabled = false;
            chkSaveHtml.Enabled = false;
            chkSaveFile.Enabled = false;
            txtResults.Enabled = false;

            Thread t = new Thread(() => GetData(txtUrl.Text));
            t.IsBackground = true;
            t.Start();
        }

        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new UriBuilder(url).Uri.ToString());

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf =  br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch //(Exception exp)
            {
                buf = null;
            }

            return (buf);
        }

        public String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            if (url != "")
            {

                Byte[] _byte = this.GetImage(url);

                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

                return _sb.ToString();
            }
            else
                return "";

        }

        public void SavePlaylistAsHTML(string fileName,string ListItems)
        {
            string strHtlmDoc = "";

            if (fileName != "")
            {
                fileName = WebUtility.HtmlDecode(fileName);

                string strFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\" + fileName + ".html";

                System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, false, Encoding.UTF8);

                strHtlmDoc += "<title>" + fileName + "</title>";

                strHtlmDoc = "<!DOCTYPE html><html lang=\"en\"" + "\r\n";
                strHtlmDoc += "<ol itemscope itemtype=\"http://schema.org/VideoGallery\">";

                strHtlmDoc += "<link id=\"css-1011281340\" class=\"www-core\" rel=\"stylesheet\" href=\"http://s.ytimg.com/yts/cssbin/www-core-vflGQCv1D.css\" data-loaded=\"true\">" + "\r\n";
                strHtlmDoc += "<link id=\"css-2085296452\" class=\"www-home-c4\" rel=\"stylesheet\" href=\"http://s.ytimg.com/yts/cssbin/www-home-c4-vflkkTB-i.css\" data-loaded=\"true\">" + "\r\n";
                strHtlmDoc += "<link id=\"css-3129785590\" class=\"www-playlist-hh\" rel=\"stylesheet\" href=\"http://s.ytimg.com/yts/cssbin/www-playlist-hh-vflzM4alL.css\" data-loaded=\"true\">" + "\r\n";

                strHtlmDoc += ListItems + "\r\n";

                strHtlmDoc += "</ol>" + "\r\n";
                strHtlmDoc += "</html>" + "\r\n";

                strHtlmDoc = strHtlmDoc.Replace("\"//", "\"http://");
                strHtlmDoc = strHtlmDoc.Replace("data-thumb=\"//", "data-thumb=\"http://");
                strHtlmDoc = strHtlmDoc.Replace("\"/watch?", "\"http://www.youtube.com/watch?");

                file.WriteLine(strHtlmDoc);
                file.Close();

                System.Diagnostics.Process.Start(strFilePath);
            }
        }

        public void SavePlaylistAsText(string fileName, string strResult)
        {
            if (fileName != "")
            {
                fileName = WebUtility.HtmlDecode(fileName);

                string strFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\" + fileName + ".txt";

                System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, false, Encoding.UTF8);

                file.Write(strResult);
                file.Close();
            }
        }

        public string RemoveScriptAndStyle(string HTML)
        {
            string Pat = "<(script|style)\\b[^>]*?>.*?</\\1>";
            return Regex.Replace(HTML, Pat, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        private void GetData(string strMainURL)
        {
            string strSearchURL = "";

            string strBlock = "";
            string strListItem = "";
            string strExtrasBlock = "";
            string strVideoUrl = "";
            string strTitle = "";
            string strOwner = "";
            string strViews = "";

            string strResults = "";
            string strLine = "";
            string strdummy = "";
            string strThumbImg = "";
            string StrThumbUriSource = "";
            string StrThumbUriThumb = "";

            StringBuilder strListItems = new StringBuilder();

            bool blnResultOk;
            string result;
            int iCounter = 0;
            int iLineCounter = 0;
            HttpWebRequest myRequest;
            HttpWebResponse myResponse;
            StreamReader sr;
            string resulttmp="";
            string strPlayListName="";

            try
            {
                CookieContainer cookies = new CookieContainer();

                if (strMainURL.IndexOf("&page=") != -1)
                {
                    strMainURL = strMainURL.Substring(0, strMainURL.IndexOf("&page="));
                }
                strMainURL = new UriBuilder(strMainURL).Uri.ToString();

                if (strMainURL != "")
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\log.txt", false, Encoding.UTF8);
                    do
                    {
                        iCounter += 1;

                        strSearchURL = strMainURL + "&page=" + iCounter.ToString();
                        SetCtrlProp(txtUrl, "Text", strSearchURL);

                        //int milliseconds = 2000;
                        //Thread.Sleep(milliseconds);

                        myRequest = (HttpWebRequest)WebRequest.Create(strSearchURL);
                        myRequest.UserAgent = "Mozilla/6.0 (Windows NT 6.2; WOW64; rv:16.0.1) Gecko/20121011 Firefox/16.0.1";
                        myRequest.Method = "GET";

                        Cookie cookie = new Cookie("PREF", "f1=500000000");
                        cookie.Domain = ".youtube.com";
                        cookies.Add(cookie);

                        myRequest.CookieContainer = cookies;
                        myResponse = (HttpWebResponse)myRequest.GetResponse();
                        sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                        result = sr.ReadToEnd();
                        resulttmp = result;
                        sr.Close();
                        myResponse.Close();

                        file.WriteLine(resulttmp);

                        if (iCounter == 1) strPlayListName = GetBlockContent(ref resulttmp, "<title>", "</title>");

                        blnResultOk = result.Contains("<h3 class=\"video-title-container\">");
                        do
                        {
                            strListItem = GetBlockContent(ref result, "<li class=\"playlist-video-item", "</li>",true);
                            if (chkSaveHtml.Checked)
                            {
                                strListItem = strListItem.Replace("src=\"//", "src=\"http://");

                                StrThumbUriSource = strListItem;
                                StrThumbUriSource = GetBlockContent(ref StrThumbUriSource, "src=\"", "\"");

                                if (StrThumbUriSource != "")
                                {
                                    if (StrThumbUriSource.Substring(StrThumbUriSource.Length - 3, 3) != "gif")
                                    {
                                        strThumbImg = "data:image/png;base64," + ConvertImageURLToBase64(StrThumbUriSource);
                                        strListItem = strListItem.Replace(StrThumbUriSource, strThumbImg);
                                    }
                                    else
                                    {
                                        StrThumbUriThumb = strListItem;
                                        StrThumbUriThumb = GetBlockContent(ref StrThumbUriThumb, "data-thumb=\"//", "\"");

                                        if (StrThumbUriThumb != "")
                                        {
                                            strThumbImg = "data:image/png;base64," + ConvertImageURLToBase64(StrThumbUriThumb);
                                            strListItem = strListItem.Replace(StrThumbUriSource, strThumbImg);
                                        }
                                    }
                                }

                                strListItems.Append(strListItem);
                            }

                            strBlock = GetBlockContent(ref strListItem, "<h3 class=\"video-title-container\">", "</div>");

                            strExtrasBlock = strBlock;

                            if (strExtrasBlock != "")
                            {
                                strVideoUrl = "www.youtube.com/watch?v=" + GetBlockContent(ref strExtrasBlock, "watch?v=", "&amp");
                                strTitle = WebUtility.HtmlDecode(GetBlockContent(ref strExtrasBlock, "dir=\"ltr\">", "</"));
                                strOwner = WebUtility.HtmlDecode(GetBlockContent(ref strExtrasBlock, "dir=\"ltr\">", "</"));
                                strViews = GetBlockContent(ref strExtrasBlock, "\"video-view-count\">", "</");
                                strViews = strViews.Trim();
                                strdummy = strViews;
                                strViews = GetBlockContent(ref strdummy, "", " ");

                                strLine = iLineCounter.ToString() + "\t" + strVideoUrl + "\t" + strOwner + "\t" + strViews + "\t" + strTitle;
                                strResults += strLine + "\r\n";

                                iLineCounter += 1;
                                SetCtrlPropCollection(sstrip, sstrip.Items[0], "Text", "Reading Page " + iCounter.ToString() + " (" + iLineCounter.ToString() + " Results.)");
                            }

                        } while (strBlock != "");

                    } while (blnResultOk);

                    if (chkSaveHtml.Checked)
                    {
                        SavePlaylistAsHTML(strPlayListName,strListItems.ToString());
                    }

                    if (chkSaveFile.Checked)
                    {
                        SavePlaylistAsText(strPlayListName, strResults);
                    }

                    SetCtrlPropCollection(sstrip, sstrip.Items[0], "Text", "Completed " + iCounter.ToString() + " Pages (" + iLineCounter.ToString() + " Results.)");
                    SetCtrlProp(txtResults, "Text", strResults);
                    file.Close();
                }

                SetCtrlProp(cmdRefresh, "Enabled", true);
                SetCtrlProp(txtUrl, "Enabled", true);
                SetCtrlProp(chkSaveHtml, "Enabled", true);
                SetCtrlProp(chkSaveFile, "Enabled", true);
                SetCtrlProp(txtResults, "Enabled", true);
            }
            catch (Exception exp)
            {
                SetCtrlProp(cmdRefresh, "Enabled", true);
                SetCtrlProp(txtUrl, "Enabled", true);
                SetCtrlProp(chkSaveHtml, "Enabled", true);
                SetCtrlProp(chkSaveFile, "Enabled", true);
                MessageBox.Show(exp.Message.ToString(), Application.ProductName);
            }
        }
    }
}

