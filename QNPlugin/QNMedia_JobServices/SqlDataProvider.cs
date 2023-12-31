using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace QNMedia.Job.JobServices
{

    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "";

        private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private string _connectionString;
        private string _providerPath;
        private string _objectQualifier;
        private string _databaseOwner;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs new SqlDataProvider instance
        /// </summary>
        public SqlDataProvider()
        {
            //Read the configuration specific information for this provider
            Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            //Read the attributes for this provider
            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (_connectionString.Length == 0)
            {
                //Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];

            if ((_objectQualifier != "") && (_objectQualifier.EndsWith("_") == false))
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if ((_databaseOwner != "") && (_databaseOwner.EndsWith(".") == false))
            {
                _databaseOwner += ".";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the connection string
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Gets and sets the Provider path
        /// </summary>
        public string ProviderPath
        {
            get { return _providerPath; }
        }

        /// <summary>
        /// Gets and sets the Object qualifier
        /// </summary>
        public string ObjectQualifier
        {
            get { return _objectQualifier; }
        }

        /// <summary>
        /// Gets and sets the database ownere
        /// </summary>
        public string DatabaseOwner
        {
            get { return _databaseOwner; }
        }

        #endregion

        #region Private Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the fully qualified name of the stored procedure
        /// </summary>
        /// <param name="name">The name of the stored procedure</param>
        /// <returns>The fully qualified name</returns>
        /// -----------------------------------------------------------------------------
        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the value for the field or DbNull if field has "null" value
        /// </summary>
        /// <param name="Field">The field to evaluate</param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        private Object GetNull(Object Field)
        {
            return Null.GetNull(Field, DBNull.Value);
        }

        #endregion

        #region Public Methods
        public override IDataReader QN_Job_AutoUpdate(QN_Job info)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_AutoUpdate", info.OgID, info.Title,info.Info, info.Required, info.Benefits, info.ContactTo, info.Sex, info.Old_From, info.Old_To,info.Salary_From,info.Salary_To, info.TypeName, info.LevelName, info.EducationName, info.LanguageName,info.KinhNghiem, info.CreatedOn, info.ExpirationOn, info.SourceID, info.Job_Career,info.Job_NoiLamViec);
        }
        public override IDataReader QN_Job_CheckUpdate(string SourceID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_CheckUpdate", SourceID);
        }
        public override IDataReader QN_Job_Company_CheckUpdate(string Name)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_Company_CheckUpdate", Name);
        }
        public override IDataReader QN_Job_Company_Gets()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_Company_Gets");
        }
        public override IDataReader QN_Job_Company_Update(QN_Job_Company info)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_Company_Update",info.ID,info.Name,info.Address,info.Info,info.Img,info.QuyMo,info.LastedModifyBy,info.SourceID);
        }
        public override IDataReader QN_Job_Gets()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_Gets");
        }
        public override IDataReader QN_Job_Company_AutoCheckUpdate(string SourceID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_Company_AutoCheckUpdate", SourceID);
        }
        public override void QN_Job_UpdateBool(int ID, int Type, bool Value)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_Job_UpdateBool", ID,  Type,  Value);
        }
        public override void QN_Job_UpdateView(int ID)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_Job_UpdateView", ID);
        }
        public override IDataReader QN_Job_Category_Gets()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_Category_Gets");
        }
        public override IDataReader QN_Job_MoreInfo_Get(int ID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_Job_MoreInfo_Get", ID);
        }
        #endregion
    }
}
