using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;

namespace YoutubeExport
{
    [DataContract]
    internal class YoutubeJsonObject
    {
        [DataMember]
        public string load_more_widget_html { get; set; }
        [DataMember]
        public string content_html { get; set; }
    }

    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                //FreeConsole();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Mainfrm());
            }
            else
            {
                bool saveTxt;
                bool saveHTML;
                string strUrl;

                Support support = new Support();

                saveTxt = args.Contains("-t");
                saveHTML = args.Contains("-h");

                strUrl = args[0];

                //AttachConsole(0x0ffffffff);

                support.DoExport(strUrl, null, saveTxt, saveHTML);

                //SendKeys.SendWait("{ENTER}"); 
                //FreeConsole();
                //System.Environment.Exit(0);            

            }

        }
    }

    public class YoutubePlaylist
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }

        public YoutubePlaylist()
        {
        }

        public YoutubePlaylist(string Url)
        {
            this.Url = Url;
        }
    };

    public class reportProgress
    {
        public string Status { get; set; }
        public string Page { get; set; }

        public reportProgress(string Status, string Page)
        {
            this.Status = Status;
            this.Page = Page;
        }
    };

    public class Support
    {

        public string RemoveScriptAndStyle(string HTML)
        {
            string Pat = "<(script|style)\\b[^>]*?>.*?</\\1>";
            return Regex.Replace(HTML, Pat, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public void SavePlaylistAsHTML(string fileName, string ListItems)
        {
            string strHtlmDoc = "";

            if (fileName != "")
            {
                fileName = WebUtility.HtmlDecode(fileName);

                fileName = new Regex("[\\/:*?\"'<>|]").Replace(fileName, "");

                string strFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\" + fileName + ".html";

                System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, false, Encoding.UTF8);

                file.WriteLine("<!DOCTYPE html>");

                file.WriteLine("<html lang=\"en\">");

                file.WriteLine("<head>");
                file.WriteLine("<title>" + fileName + "</title>");
                file.WriteLine("<link href=\"http://s.ytimg.com/yts/cssbin/www-core-vflqJi9JP.css\" rel=\"stylesheet\">");
                file.WriteLine("<link href=\"http://s.ytimg.com/yts/cssbin/www-home-c4-vfljtKkXJ.css\" rel=\"stylesheet\">");
                file.WriteLine("</head>");

                file.WriteLine("<body>");

                file.WriteLine("<ul id=\"browse-items-primary\">");
                file.WriteLine("<li>");

                file.WriteLine("<div class=\"yt-uix-dragdrop pl-video-list-editable pl-video-list\" id=\"pl-video-list\">");
                file.WriteLine("<table class=\"pl-video-table\" id=\"pl-video-table\">");
                file.WriteLine("<tbody id=\"pl-load-more-destination\">");


                string[] lstItems = ListItems.Split(new string[] { "</tr>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in lstItems)
                {
                    file.WriteLine(item.Replace("\"//", "\"http://")
                    .Replace("data-thumb=\"//", "data-thumb=\"http://")
                    .Replace("\"/watch?", "\"http://www.youtube.com/watch?"));
                }


                file.WriteLine("</tbody>");
                file.WriteLine("</table>");
                file.WriteLine("</li>");
                file.WriteLine("</ul>");
                file.WriteLine("</body>");
                file.WriteLine("</html>");

                file.WriteLine(strHtlmDoc);
                file.Close();

                //System.Diagnostics.Process.Start(strFilePath);
            }
        }

        public void SavePlaylistAsText(string fileName, string strResult)
        {
            if (fileName != "")
            {
                fileName = WebUtility.HtmlDecode(fileName);

                fileName = new Regex("[\\/:*?\"'<>|]").Replace(fileName, "");

                string strFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\" + fileName + ".txt";

                System.IO.StreamWriter file = new System.IO.StreamWriter(strFilePath, false, Encoding.UTF8);

                file.Write(strResult);
                file.Close();
            }
        }

        public string GetBlockContent(ref string strSearch, string strBlockStart, string strBLockEnd, bool blnIncludeSearch = false)
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

        public byte[] GetImage(string url)
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
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch // (Exception exp)
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

                if (_byte == null) return "";

                _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

                return _sb.ToString();
            }
            else
                return "";

        }

        public List<YoutubePlaylist> DoExport(string strMainURL, BackgroundWorker m_oWorker, bool saveTxt, bool saveHTML)
        {

            bool blnResultOk;

            string strThumbImg = "";
            string StrThumbUriSource = "";
            string StrThumbUriThumb = "";

            StringBuilder strListItems = new StringBuilder();

            string result;
            HttpWebRequest myRequest;
            HttpWebResponse myResponse;
            StreamReader sr;
            string resulttmp = "";
            string strPlayListName = "";

            List<YoutubePlaylist> ytPlaylists = new List<YoutubePlaylist>();

            CookieContainer cookies = new CookieContainer();

            if (strMainURL.IndexOf("&page=") != -1)
            {
                strMainURL = strMainURL.Substring(0, strMainURL.IndexOf("&page="));
            }

            if (strMainURL.IndexOf("playlist?list=") == -1)
            {
                myRequest = (HttpWebRequest)WebRequest.Create("http://www.youtube.com/user/" + strMainURL + "/playlists?view=1");
                myResponse = (HttpWebResponse)myRequest.GetResponse();
                sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                result = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();

                //try
                //{
                //    myRequest = (HttpWebRequest)WebRequest.Create("http://gdata.youtube.com/feeds/api/users/" + strMainURL + "/favorites?&max-results=1");
                //    myResponse = (HttpWebResponse)myRequest.GetResponse();
                //    sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                //    result += sr.ReadToEnd();
                //    sr.Close();
                //    myResponse.Close();
                //}
                //catch
                //{
                //}

                do
                {
                    blnResultOk = result.Contains("playlist?list=");

                    string strResult = "";

                    do
                    {
                        strResult = GetBlockContent(ref result, "playlist?list=", "\"", false);

                        if (strResult != "")
                        {
                            strResult = "http://www.youtube.com/playlist?list=" + strResult;

                            ytPlaylists.Add(new YoutubePlaylist(new UriBuilder(strResult).Uri.ToString()));

                        }
                    } while (strResult != "");

                } while (blnResultOk);

            }
            else
            {
                strMainURL = new UriBuilder(strMainURL).Uri.ToString();
                ytPlaylists.Add(new YoutubePlaylist(strMainURL));
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\log.txt", false, Encoding.UTF8);

            foreach (YoutubePlaylist ytPlaylist in ytPlaylists)
            {
                string strBlock = "";
                string strListItem = "";
                string strExtrasBlock = "";
                string strVideoUrl = "";
                string strTitle = "";
                string strOwner = "";
                string strViews = "";

                string strResults = "";
                string strLine = "";

                string strSearchURL = "";

                int iCounter = 0;
                int iLineCounter = 0;

                strSearchURL = ytPlaylist.Url;

                do
                {
                    iCounter += 1;

                    //int milliseconds = 2000;
                    //Thread.Sleep(milliseconds);

                    myRequest = (HttpWebRequest)WebRequest.Create(strSearchURL);
                    myRequest.UserAgent = "Mozilla/6.0 (Windows NT 6.2; WOW64; rv:16.0.1) Gecko/20121011 Firefox/16.0.1";

                    Cookie cookie = new Cookie("PREF", "f1=500000000&hl=en&gl=US");
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

                    if (iCounter == 1)
                    {
                        strPlayListName = GetBlockContent(ref resulttmp, "<h1 class=\"pl-header-title\" tabindex=\"0\">", "</h1>");
                        strPlayListName = strPlayListName.Replace("\n", "").Trim();
                        ytPlaylist.Title = strPlayListName;
                    }
                    else
                    {

                        result = HttpUtility.HtmlDecode(result);

                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(YoutubeJsonObject));
                        MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(result));

                        //result=result.Replace("\\U", "\\u");
                        stream1.Position = 0;

                        //JavaScriptSerializer oJsc = new JavaScriptSerializer();
                        //YoutubeJsonObject oJscParsed = oJsc.Deserialize <YoutubeJsonObject>(result);

                        YoutubeJsonObject oJscParsed = (YoutubeJsonObject)ser.ReadObject(stream1);

                        result = oJscParsed.content_html;
                        resulttmp = oJscParsed.load_more_widget_html;
                    }

                    blnResultOk = result.Contains("<tr class=\"pl-video yt-uix-tile");
                    do
                    {
                        strListItem = GetBlockContent(ref result, "<tr class=\"pl-video yt-uix-tile", "</tr>", true);
                        if (saveHTML)
                        {
                            strListItem = strListItem.Replace("src=\"//", "src=\"http://");

                            StrThumbUriSource = strListItem;
                            StrThumbUriSource = GetBlockContent(ref StrThumbUriSource, "src=\"", "\"");

                            if (StrThumbUriSource != "")
                            {
                                //    if (StrThumbUriSource.Substring(StrThumbUriSource.Length - 3, 3) != "gif")
                                //    {
                                //        strThumbImg = "data:image/png;base64," + ConvertImageURLToBase64(StrThumbUriSource);
                                //        strListItem = strListItem.Replace(StrThumbUriSource, strThumbImg);
                                //    }
                                //    else
                                //    {
                                StrThumbUriThumb = strListItem;
                                StrThumbUriThumb = GetBlockContent(ref StrThumbUriThumb, "data-thumb=\"", "\"");

                                if (StrThumbUriThumb != "")
                                {
                                    if (StrThumbUriThumb.Substring(0, 2) == "//") StrThumbUriThumb = "http:" + StrThumbUriThumb;

                                    strThumbImg = "data:image/png;base64," + ConvertImageURLToBase64(StrThumbUriThumb);
                                    strListItem = strListItem.Replace(StrThumbUriSource, strThumbImg);
                                }
                                //    }
                            }

                            strListItems.Append(strListItem);
                        }

                        strBlock = strListItem; // GetBlockContent(ref strListItem, "<td class=\"pl-video-title\">", "</td>");

                        strExtrasBlock = strBlock;

                        if (strExtrasBlock != "")
                        {
                            strVideoUrl = "www.youtube.com/watch?v=" + GetBlockContent(ref strExtrasBlock, "dir=\"ltr\" href=\"/watch?v=", "&amp");
                            strTitle = WebUtility.HtmlDecode(GetBlockContent(ref strExtrasBlock, ">\n", "\n")).Trim();
                            strOwner = WebUtility.HtmlDecode(GetBlockContent(ref strExtrasBlock, "\" >", "</a>"));
                            strViews = GetBlockContent(ref strExtrasBlock, "timestamp\"><span aria-label=\"", "</span>");
                            strViews = strViews.Substring(strViews.IndexOf('>') + 1);

                            strLine = iLineCounter.ToString() + "\t" + strVideoUrl + "\t" + strOwner + "\t" + strViews + "\t" + strTitle;
                            strResults += strLine + "\r\n";
                            ytPlaylist.Data = strResults;

                            iLineCounter += 1;

                            if (m_oWorker != null)
                            {
                                m_oWorker.ReportProgress(0, new reportProgress("(" + strPlayListName + ") " + "Reading Page " + iCounter.ToString() + " (" + iLineCounter.ToString() + " Results.)", ytPlaylist.Url));

                                if (m_oWorker.CancellationPending)
                                {
                                    file.Close();
                                    //e.Cancel = true;
                                    if (m_oWorker != null) m_oWorker.ReportProgress(0, new reportProgress("(" + strPlayListName + ") " + "Cancelled.", ytPlaylist.Url));
                                    return ytPlaylists;
                                }
                            }
                        }

                    } while (strBlock != "");

                    strSearchURL = GetBlockContent(ref resulttmp, "data-uix-load-more-href=\"", "\"");

                    if (!string.IsNullOrWhiteSpace(strSearchURL))
                    {
                        strSearchURL = "https://www.youtube.com" + strSearchURL;
                        blnResultOk = true;
                    }
                    else
                        blnResultOk = false;

                } while (blnResultOk);

                if (saveHTML)
                {
                    SavePlaylistAsHTML(strPlayListName, strListItems.ToString());
                }

                if (saveTxt)
                {
                    SavePlaylistAsText(strPlayListName, strResults);
                }

                if (m_oWorker != null) m_oWorker.ReportProgress(100, new reportProgress("(" + strPlayListName + ") " + "Completed " + iCounter.ToString() + " Pages (" + iLineCounter.ToString() + " Results.)", ytPlaylist.Url));
            }

            file.Close();

            return ytPlaylists;
        }

    }
}
