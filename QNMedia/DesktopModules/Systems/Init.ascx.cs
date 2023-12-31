using System;
using System.Data;
using System.Web.UI;
using DotNetNuke.Entities.Modules;
using QNMedia.CMS.Services;

namespace QNMedia.Sytems
{

    partial class Init : PortalModuleBase
    {
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //if (UserInfo.IsSuperUser || UserInfo.IsAdmin)
                    //    ltrscript.Text = "";
                    //else ltrscript.Text = "<script src='/Resources/Shared/scripts/jquery/jquery.js' ></script>";
                    if (Session["NewAccess"] == null)
                    {
                        CMSController ctrl = new CMSController();
                        ctrl.QN_LTC_Update();
                        Session["NewAccess"] = DateTime.Now;
                    }
                }
            }
            catch (Exception ex) //Module failed to load
            {
            }
        }
    
    }
}

