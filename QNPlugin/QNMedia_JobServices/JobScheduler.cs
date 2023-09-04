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
using QNMedia.Job.JobServices;
using QNMedia.Office;

namespace QNMedia.Scheduler
{
    public class JobScheduler : SchedulerClient
    {
        QNMedia.CMS.SearchServices.SearchController ctrl = new QNMedia.CMS.SearchServices.SearchController();
        QNMediaHelper hp = new QNMediaHelper();

        public JobScheduler(ScheduleHistoryItem oItem) : base()
        {
            this.ScheduleHistoryItem = oItem;
        }
        public override void DoWork()
        {
            try
            {
                this.Progressing();
                {
                    SearchAndInport();
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
        private void SearchAndInport()
        {
            JobController ctrl = new JobController();
            var weburl = "https://www.careerlink.vn/";
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc = web.Load(weburl + "tim-viec-lam-tai/quang-nam/QN?order=posted_date&posted_within=30&sort=desc");
            var listcontent = doc.DocumentNode.Descendants("ul").Where(d => d.Attributes["class"].Value.Contains("list-group")).FirstOrDefault();
            var listjob = listcontent.Elements("li").OrderByDescending(x => x.OuterStartIndex).ToList();
            listjob.ForEach(item =>
            {
                var jobinfolink = item.Descendants("a").ToList();
                var linkjob = jobinfolink.Where(d => d.Attributes["class"].Value.Contains("job-link")).FirstOrDefault().Attributes["href"].Value;
                if (ctrl.QN_Job_CheckUpdate(getjobid(linkjob))) return;
                var linkcompany = jobinfolink.Where(d => d.Attributes["class"].Value.Contains("job-company")).FirstOrDefault().Attributes["href"].Value;
                var company = ctrl.QN_Job_Company_AutoCheckUpdate(getogid(linkcompany));
                if (company == null)
                {
                    QN_Job_Company cp = new QN_Job_Company();
                    string URL = "";
                    var filename = Guid.NewGuid() + ".jpg";
                    var path = "/assets/company/";
                    var imgdata = item.Descendants("img").FirstOrDefault().Attributes["src"].Value;
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
                                resize = hp.resizeImage(bitmap, new Size(150, 90));
                                resize.Save(System.Web.Hosting.HostingEnvironment.MapPath(URL), ImageFormat.Jpeg);
                                cp.Img = URL;
                            }
                            stream.Flush();
                            stream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (imgdata.StartsWith("http"))
                        {
                            cp.Img = imgdata;
                        }
                        else
                        {
                            cp.Img = "/Assets/img/company.png";
                        }
                    }
                    cp.SourceID = getogid(linkcompany);
                    cp.Name = item.Descendants("img").FirstOrDefault().Attributes["alt"].Value;
                    HtmlAgilityPack.HtmlDocument og = new HtmlAgilityPack.HtmlDocument();
                    og = web.Load(weburl + linkcompany);
                    var divoginfo = og.DocumentNode.Descendants("div").ToList();
                    divoginfo.ForEach(child =>
                    {
                        try
                        {
                            if (child.Attributes["class"].Value.Contains("company-information"))
                            {
                                var info = child.Descendants("span").ToList();
                                if (info.Count > 1)
                                {
                                    cp.Address = info[0].InnerText;
                                    cp.QuyMo = info[1].InnerText;
                                }
                                return;
                            }
                        }
                        catch
                        {
                        }
                    });
                    divoginfo.ForEach(child =>
                    {
                        try
                        {
                            if (child.Attributes["class"].Value.Contains("company-profile"))
                            {
                                cp.Info = removeclass(child.InnerHtml);
                                return;
                            }
                        }
                        catch
                        {
                        }
                    });
                    company = ctrl.QN_Job_Company_AutoUpdate(cp);
                }
                if (company == null) return;
                var jobupdateinfo = new QN_Job();
                jobupdateinfo.OgID = company.ID;
                jobupdateinfo.SourceID = getjobid(linkjob);
                jobupdateinfo.Title = jobinfolink.Where(d => d.Attributes["class"].Value.Contains("job-link")).FirstOrDefault().InnerText.Replace("\n", "").Trim();
                HtmlAgilityPack.HtmlDocument job = new HtmlAgilityPack.HtmlDocument();
                job = web.Load(weburl + linkjob);

                var divjobinfo = job.DocumentNode.Descendants("div").ToList();
                divjobinfo.ForEach(child =>
                {
                    try
                    {
                        if (child.Attributes["class"].Value.Contains("job-overview"))
                        {
                            var jobinfo = child.Descendants("span").ToList();
                            string noilamviec = "";
                            for (int k = 0; k < jobinfo.Count - 7; k++)
                            {
                                var noilv = jobinfo[k].InnerText.Trim();
                                if (k % 2 == 0)
                                    noilamviec += String.IsNullOrEmpty(noilamviec) ? noilv : "; " + noilv;
                            }
                            jobupdateinfo.Job_NoiLamViec = noilamviec;
                            var mucluongtu = 0;
                            var mucluongden = 0;
                            var luong = getnumberfromstring(jobinfo[jobinfo.Count - 7].InnerText);
                            if (luong.Count == 2)
                            {
                                mucluongtu = luong[0];
                                mucluongden = luong[1];
                            }
                            else if (luong.Count == 1)
                            {
                                mucluongtu = luong[0];
                                mucluongden = 0;
                            }
                            jobupdateinfo.Salary_From = mucluongtu;
                            jobupdateinfo.Salary_To = mucluongden;
                            jobupdateinfo.KinhNghiem = jobinfo[jobinfo.Count - 5].InnerText.Trim();
                            var getngaydang = getnumberfromstring(jobinfo[jobinfo.Count - 4].InnerText);
                            var ngaydang = DateTime.Now;
                            if (getngaydang.Count == 3)
                            {
                                ngaydang = new DateTime(getngaydang[2], getngaydang[1], getngaydang[0]);
                            }
                            jobupdateinfo.CreatedOn = ngaydang;
                            var getngayhethan = getnumberfromstring(jobinfo[jobinfo.Count - 1].InnerText);
                            var ngayhethan = DateTime.Now.AddMonths(1);
                            if (getngayhethan.Count == 1)
                            {
                                ngayhethan = ngaydang.AddDays(getngayhethan[0]);
                            }
                            jobupdateinfo.ExpirationOn = ngayhethan;
                            jobupdateinfo.Job_NoiLamViec = jobinfo[1].InnerText.Split(',').First().Replace("\n", "").Trim();
                            return;
                        }
                    }
                    catch
                    {

                    }
                });

                divjobinfo.ForEach(child =>
                {
                    try
                    {
                        if (child.Attributes["class"].Value.Contains("job-detail-body"))
                        {
                            var divmota = child.SelectSingleNode($"//div[@id='section-job-description']").Elements("div").Last();
                            jobupdateinfo.Info = divmota.InnerHtml;
                            var divrequired = child.SelectSingleNode($"//div[@id='section-job-skills']").Elements("div").Last();
                            jobupdateinfo.Required = divrequired.InnerHtml;
                            var motachitiet = child.SelectSingleNode($"//div[@id='section-job-summary']").Elements("div").Last().Descendants("div").Where(d => d.Attributes["class"].Value.Contains("job-summary-item")).Where(d => d.Attributes["class"].Value.Contains("job-summary-item")).ToList();
                            jobupdateinfo.TypeName = motachitiet[0].Elements("div").Last().InnerText.Replace("\n", "").Trim();
                            jobupdateinfo.LevelName = motachitiet[1].Elements("div").Last().InnerText.Replace("\n", "").Trim();
                            jobupdateinfo.EducationName = motachitiet[2].Elements("div").Last().InnerText.Replace("\n", "").Trim();
                            var gioitinh = motachitiet[3].Elements("div").Last().InnerText.Replace("\n", "").Trim().ToUpper();
                            switch (gioitinh)
                            {
                                case "NAM":
                                    jobupdateinfo.Sex = 1; break;
                                case "NỮ":
                                    jobupdateinfo.Sex = 0; break;
                                default:
                                    jobupdateinfo.Sex = -1; break;
                            }
                            if (motachitiet.Count >= 6)
                            {
                                var tuoi = getnumberfromstring(motachitiet[4].Elements("div").Last().InnerText.Replace("\n", "").Trim().ToUpper());
                                if (tuoi.Count == 2)
                                {
                                    jobupdateinfo.Old_From = tuoi[0];
                                    jobupdateinfo.Old_To = tuoi[1];
                                }else if (tuoi.Count == 1)
                                {
                                    if (motachitiet[4].Elements("div").Last().InnerText.ToUpper().Contains("trên"))
                                    {
                                        jobupdateinfo.Old_From = tuoi[0];
                                    }
                                    else
                                    {
                                        jobupdateinfo.Old_To = tuoi[0];
                                    }
                                }
                                jobupdateinfo.Job_Career = motachitiet[5].Elements("div").Last().InnerText.Replace("\n", "").Trim();
                                try
                                {
                                    var divphucloi = child.SelectSingleNode($"//div[@id='section-job-benefits']");
                                    if (divphucloi != null)
                                    {
                                        jobupdateinfo.Benefits = divphucloi.Elements("div").Last().InnerHtml;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                jobupdateinfo.Old_From = 0;
                                jobupdateinfo.Old_To = 0;
                                jobupdateinfo.Job_Career = motachitiet[4].Elements("div").Last().InnerText.Replace("\n", "").Trim();
                                try
                                {
                                    var divphucloi = child.SelectSingleNode($"//div[@id='section-job-benefits']");
                                    if (divphucloi != null)
                                    {
                                        jobupdateinfo.Benefits = divphucloi.Elements("div").Last().InnerHtml;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }

                            var ngonngu = child.SelectSingleNode($"//div[@id='section-job-contact-information']").Elements("div").Last().Descendants("p").First();
                            jobupdateinfo.LanguageName = ngonngu.InnerText.Split(':').Last().Trim();
                            var divcontact = child.SelectSingleNode($"//div[@id='section-job-contact-information']").Elements("ul").Last();
                            jobupdateinfo.ContactTo = divcontact.OuterHtml.Replace("CareerLink", "quangnammedia.vn");
                            // Literal1.Text +=  jobupdateinfo.Title;

                            ctrl.QN_Job_AutoUpdate(jobupdateinfo);
                        }
                    }
                    catch
                    {
                    }
                });
            });
        }
        public List<int> getnumberfromstring(string input)
        {
            List<int> lst = new List<int>();
            string[] numbers = Regex.Split(input, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int i = int.Parse(value);
                    lst.Add(i);
                }
            }
            return lst;
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
        public string getogid(string url)
        {
            var s = new[] { '/' };
            try
            {
                return url.Split(s).Last();
            }
            catch
            {
                return null;
            }
        }
        public string getjobid(string url)
        {
            try
            {
                var s = new[] { '/' };
                return url.Split(s).Last().Split('?').First();
            }
            catch
            {
                return null;
            }
        }
    }
}