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
    public class ContactController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpPost()]
        [AllowAnonymous]
        [QNMediaPolicy]
        [QNMediaLicensePolicy]
        public HttpResponseMessage Contact_Insert([FromBody] CMS_Contact info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                var ob = ctrl.CMS_Contact_Insert(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Contact_Update([FromBody] CMS_Contact info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                info.LastedBy = UserInfo.UserID;
                ctrl.CMS_Contact_Update(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result= info });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Contact_Delete([FromBody] BaseUpdateInfo info)
        {
            try
            {
                ctrl.CMS_Contact_Delete(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = info });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Contact_UpdatePublic([FromBody] BaseUpdateInfo info)
        {
            try
            {
                ctrl.CMS_Contact_UpdatePublic(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = info });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Contact_Gets(int Public,int Deleted)
        {
            try
            {
                var ob = ctrl.CMS_Contact_Gets(PortalSettings.PortalId,Public, Deleted);
                return Request.CreateResponse(HttpStatusCode.OK, ob);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Contact_Count()
        {
            try
            {
                var ob = ctrl.QN_Contact_Count(PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}