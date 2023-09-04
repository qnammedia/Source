using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Web;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using System.IO;
using System.Linq;
using QNMedia.CMS.Services;
using System.Text;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Cryptography;
using DotNetNuke.Web.Api;
using System.Web.Http.Controllers;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace QNMedia.Office
{
    public class QNMediaHelper
    {
        public QNMediaHelper()
        {
        }
        public CMS_Files TempToFile(CMS_Files info)
        {
            var movepath = String.Format("/QNMedia/{0}/{1}/{2}/", DateTime.Now.Year, info.FID, info.ID);
            CreateFolder(movepath);
            string filename = Path.GetFileName(info.FileURL);
            string newpath = movepath + filename;
            if (File.Exists(HttpContext.Current.Server.MapPath(newpath)))
            {
                bool exists = true;
                int stt = 1;
                while (exists)
                {
                    newpath = movepath + Path.GetFileNameWithoutExtension(filename) + "(" + stt + ")" + Path.GetExtension(filename);
                    if (File.Exists(HttpContext.Current.Server.MapPath(newpath))) stt++;
                    else
                    {
                        exists = false;
                    }
                }
            }
            File.Move(HttpContext.Current.Server.MapPath(info.FileURL), HttpContext.Current.Server.MapPath(newpath));
            info.FileURL = newpath;
            return info;
        }
        public string MoveFile(string oldpath, string movepath)
        {
            CreateFolder(HttpContext.Current.Server.MapPath(movepath));
            string filename = Path.GetFileName(oldpath);
            string newpath = movepath + filename;
            if (File.Exists(HttpContext.Current.Server.MapPath(newpath)))
            {
                bool exists = true;
                int stt = 1;
                while (exists)
                {
                    newpath = movepath + Path.GetFileNameWithoutExtension(filename) + "(" + stt + ")" + Path.GetExtension(filename);
                    if (File.Exists(HttpContext.Current.Server.MapPath(newpath))) stt++;
                    else
                    {
                        exists = false;
                    }
                }
            }
            File.Move(HttpContext.Current.Server.MapPath(oldpath), HttpContext.Current.Server.MapPath(newpath));
            return newpath;
        }
        public void CreateFolder(string strPath)
        {
            try
            {
                string[] s = System.Web.Hosting.HostingEnvironment.MapPath(strPath).Split('/');
                if (s.Length > 0)
                {
                    string path = "";
                    foreach (var item in s)
                    {
                        path += item + "/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        public ResponsePagingServerStatus GetPagingData<T>(List<T> lst, int Size, int Page)
        {
            try
            {
                var recordCount = lst.Count();
                var pageCount = (int)((recordCount + Size) / Size);
                return new ResponsePagingServerStatus { status = "OK", result = PagingData<T>(lst, Size, Page), length = recordCount, totalpage = pageCount };
            }
            catch (Exception ex)
            {
                return new ResponsePagingServerStatus { status = "Failed", message = ex.Message };
            }
        }
        public IEnumerable<T> PagingData<T>(List<T> objectList, int PageSize, int page = 0)
        {
            if (objectList.Count < 1)
            {
                return objectList;
            }
            else
            {
                return objectList.Skip(PageSize * page).Take(PageSize).ToList();
            }
        }
        public List<string> GetCMSFilterImages(string imgdata)
        {
            List<string> s = new List<string>();
            try
            {
                string URL = "";
                var filename = Guid.NewGuid() + ".jpg";
                var path = "/assets/cms/"+DateTime.Now.Year+"/"+ DateTime.Now.Month+ "/";
                CreateFolder(path);
                if (imgdata.StartsWith("http"))
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(imgdata);
                    Bitmap bitmap; bitmap = new Bitmap(stream);
                    if (bitmap != null)
                    {
                        URL = path + filename;
                        var resize = resizeImage(bitmap, new Size(450, 280));
                        resize.Save(HttpContext.Current.Server.MapPath(URL), ImageFormat.Jpeg);
                        s.Add(URL);
                        var sm = path + "sm" + filename;
                        resize = resizeImage(bitmap, new Size(150, 90));
                        resize.Save(HttpContext.Current.Server.MapPath(sm), ImageFormat.Jpeg);
                        s.Add(sm);
                    }
                    stream.Flush();
                    stream.Close();
                    client.Dispose();
                }
            }
            catch (Exception)
            {
            }
            return s;
        }
        public string GetCMSImgURL(string imgdata, int width, int height)
        {
            string URL = "";
            if (String.IsNullOrEmpty(imgdata)) return "";
            try
            {
                var filename = Guid.NewGuid() + ".jpg";
                var path = "/assets/cms/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                CreateFolder(path);
                if (String.IsNullOrEmpty(imgdata))
                    return URL;
                if (IsBase64(imgdata))
                {
                    try
                    {
                        URL = path + filename;
                        var bytes = Convert.FromBase64String(imgdata.Split(',')[1]);
                        using (var ms = new MemoryStream(bytes))
                        {
                            var resize = resizeImage(Image.FromStream(ms), new Size(width, height));
                            resize.Save(HttpContext.Current.Server.MapPath(URL), ImageFormat.Jpeg);
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {
                    if (imgdata.StartsWith("http"))
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(imgdata);
                        Bitmap bitmap; bitmap = new Bitmap(stream);
                        if (bitmap != null)
                        {
                            URL = path + filename;
                            var resize = resizeImage(bitmap, new Size(width, height));
                            resize.Save(HttpContext.Current.Server.MapPath(URL), ImageFormat.Jpeg);
                        }
                        stream.Flush();
                        stream.Close();
                        client.Dispose();
                    }
                    else
                    {
                        URL = imgdata;
                    }
                }
                return URL;
            }
            catch (Exception)
            {

                return "";
            }

        }
        public bool IsBase64(String str)
        {
            try
            {
                return str.StartsWith("data:image");
            }
            catch
            {
                // If exception is caught, then it is not a base64 encoded string
                return false;
            }
        }
        public System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size, bool keeppercent = false)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = keeppercent ? (int)(sourceWidth * nPercent) : size.Width;
            //New Height  
            int destHeight = keeppercent ? (int)(sourceHeight * nPercent) : size.Height;
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
        public bool CheckVerifyAPI(string encrypt)
        {
            try
            {
                var now = DateTime.Now;
                var gettime = DateTime.Parse(Decrypt(encrypt));
                TimeSpan diff = now - gettime;
                return diff.TotalSeconds <= 2;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string CreateValidateKey()
        {
            return Base64Encode(Encrypt(DateTime.Now.ToString()));
            //return Encrypt(DateTime.Now.ToString());
        }
        string key = ConfigurationManager.AppSettings["CustomValidateKey"];
        public string Encrypt(string toEncrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Decrypt(string toDecrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string Base64Encode(string text)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textBytes);
        }
        public string GetSettingValue(List<TabModuleSettingInfo> lst, string key)
        {
            try
            {
                var index = lst.FindLastIndex(x => x.SettingName == key);
                return index >= 0 ? lst[index].SettingValue : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public object GetIDFromURL(string url)
        {
            try
            {
                var getid = url.Split('-');
                var Id = getid[getid.Length - 1].Split('.').FirstOrDefault();
                return Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool checkfileisdoc(string fileName)
        {
            var type = Path.GetExtension(fileName).ToLower();
            var accept = new List<string>()
                    {
                        ".jpg",
                        ".jpeg",
                        ".bmp",
                        ".gif",
                        ".png",
                        ".txt",
                        ".ppt",
                        ".pptx",
                        ".doc",
                        ".docx",
                        ".xsl",
                        ".xslx",
                        ".pdf",
                        ".zip",
                        ".7zip",
                        ".7z",
                        ".gzip",
                        ".gz",
                        ".rar",
                        ".tar",
                        ".tz",
                        ".iso",
                        ".avi",
                        ".flv",
                        ".mp4",
                        ".mp3",
                        ".mkv",
                        ".mpg",
                        ".3gp",
                        ".mov",
                        ".wmv",
                        ".mpeg",
                        ".webm",
                        ".m3u8",
                        ".ts"
                    };
            return accept.Contains(type);
        }
        public bool CheckFileIsImgage(string fileName)
        {
            var type = Path.GetExtension(fileName).ToLower();
            var accept = new List<string>()
                    {
                        ".jpg",
                        ".jpeg",
                        ".bmp",
                        ".gif",
                        ".png",
                    };
            return accept.Contains(type);
        }
        public  string ToEngString( string text)
        {
            string[] arr1 = new string[] {
                "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ", "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ"
            };
            string[] arr2 = new string[] 
            { 
                "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d","e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            text = RemoveSpecialCharacters(text);
            return text;
        }
        public  string RemoveSpecialCharacters(string str)
        {
            str= Regex.Replace(str, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
            return Regex.Replace(str, "  ", " ", RegexOptions.Compiled);
        }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class QNMediaPolicyAttribute : AuthorizeAttributeBase
    {
        public QNMediaPolicyAttribute()
        {
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            QNMediaHelper hp = new QNMediaHelper();
            var headers = actionContext.Request.Headers;
            if (!hp.CheckVerifyAPI(headers.GetValues("client").First().ToString()))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest);
                return;
            }
        }
        public override bool IsAuthorized(AuthFilterContext context)
        {
            throw new NotImplementedException();
        }
        //public override bool IsAuthorized(AuthFilterContext context)
        //{
        //    QNMediaHelper hp = new QNMediaHelper();
        //    var headers = context.ActionContext.Request.Headers;
        //    return hp.CheckVerifyAPI(headers.GetValues("client").First().ToString());
        //}
    }
    public sealed class QNMediaLicensePolicy : AuthorizeAttributeBase
    {
        public QNMediaLicensePolicy()
        {
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            QNMediaHelper hp = new QNMediaHelper();
            // Get the headers
            if (!actionContext.Request.RequestUri.ToString().Contains("quangnammedia.vn") && !actionContext.Request.RequestUri.ToString().Contains("qnmedia.vn"))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest);
                return;
            }
        }
        public override bool IsAuthorized(AuthFilterContext context)
        {
            throw new NotImplementedException();
        }
        //public override bool IsAuthorized(AuthFilterContext context)
        //{
        //    QNMediaHelper hp = new QNMediaHelper();
        //    var headers = context.ActionContext.Request.Headers;
        //    return hp.CheckVerifyAPI(headers.GetValues("client").First().ToString());
        //}
    }
}