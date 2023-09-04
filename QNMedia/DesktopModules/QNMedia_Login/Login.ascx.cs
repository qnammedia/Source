using System;
using System.Web.UI;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;

namespace QTI.SAMLSSO
{

    partial class Login : PortalModuleBase
    {

        #region Private Members

        private string DivInfo = @"<div style=""line-height:24px; width: 90%; color: blue; padding: 9px""><img src=""/Images/img/Info.png"" />";
        private string DivWarning = @"<div style=""line-height:24px; width: 90%; color: red; padding: 9px""><img src=""/Images/img/Warning.png"" />";

        #endregion

        public bool isLogin = false;
        public static string homepage { get; set; }
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {

                homepage = DotNetNuke.Common.Globals.NavigateURL(PortalSettings.HomeTabId);
                if (UserId > 0)
                {
                    if (Session["SSOLOUT"] != null)
                        Response.Redirect("Logoff.aspx");
                    else
                    {
                        
                            Response.Redirect(homepage);
                    }
                }
                if (Request.QueryString["Err"] != null)
                {
                    lblMS.Text = (string)Request.QueryString["Err"] + "<br/>";
                    lblMS.Visible = true;
                }
            }
            catch(Exception ex) {

            }
        }
        protected void LoginDNN(string username, string pass)
        {
            try
            {
                UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
                DotNetNuke.Entities.Users.UserInfo UI = DotNetNuke.Entities.Users.UserController.GetUserByName(this.PortalId, username);
                if (UI != null)
                {
                    DotNetNuke.Entities.Users.UserInfo objUserInfo = UserController.ValidateUser(this.PortalId, UI.Username,
                      pass, "DNN", "", PortalSettings.PortalName, this.Request.UserHostAddress, ref loginStatus);
                    if (loginStatus == UserLoginStatus.LOGIN_SUCCESS || loginStatus == UserLoginStatus.LOGIN_SUPERUSER)
                    {
                        bool isPersistent = false;
                        UserController.UserLogin(this.PortalId, UI, PortalSettings.PortalName, this.Request.UserHostAddress, isPersistent);
                        if (Request.QueryString["returnurl"] != null)
                            Response.Redirect(Request.QueryString["returnurl"]);
                        else
                            Response.Redirect(homepage);
                    }
                    else
                    {
                        lblMS.Visible = true;
                    }
                }
                else
                {
                    lblMS.Visible = true;
                }
            }
            catch(Exception ex)
            {
            }
        }
        protected void btnLogindnn_Click(object sender, EventArgs e)
        {
            if (UserId > 0)
            {
                Response.Redirect(homepage);
            }
            else
            {
                LoginDNN(txtUserName.Text.Trim(), txtPassWord.Text.Trim());
            }
        }
    }
}