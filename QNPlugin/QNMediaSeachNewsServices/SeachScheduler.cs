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
    public class SeachScheduler : SchedulerClient
    {
        QNMedia.CMS.SearchServices.SearchController ctrl = new QNMedia.CMS.SearchServices.SearchController();
        QNMediaHelper hp = new QNMediaHelper();
        CMSController cms = new CMSController();
        public SeachScheduler(ScheduleHistoryItem oItem) : base()
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
                            ctrl.CMS_ZSearch_Config_UpdateTime(item.ID);
                            var auto = Search(item.CID, item.SearchKey);
                            //if(auto!=null&& lst.FindLastIndex(x=>x.PortalID==auto.PortalID )<0) lst.Add(auto);
                        });
                        //if (lst.Count > 0)
                        //{
                        //    lst.ForEach(item =>
                        //    {
                        //        cms.QN_CMS_ResetCache(item.PortalID);
                        //    });
                        //}
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
        private AutoUpdate Search(int cm, string key)
        {
            AutoUpdate auto = new AutoUpdate();
            this.ScheduleHistoryItem.AddLogNote("begin " + key);
            QN_CMS info = new QN_CMS();
            var weburl = "https://baomoi.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = web.Load(weburl + "tim-kiem/" + key + ".epi");
            var listcontent = doc.DocumentNode.Descendants("div").First();
            var body = listcontent.Elements("div").ToList();
            if (body.Count() >= 2)
            {
                var doclink = body[2].SelectNodes("//a[@href]").Where(link => !String.IsNullOrEmpty(link.InnerText) && !link.Attributes["href"].Value.Contains("/tag") && link.Attributes["href"].Value.Length > 50).OrderByDescending(x => x.OuterStartIndex).ToList();
                if (doclink.Count() > 0)
                {
                    this.ScheduleHistoryItem.AddLogNote("begin search link");

                    foreach (var link in doclink)
                    {
                        this.ScheduleHistoryItem.AddLogNote("link:" + link.Attributes["href"].Value);

                        info.Source_ID = getlinkid(link.Attributes["href"].Value).ToString();
                        if (info.Source_ID != "0" && !link.Attributes["href"].Value.Contains("vtc.vn"))
                        {
                            HtmlAgilityPack.HtmlDocument docdetail = new HtmlAgilityPack.HtmlDocument();
                            docdetail = web.Load(weburl + link.Attributes["href"].Value);
                            var detailhtml = docdetail.DocumentNode.Descendants("div").First().Elements("div").ToList();
                            if (detailhtml.Count() >= 1)
                            {
                                var detail = detailhtml[2];
                                var filtercontent = detail.Element("div").Element("div").Elements("div").ToList();
                                var fullcontent = filtercontent[1].Element("div").Element("div");
                                var content = fullcontent.Elements("div").ToList();
                                if (content.Count() >= 2)
                                {

                                    var title = content[0].Element("h1");
                                    if (!ctrl.QN_CMS_CheckFilterExist(info.Source_ID, title.InnerText))
                                    {
                                        info.Title = title.InnerText.Trim();
                                        var intro = content[0].Element("h3");
                                        info.Intro_Content = intro.InnerHtml;
                                        var noidungcontainer = content[0].Elements("div").ToList();
                                        if (noidungcontainer.Count >= 1)
                                        {
                                            info.Content = removeclass(noidungcontainer[1].InnerHtml);
                                            var img = noidungcontainer[1].SelectNodes("//img").Last();
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
                                        }
                                        if (String.IsNullOrEmpty(info.Intro_Img_Lg))
                                        {
                                            info.Intro_Img_Lg = "/Assets/img/qnmedia_lg.png";
                                            info.Intro_Img = "/Assets/img/qnmedia.png";
                                        }
                                        var source = fullcontent.Element("p").Element("a").Elements("span").ToList();
                                        if (source.Count() >= 1)
                                        {
                                            info.Source_Name = source[0].InnerText.Replace("Nguồn", "").Replace(":", "").Trim();
                                            info.Source = source[1].InnerHtml;
                                        }
                                        info.CID = cm;
                                        info.KeyWord = key;
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
                        }
                    }
                }
            }
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
        public int getlinkid(string url)
        {
            var s = new[] { '/' };
            string id = url.Split(s).Last().Split('.').First();
            try
            {
                return int.Parse(id);
            }
            catch
            {
                return 0;
            }
        }
    }
}