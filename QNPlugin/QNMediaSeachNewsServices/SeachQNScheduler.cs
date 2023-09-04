using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Services.Search.Controllers;
using HtmlAgilityPack;
using IISTask;
using QNMedia.CMS.Services;
using QNMedia.Office;

namespace QNMedia.Scheduler
{
    public class SeachQNScheduler : SchedulerClient
    {
        QNMedia.CMS.SearchServices.SearchController ctrl = new QNMedia.CMS.SearchServices.SearchController();
        QNMediaHelper hp = new QNMediaHelper();
        CMSController cms = new CMSController();
        public SeachQNScheduler(ScheduleHistoryItem oItem) : base()
        {
            this.ScheduleHistoryItem = oItem;
        }
        public override void DoWork()
        {
            try
            {
                this.Progressing();
                {
                    var lstseach = ctrl.CMS_ZSearch_Config_Gets().ToList();
                    if (lstseach.Count > 0)
                    {
                        this.ScheduleHistoryItem.AddLogNote("begin filter");
                        List<AutoUpdate> lst = new List<AutoUpdate>();
                        lstseach.ForEach((item) =>
                        {
                            var auto = SearchQN(item.CID, item.KeyQN,item.SearchKey);
                            if (auto != null && lst.FindLastIndex(x => x.PortalID == auto.PortalID) < 0) lst.Add(auto);
                        });
                        if (lst.Count > 0)
                        {
                            lst.ForEach(item =>
                            {
                                cms.QN_CMS_ResetCache(item.PortalID);
                            });
                        }
                    }
                    this.ScheduleHistoryItem.Succeeded = true;
                }
            }
            catch (Exception ex)
            {
                this.ScheduleHistoryItem.AddLogNote(ex.Message);
                this.ScheduleHistoryItem.Succeeded = false;
                this.Errored(ref ex);
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }

        }
        public class AutoUpdate
        {
            public int PortalID { get; set; }
        }
        public AutoUpdate SearchQN(int cm, string key,string keywork)
        {
            AutoUpdate auto = new AutoUpdate();
            QN_CMS info = new QN_CMS();
            var weburl = "https://baoquangnam.vn/";
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = web.Load(weburl + "tags/" + key + ".html");
            var listtitle = doc.DocumentNode.SelectSingleNode("//div[@class='list-3col']");
            var listitem = listtitle.SelectNodes("//h1").ToList();
            listitem.ForEach(item =>
            {
                var child = item.Element("a");
                if (child.Attributes["href"] != null)
                {
                    var link = child.Attributes["href"].Value;
                    var sourceid = getlinkidqn(link);
                    if (sourceid != null)
                    {
                        if (!ctrl.QN_CMS_CheckFilterExist(info.Source_ID, item.InnerText.Replace(System.Environment.NewLine, string.Empty).Trim()))
                        {
                            info.Source = link;
                            info.Source_ID = sourceid;
                            info.Source_Name = "Báo Quảng Nam";
                            HtmlAgilityPack.HtmlDocument doccontent = new HtmlAgilityPack.HtmlDocument();
                            doccontent = web.Load(link);
                            var detail = doccontent.DocumentNode.SelectSingleNode("//article");
                            var title = detail.Element("h1").InnerText;

                            info.Title = title.Replace(System.Environment.NewLine, string.Empty).Trim();

                            var content = detail.SelectSingleNode("//div[@class='body-content']");
                            var intro = content.SelectSingleNode("//div[@class='chapeau']");
                            info.Intro_Content = intro.InnerText.Replace(System.Environment.NewLine, string.Empty).Replace("(QNO) - ", "").Trim();
                            var contentbody = "";
                            int index = -1;
                            content.ChildNodes.ToList().ForEach(citem =>
                            {
                                index++;
                                if (index != 1) contentbody += citem.InnerHtml;
                            });
                            var img = content.SelectNodes("//img").Where(x => x.Attributes["src"].Value.Contains("https")).First();
                            if (img != null)
                            {
                                var imgdata = img.Attributes["src"].Value;
                                string URL = "";
                                var filename = Guid.NewGuid() + ".jpg";
                                var path = "/assets/cms/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                                hp.CreateFolder(path);
                                try
                                {
                                    if (imgdata.StartsWith("http"))
                                    {
                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgdata);
                                        WebResponse response = request.GetResponse();
                                        Stream stream = response.GetResponseStream();
                                        Bitmap bitmap = new Bitmap(stream);
                                        if (bitmap != null)
                                        {
                                            URL = path + filename;
                                            var resize = hp.resizeImage(bitmap, new Size(450, 280));
                                            resize.Save(System.Web.Hosting.HostingEnvironment.MapPath(URL), ImageFormat.Jpeg);
                                            info.Intro_Img_Lg = URL;
                                            var sm = path + "sm" + filename;
                                            resize = hp.resizeImage(bitmap, new Size(150, 90));
                                            resize.Save(System.Web.Hosting.HostingEnvironment.MapPath(sm), ImageFormat.Jpeg);
                                            info.Intro_Img = sm;
                                        }
                                        stream.Flush();
                                        stream.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (imgdata.StartsWith("http"))
                                    {
                                        info.Intro_Img_Lg = imgdata;
                                        info.Intro_Img = imgdata;
                                    }
                                    else
                                    {
                                        info.Intro_Img_Lg = "/Assets/img/qnmedia_lg.png";
                                        info.Intro_Img = "/Assets/img/qnmedia.png";
                                    }
                                }
                                if (String.IsNullOrEmpty(info.Intro_Img_Lg))
                                {
                                    info.Intro_Img_Lg = "/Assets/img/qnmedia_lg.png";
                                    info.Intro_Img = "/Assets/img/qnmedia.png";
                                }
                            }
                            info.Content = removeclass(contentbody);
                            info.CID = cm;
                            info.KeyWord = keywork;
                            info.CreatedByName = "Quảng Nam Media";
                            info.IsCheck = true;
                            var updateinfo = ctrl.QN_CMS_AutoUpdate(info);
                            if (updateinfo != null)
                            {
                                auto.PortalID = updateinfo.PortalID;
                            }
                        }
                    }
                }
            });
            return auto;
        }
        public string FindHrefs(string input)
        {
            try
            {
                Regex regex = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
                Match match = regex.Match(input);
                return match.Success ? match.Groups[1].ToString() : "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string removeclass(string input)
        {
            try
            {
                input = Regex.Replace(input, "class\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", String.Empty);
                input = Regex.Replace(input, "width\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", String.Empty);
                return Regex.Replace(input, "height\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", String.Empty);
            }
            catch (Exception)
            {
                return input;
            }
        }
        public string getlinkidqn(string url)
        {
            var s = new[] { '-' };
            try
            {
                return "bqn" + url.Split(s).Last().Split('.').First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}