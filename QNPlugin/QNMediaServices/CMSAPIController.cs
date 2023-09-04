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
using IISTask;

namespace QNMedia.CMSAPI
{
    [QNMediaLicensePolicy]
    public class CMSAPIController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        MemoryCacheHelper cache = new MemoryCacheHelper();
        [HttpGet()]
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
                var rs = ctrl.QN_Sys_Policy_Get(UserInfo.UserID, PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Sys_Categorys_Get(string type)
        {
            try
            {
                var rs = ctrl.QN_Sys_Categorys_Get(PortalSettings.PortalId).Where(x => x.Type == type).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles = "Administrators")]
        public HttpResponseMessage ClearCache(string Key)
        {
            try
            {
                MemoryCacheHelper.Delete(Key);
                return Request.CreateResponse(HttpStatusCode.OK, Key);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize(StaticRoles = "Administrators")]
        public HttpResponseMessage GetCache(string Key)
        {
            try
            {
                var rs = MemoryCacheHelper.GetValue(Key);
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject( rs));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResponseStatus { status = "Failed", message = PortalSettings.PortalId + ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Sys_Categorys_Update([FromBody] QN_Sys_Categorys info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                var check = ctrl.QN_Sys_Categorys_CheckUpdate(info);
                if (check.ID > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", result = check, message = "Đã tồn tại mục này trong hệ thống" });
                }
                info.CreatedBy = UserInfo.UserID;
                var rs = ctrl.QN_Sys_Categorys_Update(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Sys_Categorys_UpdateSort([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.QN_Sys_Categorys_UpdateSort(info);
               
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Sys_Categorys_UpdatePublic([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                ctrl.QN_Sys_Categorys_UpdatePublic(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage UploadImage([FromBody] BaseUpdateInfo info)
        {
            try
            {
                var image = hp.GetCMSImgURL(info.Value.ToString(),150,90);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = image });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Sys_Admin_GetUsers()
        {
            try
            {
                List<QN_UserInfo> lst = new List<QN_UserInfo>();
                var isadmin = UserInfo.IsAdmin || UserInfo.IsSuperUser;
                if (isadmin)
                {
                    lst = ctrl.QN_Sys_GetUserPortal(PortalSettings.PortalId);
                }
                else
                {
                    lst.Add(new QN_UserInfo { UserID = UserInfo.UserID, DisplayName = UserInfo.DisplayName });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage UserPolicy_Gets(int PID)
        {
            try
            {
                var lst = ctrl.QN_CMS_UserPolicy_Gets(PID);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Policy_Get(string type)
        {
            try
            {
                var lst = ctrl.QN_CMS_Policy_Get(PortalSettings.PortalId, type);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Policy_Update([FromBody] QN_CMS_Policy info)
        {
            try
            {
                info.PortalId = PortalSettings.PortalId;
                var rs = ctrl.QN_CMS_Policy_Update(info);
                if (rs != null)
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", result = "Có lỗi xảy ra trong quá trình thêm dữ liệu" });

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_UserPolicy_Update([FromBody] QN_CMS_UserPolicy info)
        {
            try
            {
                var rs = ctrl.QN_CMS_UserPolicy_Update(info);
                if (rs != null)
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = rs });
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", result = "Có lỗi xảy ra trong quá trình thêm dữ liệu" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Policy_Delete([FromBody] BaseUpdateInfo info)
        {
            try
            {
                ctrl.QN_CMS_Policy_Delete(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Get(string id)
        {
            try
            {
                var lst = ctrl.QN_CMS_Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage UserPolicy_Get(string Type)
        {
            try
            {

                var lst = ctrl.QN_CMS_UserPolicy_GetByUser(UserInfo.UserID, Type, PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = lst });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Policy_GetByUser(string Type)
        {
            try
            {
                var ob = ctrl.QN_CMS_Policy_GetByUser(UserInfo.UserID, Type, PortalSettings.PortalId);
                if (ob != null)
                {
                    ob.lstUserPolicy = ctrl.QN_CMS_UserPolicy_GetByUser(UserInfo.UserID, Type, PortalSettings.PortalId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Update([FromBody] QN_CMS info)
        {
            try
            {
                var img = info.Intro_Img;
                info.CreatedBy = UserInfo.UserID;
                info.PortalID = PortalSettings.PortalId;
                info.Intro_Img_Lg = hp.GetCMSImgURL(img, 450,280);
                info.Intro_Img = hp.GetCMSImgURL(img, 150,90);
                if (!String.IsNullOrEmpty(info.Start))
                {
                    info.StartDay = DateTime.Parse(info.Start);
                }
                if (!String.IsNullOrEmpty(info.End))
                {
                    info.EndDay = DateTime.Parse(info.End);
                }
                var ob = ctrl.QN_CMS_Update(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_GetByUser(int CID, int Public, int LoaiTin, int CreatedBy, string TuNgay, string DenNgay, string TuKhoa, int Size, int Page)
        {
            try
            {
                DateTime NgayTu = Null.NullDate;
                if (!String.IsNullOrWhiteSpace(TuNgay))
                {
                    NgayTu = DateTime.Parse(TuNgay);
                }
                DateTime NgayDen = Null.NullDate;
                if (!String.IsNullOrWhiteSpace(DenNgay))
                {
                    NgayDen = DateTime.Parse(DenNgay);
                }
                var lst = ctrl.QN_CMS_GetByUser(UserInfo.UserID, PortalSettings.PortalId, CID, Public, LoaiTin, CreatedBy, NgayTu, NgayDen, TuKhoa, Size, Page);
                var recordCount = 0;
                if (lst.Count > 0)
                {
                    recordCount = lst[0].Total;
                }
                var pageCount = (int)Math.Floor(recordCount * 1.0 / Size) + (recordCount % Size == 0 && recordCount > Size ? 0 : 1);
                return Request.CreateResponse(HttpStatusCode.OK, new ResponsePagingServerStatus { status = "OK", result = lst, length = recordCount, totalpage = pageCount });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_UpdateCheck([FromBody] CMS_UpdateCheckInfo info)
        {
            try
            {
                info.UserID = UserInfo.UserID;
                ctrl.QN_CMS_UpdateCheck(info);
                var cache = MemoryCacheHelper.GetValue(PortalSettings.PortalId + "_CMSTinTuc");
                if (cache != null)
                {
                    List<QN_CMS> lst = cache as List<QN_CMS>;
                    var index = lst.FindLastIndex(x => x.UID == info.UID);
                    switch (info.Type)
                    {
                        case 1:
                            lst[index].IsCheckContent = (bool)info.Value;
                            break;
                        case 2:
                            lst[index].IsPublic = (bool)info.Value;
                            break;
                        case 3: 
                            lst[index].IsTit = (bool)info.Value;
                            break;
                        case 4: 
                            lst[index].IsHot = (bool)info.Value;
                            break;
                    }
                    MemoryCacheHelper.Add(PortalSettings.PortalId + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_UpdateSort([FromBody] BaseUpdateInfo info)
        {
            try
            {
                ctrl.QN_CMS_UpdateSort(info);
                var cache = MemoryCacheHelper.GetValue(PortalSettings.PortalId + "_CMSTinTuc");
                if (cache != null)
                {
                    List<QN_CMS> lst = cache as List<QN_CMS>;
                    var index = lst.FindLastIndex(x => x.UID == info.UID);
                    lst[index].SortOrder = (int)info.Value;
                    MemoryCacheHelper.Add(PortalSettings.PortalId + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Delete([FromBody] BaseUpdateInfo info)
        {
            try
            {
                info.UserID = UserInfo.UserID;
                ctrl.QN_CMS_Delete(info);
                var cache = MemoryCacheHelper.GetValue(PortalSettings.PortalId + "_CMSTinTuc");
                if (cache != null)
                {
                    List<QN_CMS> lst = cache as List<QN_CMS>;
                    var index = lst.FindLastIndex(x => x.UID == info.UID);
                    lst[index].IsDeleted = true;
                    MemoryCacheHelper.Add(PortalSettings.PortalId + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_GetUserCreate(string Type)
        {
            try
            {
                var ob = ctrl.QN_CMS_GetUserCreate(PortalSettings.PortalId, Type);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob } );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Policy_GetByUserPortal(int UserId)
        {
            try
            {
                var ob = ctrl.QN_Sys_Policy_GetByUserPortal(UserId, PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result= ob } );
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage UserPolicy_Update([FromBody] QN_Sys_UserPolicy info)
        {
            try
            {
                info.PortalID = PortalSettings.PortalId;
                if (info.IsSuDung)
                    ctrl.QN_Sys_UserPolicy_Update(info);
                else
                    ctrl.QN_Sys_UserPolicy_Delete(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage CMS_Count()
        {
            try
            {
                var ob = ctrl.QN_CMS_Count(PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result =ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }

        [HttpGet()]
        [DnnAuthorize()]
        public HttpResponseMessage CMS_GetTopView(int top)
        {
            try
            {
                var result = ctrl.QN_CMS_GetTopView(PortalSettings.PortalId, top);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = result });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpGet()]
        [DnnAuthorize]
        public HttpResponseMessage Users_Get(int TabModuleId)
        {
            try
            {
                var ob = ctrl.CMS_Users_Profile_Get(UserInfo.UserID);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", result = ob });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Users_Update(CMS_Users_Profile info)
        {
            try
            {
                info.UserID = UserInfo.UserID;
                ctrl.CMS_Users_Profile_Update(info);
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
        [HttpPost()]
        [DnnAuthorize]
        public HttpResponseMessage Users_UpdateImg()
        {
            try
            {
                CMS_Users_Profile info = new CMS_Users_Profile();
                info.UserID = UserInfo.UserID;
                string absolutePath = "/CMS/Avatars/" + UserInfo.UserID + "/";
                hp.CreateFolder(absolutePath);
                string sPath = HttpContext.Current.Server.MapPath(absolutePath);
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    System.Web.HttpPostedFile hpf = hfc[0];
                    if (hp.CheckFileIsImgage(hpf.FileName))
                        return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Định dạng không đúng!" });
                    if (hpf.ContentLength > 0)
                    {
                        var fileName = info.UserID + Path.GetExtension(hpf.FileName);
                        if (File.Exists(sPath + Path.GetFileName(fileName)))
                        {
                            File.Delete(sPath + Path.GetFileName(fileName));
                        }
                        hpf.SaveAs(sPath + "\\" + fileName);
                        info.Img = absolutePath + fileName;
                        ctrl.CMS_Users_Profile_UpdateImg(info);
                        return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", message = "Cập nhật thành công" });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "File đính kèm không hợp lệ" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Không có file!" });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}