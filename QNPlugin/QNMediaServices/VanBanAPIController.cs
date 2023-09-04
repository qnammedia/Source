using System;
using System.Collections.Generic;
using System.Web;

using System.Net.Http;
using DotNetNuke.Web.Api;
using System.Web.Http;
using System.Net;
using System.IO;
using DotNetNuke.Entities.Users;
using Newtonsoft.Json;
using System.Linq;
using System.Configuration;
using QNMedia.CMS.Services;
using QNMedia.Office;
using DotNetNuke.Common.Utilities;

namespace QNMedia.CMSAPI
{
    [DnnAuthorize]
    [QNMediaLicensePolicy]
    public class VanBanAPIController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpPost()]
        public HttpResponseMessage Update([FromBody] CMS_Vanban info)
        {
            try
            {
                info.CreatedBy = UserInfo.UserID;
                info.PortalID = PortalSettings.PortalId;
                if (info.ID==0&& info.FileChinh == null) return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Không tìm thấy file văn bản" });
                if(info.FileChinh != null)
                {
                    if (info.FileChinh.ID == 0)
                    {
                        var filechinh = hp.TempToFile(info.FileChinh);
                        filechinh.FID = 0;
                        var filevanban = ctrl.CMS_Files_Add(filechinh);
                        info.FileID = filevanban.ID;
                        info.FileURL = filevanban.FileURL;
                    }
                }
                var vb = ctrl.CMS_VanBan_Update(info);
                if (vb != null)
                {
                    var message = "";

                    if (info.LstFileKhac.Count > 0)
                    {
                        var files = info.LstFileKhac;
                        for (int i = 0; i < files.Count; i++)
                        {
                            try
                            {
                                if (files[i].ID == 0)
                                {
                                    var file = hp.TempToFile(files[i]);
                                    file.FID = 0;
                                    var filevanban = ctrl.CMS_Files_Add(file);
                                    if (filevanban.ID > 0)
                                    {
                                        var filekhac = ctrl.CMS_VanBan_File_Update(new CMS_VanBan_File { VBID = vb.ID, FileID = filevanban.ID });
                                        if (filekhac != null)
                                            vb.LstFileKhac.Add(filekhac);
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                message += "thao tac: " + files[i].FileName + " that bai" + ex.Message;
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = vb,message=message });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Thao tác thất bại" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        public HttpResponseMessage Update_Public([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.CMS_VanBan_UpdatePublic(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        public HttpResponseMessage Update_View([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.CMS_VanBan_UpdateView(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        public HttpResponseMessage UpdateSortOrder([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.CMS_VanBan_UpdateSortOrder(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        public HttpResponseMessage Delete([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.CMS_VanBan_Delete(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        public HttpResponseMessage VanBan_File_Delete([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.CMS_VanBan_File_Delete(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        
        [HttpGet()]
        public HttpResponseMessage Get()
        {
            try
            {
                var lst = ctrl.CMS_VanBan_Get(PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        public HttpResponseMessage GetByID(int ID)
        {
            try
            {
                var ob = ctrl.CMS_VanBan_Get(PortalSettings.PortalId).Where(x => x.ID == ID).FirstOrDefault();
                ob.LstFileKhac = ctrl.CMS_VanBan_File_GetByVB(ob.ID);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles = "Administrators")]
        [QNMediaPolicy]
        public HttpResponseMessage GetByModule(int tmid)
        {
            try
            {
                var ob = ctrl.CMS_VanBan_GetByModule(PortalSettings.PortalId, tmid);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}