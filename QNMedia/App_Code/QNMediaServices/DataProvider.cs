

using System;
using DotNetNuke;
using System.Data;

using DotNetNuke.Framework;

namespace QNMedia.CMS.Services
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
            objProvider = (DataProvider)Reflection.CreateObject("data", "QNMedia.CMS.Services", "");
        }

        // return the provider
        public static  DataProvider Instance() 
        {
            return objProvider;
        }

        #endregion

        #region Abstract methods
        public abstract void QN_LTC_Update();
        public abstract IDataReader QN_LTC_Get();
        public abstract IDataReader QN_Sys_Policy_Get(int UserId);
        public abstract IDataReader QN_Sys_Categorys_Get(string type, int portalid);

        #endregion

    }
}
