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
using DotNetNuke.Entities.Modules;

namespace QNMedia.CMSAPI
{
    [DnnAuthorize(StaticRoles ="Administrator")]
    [QNMediaLicensePolicy]
    [QNMediaPolicy]
    public class AdminActionController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpGet()]
        public HttpResponseMessage GetServerStatus(int TabModuleId)
        {
            try
            {
                if(hp.CheckVerifyAPI(Request.Headers.GetValues("client").First().ToString())) 
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Truy cập không hợp lệ" });
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result=ctrl.CMS_GetTabModuleSettings(TabModuleId) });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [QNMediaPolicy]
        [DnnAuthorize(StaticRoles ="Administrators")]
        public HttpResponseMessage UpdateTabModuleSetting([FromBody] List<TabModuleSettingInfo> lst)
        {
            try
            {
                //foreach (var info in lst)
                //{
                //    info.UserID = UserInfo.UserID;
                //    ctrl.CMS_UpdateTabModuleSetting(info);
                //}
                ModuleController objModules = new ModuleController();
                foreach (var info in lst)
                {
                    objModules.UpdateTabModuleSetting(info.TabModuleId, info.SettingName, info.SettingValue);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK",result= PortalSettings.ActiveTab.TabID });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles ="Administrators")]
        [QNMediaPolicy]
        public HttpResponseMessage GetTabModuleSettings(int TabModuleId)
        {
            try
            {
                var lst = ctrl.CMS_GetTabModuleSettings(TabModuleId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }

        [HttpGet()]
        [DnnAuthorize(StaticRoles = "Administrators")]
        public HttpResponseMessage CMS_PortalSettings_Get()
        {
            try
            {
                var obj = ctrl.CMS_PortalSettings_Get(PortalSettings.PortalId);
                if (obj == null)
                    obj = new CMS_PortalSettings { PortalID = PortalSettings.PortalId, };
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = obj });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }

    }
}