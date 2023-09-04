using System;
using System.Web;

using System.Net.Http;
using DotNetNuke.Web.Api;
using System.Web.Http;
using System.Net;
using System.IO;
using DotNetNuke.Entities.Users;
using QNMedia.CMS.Services;
using QNMedia.Office;

namespace QNMedia.CMSAPI
{
    [QNMediaLicensePolicy]
     [DnnAuthorize]
    public class UserProfilesController : DnnApiController
    {
        CMSController ctrl = new CMSController();
        QNMediaHelper hp = new QNMediaHelper();
        [HttpGet()]
        public HttpResponseMessage Get()
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
        public HttpResponseMessage Update(CMS_Users_Profile info)
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
        public HttpResponseMessage UpdateImg()
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
                    if (!hp.CheckFileIsImgage(hpf.FileName))
                        return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Định dạng không đúng!" });
                    if (hpf.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(hpf.FileName);
                        if (File.Exists(sPath + Path.GetFileName(fileName)))
                        {
                            File.Delete(sPath + Path.GetFileName(fileName));
                        }
                        hpf.SaveAs(sPath + "\\" + fileName);
                        info.Img = absolutePath + fileName;
                        ctrl.CMS_Users_Profile_UpdateImg(info);
                        return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK",result= info.Img, message = "Cập nhật thành công" });
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
        [HttpPost]
        [DnnAuthorize()]
        public HttpResponseMessage Users_ChangePassWord([FromBody] User_ChangePWInfo info)
        {
            var PortalId = PortalSettings.PortalId;
            var username = info.UserName==null?UserInfo.Username: info.UserName;
            UserInfo thisUser = UserController.GetUserByName(PortalId, username);
            String newPassword = info.PassWord;
            UserController.ResetPasswordToken(thisUser); // set to 2 minute tuim
            String passwordResetToken = thisUser.PasswordResetToken.ToString();
            Boolean returnResult = false;
            try
            {
                returnResult = UserController.ChangePasswordByToken(thisUser.PortalID, thisUser.Username, newPassword, passwordResetToken);
                if(returnResult)
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "OK", message = "Cập nhật thành công!" });
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = "Cập nhật thất bại, vui lòng kiểm tra lại mật khẩu!" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new StatusInfo { status = "Failed", message = ex.Message });
            }
        }
    }
}