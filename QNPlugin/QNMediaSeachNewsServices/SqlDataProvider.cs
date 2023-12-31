using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace QNMedia.CMS.SearchServices
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
            get {   return _connectionString;   }
        }

        /// <summary>
        /// Gets and sets the Provider path
        /// </summary>
        public string ProviderPath
        {
            get {   return _providerPath;   }
        }

        /// <summary>
        /// Gets and sets the Object qualifier
        /// </summary>
        public string ObjectQualifier
        {
            get {   return _objectQualifier;   }
        }

        /// <summary>
        /// Gets and sets the database ownere
        /// </summary>
        public string DatabaseOwner
        {
            get {   return _databaseOwner;   }
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
        public override IDataReader CMS_ZSearch_Config_Gets()
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "CMS_ZSearch_Config_Gets");
        }
        public override IDataReader QN_CMS_CheckFilterExist(string id,string title)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_CheckFilterExist", id, title);
        }
        public override IDataReader QN_CMS_AutoUpdate(Services.QN_CMS info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_AutoUpdate", info.CID,info.Title,info.Intro_Img,info.Intro_Img_Lg,info.Intro_Content,info.Source,info.CreatedByName,info.Content,info.KeyWord,GetNull(info.StartDay), GetNull(info.EndDay),info.Source_Name, info.Source_ID,info.IsCheck);
        }
        public override void CMS_ZSearch_Config_UpdateTime(int ID)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_ZSearch_Config_UpdateTime", ID);
        }
        #endregion
    }
}
