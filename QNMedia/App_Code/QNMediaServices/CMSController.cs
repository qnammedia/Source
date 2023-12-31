using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Web;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace QNMedia.CMS.Services
{

    public class CMSController
    {

        #region Constructors

        public CMSController()
        {
        }

        #endregion

        #region Public Methods
        public void QN_LTC_Update()
        {
            DataProvider.Instance().QN_LTC_Update();
        }
        public LTCInfo QN_LTC_Get()
        {
            return CBO.FillObject<LTCInfo>(DataProvider.Instance().QN_LTC_Get());
        }
        public List<QN_Sys_Policy> QN_Sys_Policy_Get(int UserId)
        {
            return CBO.FillCollection<QN_Sys_Policy>(DataProvider.Instance().QN_Sys_Policy_Get(UserId));
        }
        public List<QN_Sys_Categorys> QN_Sys_Categorys_Get(string type,int portalid)
        {
            return CBO.FillCollection<QN_Sys_Categorys>(DataProvider.Instance().QN_Sys_Categorys_Get(type,portalid));
        }
        #endregion

    }
}

