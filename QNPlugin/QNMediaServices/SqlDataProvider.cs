using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace QNMedia.CMS.Services
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
        public override IDataReader QN_LTC_Get(int PortalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_LTC_Get",PortalId);
        }
        public override void QN_LTC_Update(int PortalId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_LTC_Update", PortalId);
        }
        public override IDataReader QN_Sys_Policy_Get(int UserId,int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Sys_Policy_Get", UserId, PortalID);
        }
        public override IDataReader QN_Sys_Categorys_Get( int portalid)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Sys_Categorys_Get", portalid);
        }
        public override IDataReader QN_Sys_Categorys_Update(QN_Sys_Categorys info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Sys_Categorys_Update", info.ID,info.PortalID,info.Type,info.ParentID,info.Title,info.CreatedBy,info.KeyWord);
        }
        public override IDataReader QN_Sys_Categorys_CheckUpdate(QN_Sys_Categorys info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Sys_Categorys_CheckUpdate", info.ID, info.PortalID, info.Type, info.Title);
        }
        public override void QN_Sys_Categorys_UpdatePublic(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_Sys_Categorys_UpdatePublic",info.ID,info.Value);
        }
        public override void QN_Sys_Categorys_UpdateSort(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_Sys_Categorys_UpdateSort", info.ID,info.Value);
        }
        public override IDataReader QN_Sys_GetUserPortal(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Sys_GetUserPortal", PortalID);
        }
        public override void QN_CMS_Policy_Delete(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_CMS_Policy_Delete", info.ID);
        }
        public override IDataReader QN_CMS_Policy_Get(int PortalID,string type)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Policy_Get", PortalID, type);
        }
        public override IDataReader QN_CMS_Policy_Update(QN_CMS_Policy info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Policy_Update", info.ID,info.UserID,info.Type,info.PortalId,info.Role,info.DisplayName);
        }
        public override IDataReader QN_CMS_UserPolicy_Update(QN_CMS_UserPolicy info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_UserPolicy_Update", info.PID,info.UserID,info.CID,info.AllowCreate,info.AllowCheckContent,info.AllowCheckPublic);
        }
        public override void QN_CMS_UserPolicy_Delete(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_CMS_UserPolicy_Delete", info.ID);
        }
        public override IDataReader QN_CMS_UserPolicy_Gets(int PID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_UserPolicy_Gets", PID);
        }
        public override IDataReader QN_CMS_Get(string UID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Get", UID);
        }
        public override IDataReader QN_CMS_GetByUser(int UserId, int PortalID, int CID, int Public, int LoaiTin, int CreateBy, DateTime TuNgay, DateTime DenNgay, string TuKhoa, int Size, int Page)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_GetByUser", UserId, PortalID, CID, Public, LoaiTin, CreateBy,GetNull(TuNgay),GetNull(DenNgay), TuKhoa, Size, Page);
        }
        public override IDataReader QN_CMS_Gets(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Gets", PortalID);
        }
        public override IDataReader QN_CMS_Update(QN_CMS info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Update", info.ID,info.PortalID,info.ParentID,info.CID,info.Language,info.Title,info.Intro_Img,info.Intro_Img_Lg,info.Intro_Content,info.Source,info.CreatedByName,info.Content,info.CreatedBy,info.KeyWord,GetNull(info.StartDay), GetNull(info.EndDay),info.Source_Name);
        }
        public override void QN_CMS_UpdateCheck(CMS_UpdateCheckInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_CMS_UpdateCheck", info.UID,info.Value,info.Type,info.UserID);
        }
        public override void QN_CMS_UpdateSort(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_CMS_UpdateSort", info.UID,info.Value);
        }
        public override IDataReader QN_CMS_UserPolicy_GetByUser(int UserID, string Type,int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_UserPolicy_GetByUser", UserID,  Type,PortalID);
        }
        public override IDataReader QN_CMS_Policy_GetByUser(int UserID, string Type, int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Policy_GetByUser", UserID,  Type,PortalID);
        }
        public override void QN_CMS_UpdateView(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_CMS_UpdateView", info.UID);
        }
        public override void QN_CMS_Delete(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_CMS_Delete", info.UID,info.UserID);
        }
        public override IDataReader QN_Sys_Policy_GetByUserPortal(int UserID, int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Sys_Policy_GetByUserPortal", UserID, PortalID);
        }
        public override void QN_Sys_UserPolicy_Delete(QN_Sys_UserPolicy info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_Sys_UserPolicy_Delete", info.UserID,info.PortalID,info.PID);
        }
        public override void QN_Sys_UserPolicy_Update(QN_Sys_UserPolicy info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "QN_Sys_UserPolicy_Update", info.UserID,info.PortalID,info.PID);
        }
        public override void CMS_Contact_Delete(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Contact_Delete", info.ID);
        }
        public override IDataReader CMS_Contact_Insert(CMS_Contact info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Contact_Insert",info.PortalID, info.HoVaTen,info.DienThoai,info.Email,info.DonVi,info.TieuDe,info.NoiDung,info.Type);
        }
        public override IDataReader CMS_Contact_Gets(int PortalID, int Public, int Deleted)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Contact_Gets", PortalID, Public, Deleted);
        }
        public override void CMS_Contact_Update(CMS_Contact info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Contact_Update", info.ID, info.TieuDe,info.NoiDung,info.GhiChu,info.Status,info.IsPublic,info.LastedBy);
        }
        public override void CMS_Contact_UpdatePublic(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Contact_UpdatePublic", info.ID,info.Value);
        }
        public override IDataReader CMS_GetTabModuleSettings(int TabModuleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_GetTabModuleSettings", TabModuleId);
        }
        public override void CMS_UpdateTabModuleSetting(TabModuleSettingInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_UpdateTabModuleSetting",info.TabModuleId, info.SettingName,info.SettingValue, info.UserID);
        }
        public override IDataReader CMS_PortalSettings_Get(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_PortalSettings_Get", PortalID);
        }
        public override IDataReader CMS_User_Folders_Gets()
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_User_Folders_Gets");
        }
        public override void CMS_User_Folders_UpdateActive(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_User_Folders_UpdateActive", info.ID,info.Value);
        }
        public override void CMS_User_Folders_UpdateSort(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_User_Folders_UpdateSort", info.ID, info.Value);
        }
        public override IDataReader CMS_User_Folders_Update(CMS_User_Folders info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_User_Folders_Update", info.ID,info.ParentID,info.Name,info.CreatedBy);
        }
        public override IDataReader CMS_Files_Add(CMS_Files info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Files_Add", info.FID,info.PortalID,info.FileURL,info.FileName,info.FileSize,info.CreatedBy,GetNull(info.SortOrder));
        }
        public override void CMS_Files_Delete(BaseUpdateInfo info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Files_Delete", info.ID);
        }
        public override IDataReader CMS_Files_Get(int CreatedBy)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Files_Get", CreatedBy);
        }
        public override void CMS_Users_Log_Update(CMS_Users_Log info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Users_Log_Update", info.UserId, info.LoginIn, info.LoginIP);
        }
        public override IDataReader CMS_Users_Log_Get(int UserId, int limit)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Users_Log_Get", UserId,  limit);
        }
        public override IDataReader QN_CMS_Count(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_Count", PortalID);
        }
        public override IDataReader QN_Contact_Count(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_Contact_Count", PortalID);
        }
        public override IDataReader QN_CMS_GetTopView(int PortalID, int Top)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "QN_CMS_GetTopView", PortalID, Top);
        }
        public override IDataReader CMS_Users_Profile_Get(int UserID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Users_Profile_Get", UserID);
        }
        public override void CMS_Users_Profile_Update(CMS_Users_Profile info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Users_Profile_Update", info.UserID, info.FirstName,info.LastName,info.Email);
        }
        public override void CMS_Users_Profile_UpdateImg(CMS_Users_Profile info)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Users_Profile_UpdateImg", info.UserID, info.Img);
        }
        public override IDataReader CMS_Video_Get(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Video_Get", PortalID);
        }
        public override void CMS_Video_Delete(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Video_Delete", info.ID);
        }
        public override void CMS_Video_Update_Public(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Video_Update_Public", info.ID,info.Value);
        }
        public override void CMS_Video_Update_View(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Video_Update_View", info.ID,info.Value);
        }
        public override IDataReader CMS_Video_Update(CMS_Video info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_Video_Update", info.ID,info.PortalID,info.CID,info.Title,info.Intro,info.Img,info.Img_Lg,info.URL,info.LastedModifyBy);
        }
        // van ban
        public override void CMS_VanBan_Delete(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_VanBan_Delete", info.ID);
        }
        public override void CMS_VanBan_File_Delete(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_VanBan_File_Delete", info.ID,info.Value);
        }
        public override IDataReader CMS_VanBan_File_GetByVB(int IDVB)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_VanBan_File_GetByVB", IDVB);
        }
        public override IDataReader CMS_VanBan_File_Update(CMS_VanBan_File info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_VanBan_File_Update", info.VBID,info.FileID);
        }
        public override IDataReader CMS_VanBan_Get(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_VanBan_Get", PortalID);
        }
        public override IDataReader CMS_VanBan_Update(CMS_Vanban info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_VanBan_Update", info.ID,info.PortalID,info.SoKyHieu,info.TrichYeu,info.CQID, info.LID, info.LVID,info.FileID,info.FileURL,info.NgayBanHanh,info.NgayHieuLuc,GetNull(info.NgayHetHieuLuc),info.CreatedBy);
        }
        public override void CMS_Files_UpdateSortOrder(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_Files_UpdateSortOrder", info.ID,info.Value);
        }
        public override void CMS_VanBan_UpdatePublic(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_VanBan_UpdatePublic", info.ID,info.Value);
        }
        public override void CMS_VanBan_UpdateView(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_VanBan_UpdateView", info.ID);
        }
        public override void CMS_VanBan_UpdateSortOrder(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_VanBan_UpdateSortOrder", info.ID,info.Value);
        }
        public override void CMS_DanhBaDienThoai_Delete(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_DanhBaDienThoai_Delete", info.ID);
        }
        public override IDataReader CMS_DanhBaDienThoai_Get(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_DanhBaDienThoai_Get", PortalID);
        }
        public override IDataReader CMS_DanhBaDienThoai_Update(CMS_DanhBaDienThoai info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_DanhBaDienThoai_Update", info.ID,info.CID,info.CVID,info.PortalID,info.HoLot,info.Ten,info.Img,info.GioiTinh,info.DiDong,info.CoDinh,info.Email,info.LastedModifyBy);
        }
        public override void CMS_DanhBaDienThoai_UpdateSortOrder(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_DanhBaDienThoai_UpdateSortOrder", info.ID,info.Value);
        }
        public override void CMS_DanhBaDienThoai_UpdateSuDung(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_DanhBaDienThoai_UpdateSuDung", info.ID,info.Value);
        }
        public override void CMS_LinkLienKet_Delete(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_LinkLienKet_Delete", info.ID);
        }
        public override IDataReader CMS_LinkLienKet_Get(int PortalID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_LinkLienKet_Get", PortalID);
        }
        public override IDataReader CMS_LinkLienKet_Update(CMS_LinkLienKet info)
        {
            return SqlHelper.ExecuteReader(ConnectionString, "CMS_LinkLienKet_Update", info.ID,info.CID,info.PortalID,info.URL,info.Img,info.Title,info.Target,info.LastedModifyBy);
        }
        public override void CMS_LinkLienKet_UpdateSortOrder(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_LinkLienKet_UpdateSortOrder", info.ID,info.Value);
        }
        public override void CMS_LinkLienKet_UpdateSuDung(BaseUpdateInfo info)
        {
             SqlHelper.ExecuteNonQuery(ConnectionString, "CMS_LinkLienKet_UpdateSuDung", info.ID,info.Value);
        }
        #endregion
    }
}
