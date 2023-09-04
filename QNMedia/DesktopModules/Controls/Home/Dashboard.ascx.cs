using DotNetNuke.Entities.Modules;
using System;
namespace QNMedia.Controls.TrangChu
{
    partial class Dashboard : PortalModuleBase
    {
        public string SkinPath { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SkinPath = "/Portals/_default/skins/AdminSkin/";
                }
            }
            catch (Exception ex) //Module failed to load
            {
            }
        }
    }
}