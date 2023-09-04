using System;
using System.Net;
using DotNetNuke.Web.Api;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using System.IO;
using QNMedia.CMS.Services;
using QNMedia.Office;

namespace QNMedia.CMSAPI
{
    [AllowAnonymous]
    [QNMediaLicensePolicy]
    public class CMSFilesController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper qnhelp = new QNMediaHelper();
        [HttpGet()]
        public HttpResponseMessage Ping()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        public HttpResponseMessage Get()
        {
            try
            {
                ResponseStatus err = new ResponseStatus { status = HttpContext.Current.User.Identity.Name };
                return Request.CreateResponse(HttpStatusCode.OK, err);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Files_GetByFolder(int FID)
        {
            try
            {
                var rs = ctrl.CMS_Files_Get(UserInfo.UserID, FID);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK",result= rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Files_Delete([FromBody] BaseUpdateInfo info)
        {
            try
            {
               var messsage= ctrl.CMS_Files_Delete(info);
               IISTask.IISTaskFactory.StartNew(()=> File.Delete(HttpContext.Current.Server.MapPath(info.Value.ToString())));
               return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK", result = info,message= messsage });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_UploadFile([FromUriAttribute] int FID,int Media=0)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = "Không có file" });
                }
                List<CMS_Files> rs = new List<CMS_Files>();
                var docfiles = new List<string>();
                var UserId = base.UserInfo.UserID;
                var uri = httpRequest.Url;
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (qnhelp.checkfileisdoc(postedFile.FileName))
                    {
                        var tempname = String.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(postedFile.FileName));
                        var path = String.Format("/CMS/{0}/{1}/{2}/{3}/{4}/", DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day, PortalSettings.PortalId,UserId);
                        if (Media==1)
                        {
                            path = String.Format("/CMS/Media/{0}/{1}/{2}/{3}/",  DateTime.Now.Year, DateTime.Now.Month, PortalSettings.PortalId, UserId);
                        }
                        qnhelp.CreateFolder(path);
                        string sPath = HttpContext.Current.Server.MapPath(path);
                        var filePath = sPath + tempname;
                        postedFile.SaveAs(filePath);
                        CMS_Files f = new CMS_Files
                        {
                            FileName = postedFile.FileName,
                            FileSize = postedFile.ContentLength,
                            FileURL = path + tempname,
                            ID = 0,
                            CreatedBy = UserId,
                            PortalID = PortalSettings.PortalId,
                            FID = FID,
                        };
                        var insertfile = ctrl.CMS_Files_Add(f);
                        if(insertfile.ID>0) rs.Add(f);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage DeleteTempFile([FromBody] CMS_Files info)
        {
            try
            {
                if(info.CreatedBy!=UserInfo.UserID)
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message ="Không xác thực được người dùng" });
                var file = HttpContext.Current.Server.MapPath(info.FileURL);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Folders_Gets()
        {
            try
            {
                var lst = ctrl.CMS_User_Folders_Gets().Where(x=>x.CreatedBy==UserInfo.UserID&&x.IsActive).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Folders_Update([FromBody] CMS_User_Folders info)
        {
            try
            {
                info.CreatedBy = info.CreatedBy>0? info.CreatedBy: UserInfo.UserID;
                var lst = ctrl.CMS_User_Folders_Update(info);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK", result = lst } );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Files_UpdateSortOrder([FromBody] BaseUpdateInfo info)
        {
            try
            {
                ctrl.CMS_Files_UpdateSortOrder(info);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Folders_UpdateSort([FromBody] BaseUpdateInfo info)
        {
            try
            {
                 ctrl.CMS_User_Folders_UpdateSort(info);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK"});
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Folders_UpdateActive([FromBody] BaseUpdateInfo info)
        {
            try
            {
                ctrl.CMS_User_Folders_UpdateActive(info);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage VideoFolders_Gets()
        {
            try
            {
                var lst = ctrl.CMS_User_Folders_Gets().Where(x => x.CreatedBy == -1).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [AllowAnonymous]
        public HttpResponseMessage UpdateFileTemp()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = "Không có file" });
                }
                List<CMS_Files> rs = new List<CMS_Files>();
                var docfiles = new List<string>();
                var UserId = base.UserInfo.UserID;
                var uri = httpRequest.Url;
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    if (qnhelp.checkfileisdoc(postedFile.FileName))
                    {
                        var tempname = String.Format("{0}{1}", Guid.NewGuid(), Path.GetExtension(postedFile.FileName));
                        var path = "/CMS/Temp/";
                        qnhelp.CreateFolder(path);
                        string sPath = HttpContext.Current.Server.MapPath(path);
                        var filePath = sPath + tempname;
                        postedFile.SaveAs(filePath);
                        CMS_Files f = new CMS_Files
                        {
                            CreatedBy = UserInfo.UserID,
                            FileName = postedFile.FileName,
                            FileSize = postedFile.ContentLength,
                            FileURL = path + tempname,
                            ID = 0,
                        };
                        rs.Add(f);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
    }
}