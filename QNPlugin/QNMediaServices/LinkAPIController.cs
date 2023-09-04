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
    public class LinkAPIController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpPost()]
        public HttpResponseMessage Update([FromBody] CMS_LinkLienKet info)
        {
            try
            {
                info.LastedModifyBy = UserInfo.UserID;
                info.PortalID = PortalSettings.PortalId;
                if (!info.Img.StartsWith("/"))
                {
                    info.Img = hp.GetCMSImgURL(info.Img, 160, 120);
                }
                var vb = ctrl.CMS_LinkLienKet_Update(info);
                if (vb != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = vb });
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
                ctrl.CMS_LinkLienKet_UpdateSuDung(info);
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
                ctrl.CMS_LinkLienKet_UpdateSortOrder(info);
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
                ctrl.CMS_LinkLienKet_Delete(info);
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
                var lst = ctrl.CMS_LinkLienKet_Get(PortalSettings.PortalId);
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
                var ob = ctrl.CMS_LinkLienKet_Get(PortalSettings.PortalId).Where(x => x.ID == ID).FirstOrDefault();
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
                var ob = ctrl.CMS_LinkLienKet_GetByModule(PortalSettings.PortalId, tmid);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}