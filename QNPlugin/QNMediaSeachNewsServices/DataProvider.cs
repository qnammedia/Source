

using System;
using DotNetNuke;
using System.Data;

using DotNetNuke.Framework;
using QNMedia.CMS.Services;

namespace QNMedia.CMS.SearchServices
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// An abstract class that provides the DAL contract
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    /// </history>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {

    #region Shared/Static Methods

        // singleton reference to the instantiated object 
        static DataProvider  objProvider = null;

        // constructor
        static DataProvider()
        {
            CreateProvider();
        }

        // dynamically create provider
        private static void CreateProvider()
        {
            objProvider = (DataProvider)Reflection.CreateObject("data", "QNMedia.CMS.SearchServices", "");
        }

        // return the provider
        public static  DataProvider Instance() 
        {
            return objProvider;
        }

        #endregion

        #region Abstract methods
        public abstract IDataReader CMS_ZSearch_Config_Gets();
        public abstract IDataReader QN_CMS_CheckFilterExist(string id,string title);
        public abstract IDataReader QN_CMS_AutoUpdate(QN_CMS info);
        public abstract void CMS_ZSearch_Config_UpdateTime(int ID);
        #endregion

    }
}
