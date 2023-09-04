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
using DotNetNuke.Security;

namespace QNMedia.CMSAPI
{
    [QNMediaLicensePolicy]
    public class PublicController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpGet()]
        [AllowAnonymous]
        public HttpResponseMessage GetServerStatus()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result=hp.CreateValidateKey() });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [AllowAnonymous]
        public HttpResponseMessage LTC_Get()
        {
            try
            {
                var ltc = ctrl.QN_LTC_Get(PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ltc });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Users_Log_Get(int limit)
        {
            try
            {
                var info = ctrl.CMS_Users_Log_Get(UserInfo.UserID, limit);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = info });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [AllowAnonymous]
        public HttpResponseMessage CheckStatus()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status =  hp.CheckVerifyAPI(Request.Headers.GetValues("client").First().ToString())?"OK":"Failed" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [AllowAnonymous]
        [QNMediaPolicy]
        public HttpResponseMessage CheckToken([FromBody] BaseUpdateInfo info)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = hp.CheckVerifyAPI(Request.Headers.GetValues("client").First().ToString()) });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [AllowAnonymous]
        [QNMediaPolicy]
        public HttpResponseMessage Categorys_Get(string type)
        {
            try
            {
                var lst = ctrl.QN_Sys_Categorys_Get(PortalSettings.PortalId);
                var rs = lst.FindAll(x => x.Type == type&&x.IsPublic);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles ="Administrators")]
        [QNMediaPolicy]
        public HttpResponseMessage CMS_GetByTMid(int tmid)
        {
            try
            {
                var result = ctrl.CMS_GetByModulePortal(PortalSettings.PortalId,  tmid);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = result });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles = "Administrators")]
        [QNMediaPolicy]
        public HttpResponseMessage CMS_GetByChuyenMuc(int tmid,int cid)
        {
            try
            {
                var result = ctrl.CMS_GetByChuyenMuc(PortalSettings.PortalId, tmid, cid);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = result });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles = "Administrators")]
        [QNMediaPolicy]
        public HttpResponseMessage Video_GetByModule(int tmid)
        {
            try
            {
                var ob = ctrl.CMS_Video_GetByModule(PortalSettings.PortalId, tmid);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}