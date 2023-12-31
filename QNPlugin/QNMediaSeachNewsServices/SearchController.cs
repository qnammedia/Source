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
using System.Linq;
using QNMedia.CMS.Services;

namespace QNMedia.CMS.SearchServices
{
    public class SearchController
    {
        QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();

        #region Constructors

        public SearchController()
        {
        }

        #endregion

        #region Public Methods
        public List<ZSearchInfo> CMS_ZSearch_Config_Gets()
        {
            return  CBO.FillCollection<ZSearchInfo>(DataProvider.Instance().CMS_ZSearch_Config_Gets());
        }
        public bool QN_CMS_CheckFilterExist(string id,string title)
        {
            return CBO.FillObject<CheckExists>(DataProvider.Instance().QN_CMS_CheckFilterExist(id,  title)).IsExists;
        }
        public  void CMS_ZSearch_Config_UpdateTime(int ID)
        {
            DataProvider.Instance().CMS_ZSearch_Config_UpdateTime(ID);
        }

        public QN_CMS QN_CMS_AutoUpdate(QN_CMS info)
        {
            var ob = CBO.FillObject<QN_CMS>(DataProvider.Instance().QN_CMS_AutoUpdate(info));
            //if (ob != null)
            //{
            //    var cache = MemoryCacheHelper.GetValue(ob.PortalID + "_CMSTinTuc");
            //    if (cache != null)
            //    {
            //        List<QN_CMS> lst = cache as List<QN_CMS>;
            //        lst.Add(ob);
            //        MemoryCacheHelper.Add(ob.PortalID + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
            //    }
            //}
            return ob;
        }
        #endregion

    }
}

