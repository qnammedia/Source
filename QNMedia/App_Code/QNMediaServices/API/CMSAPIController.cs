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
using System.Web.Script.Serialization;
using QNMedia.CMS.Services;
namespace QNMedia.CMSAPI
{
    public class CMSAPIController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        [HttpGet()]
        [DnnAuthorize(AuthTypes = "JWT")]
        public HttpResponseMessage Ping()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Policy_Get()
        {
            try
            {
                var rs = ctrl.QN_Sys_Policy_Get(UserInfo.UserID);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result=rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [AllowAnonymous]
        public HttpResponseMessage Sys_Categorys_Get(string type)
        {
            try
            {
                var rs = ctrl.QN_Sys_Categorys_Get(type,PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}