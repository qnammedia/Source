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
    public class VideoAPIController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpPost()]
        public HttpResponseMessage Update([FromBody] CMS_Video info)
        {
            try
            {
                var img = info.Img;
                info.LastedModifyBy = UserInfo.UserID;
                info.PortalID = PortalSettings.PortalId;
                if (img!= "/assets/img/video.png")
                {
                    info.Img = hp.GetCMSImgURL(img, 150, 90);
                    info.Img_Lg = hp.GetCMSImgURL(img, 400, 240);
                }
                else
                {
                    info.Img = "/assets/img/video_sm.png";
                    info.Img_Lg = "/assets/img/video.png";
                }
                var ob = ctrl.CMS_Video_Update(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result= ob });
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
                ctrl.CMS_Video_Update_Public(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK"});
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
                ctrl.CMS_Video_Update_View(info);
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
                ctrl.CMS_Video_Delete(info);
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
                var lst = ctrl.CMS_Video_Get(PortalSettings.PortalId);
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
                var ob = ctrl.CMS_Video_Get(PortalSettings.PortalId).Where(x=>x.ID==ID).FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK",result =ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    
    }
}