

using System;
using DotNetNuke;
using System.Data;

using DotNetNuke.Framework;
using QNMedia.CMS.Services;

namespace QNMedia.Job.JobServices
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
            objProvider = (DataProvider)Reflection.CreateObject("data", "QNMedia.Job.JobServices", "");
        }

        // return the provider
        public static  DataProvider Instance() 
        {
            return objProvider;
        }

        #endregion

        #region Abstract methods
        public abstract IDataReader QN_Job_Company_Update(QN_Job_Company info);
        public abstract IDataReader QN_Job_Company_Gets();
        public abstract IDataReader QN_Job_Company_CheckUpdate(string Name);
        public abstract IDataReader QN_Job_Company_AutoCheckUpdate(string SourceID);
        public abstract IDataReader QN_Job_CheckUpdate(string SourceID);
        public abstract IDataReader QN_Job_AutoUpdate(QN_Job info);
        public abstract IDataReader QN_Job_Gets();
        public abstract void QN_Job_UpdateView(int ID);
        public abstract void QN_Job_UpdateBool(int ID,int Type,bool Value);
        public abstract IDataReader QN_Job_Category_Gets();
        public abstract IDataReader QN_Job_MoreInfo_Get(int ID);
        #endregion

    }
}
