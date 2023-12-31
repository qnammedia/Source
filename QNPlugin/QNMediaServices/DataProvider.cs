

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
        public abstract void QN_LTC_Update(int PortalId);
        public abstract IDataReader QN_LTC_Get(int PortalId);
        public abstract IDataReader QN_Sys_Policy_Get(int UserId, int PortalID );
        public abstract IDataReader QN_Sys_Categorys_Get(int portalid);
        public abstract IDataReader QN_Sys_Categorys_CheckUpdate(QN_Sys_Categorys info);
        public abstract IDataReader QN_Sys_Categorys_Update(QN_Sys_Categorys info);
        public abstract void QN_Sys_Categorys_UpdateSort(BaseUpdateInfo info);
        public abstract void QN_Sys_Categorys_UpdatePublic(BaseUpdateInfo info);
        public abstract IDataReader QN_Sys_GetUserPortal(int PortalID);
        public abstract void QN_CMS_Policy_Delete(BaseUpdateInfo info);
        public abstract IDataReader QN_CMS_Policy_Get(int PortalID,string type);
        public abstract IDataReader QN_CMS_Policy_Update(QN_CMS_Policy info);
        public abstract IDataReader QN_CMS_UserPolicy_Update(QN_CMS_UserPolicy info);
        public abstract void QN_CMS_UserPolicy_Delete(BaseUpdateInfo info);
        public abstract IDataReader QN_CMS_UserPolicy_Gets(int PID);
        public abstract IDataReader QN_CMS_UserPolicy_GetByUser(int UserID, string Type,int PortalID);
        public abstract IDataReader QN_CMS_Update(QN_CMS info);
        public abstract IDataReader QN_CMS_Get(string UID);
        public abstract IDataReader QN_CMS_Gets(int PortalID);
        public abstract IDataReader QN_CMS_GetByUser(int UserId,int PortalID,int CID,int Public,int LoaiTin,int CreateBy,DateTime TuNgay,DateTime DenNgay,string TuKhoa,int Size,int Page);
        public abstract void QN_CMS_UpdateSort(BaseUpdateInfo info);
        public abstract void QN_CMS_UpdateCheck(CMS_UpdateCheckInfo info);
        public abstract IDataReader QN_CMS_Policy_GetByUser(int UserID, string Type, int PortalID);
        public abstract void QN_CMS_UpdateView(BaseUpdateInfo info);
        public abstract void QN_CMS_Delete(BaseUpdateInfo info);
        public abstract IDataReader QN_Sys_Policy_GetByUserPortal(int UserID, int PortalID);
        public abstract void QN_Sys_UserPolicy_Update(QN_Sys_UserPolicy info);
        public abstract void QN_Sys_UserPolicy_Delete(QN_Sys_UserPolicy info);
        public abstract IDataReader CMS_Contact_Insert(CMS_Contact info);
        public abstract void CMS_Contact_Update(CMS_Contact info);
        public abstract void CMS_Contact_UpdatePublic(BaseUpdateInfo info);
        public abstract void CMS_Contact_Delete(BaseUpdateInfo info);
        public abstract IDataReader CMS_Contact_Gets(int PortalID, int Public, int Deleted);
        public abstract void CMS_UpdateTabModuleSetting(TabModuleSettingInfo info);
        public abstract IDataReader CMS_GetTabModuleSettings(int TabModuleId);
        public abstract IDataReader CMS_PortalSettings_Get(int PortalID);
        // cms folder
        public abstract IDataReader CMS_User_Folders_Update(CMS_User_Folders info);
        public abstract void CMS_User_Folders_UpdateSort(BaseUpdateInfo info);
        public abstract void CMS_User_Folders_UpdateActive(BaseUpdateInfo info);
        public abstract IDataReader CMS_User_Folders_Gets();
        // cms file
        public abstract IDataReader CMS_Files_Add(CMS_Files info);
        public abstract IDataReader CMS_Files_Get(int CreatedBy);
        public abstract void CMS_Files_Delete(BaseUpdateInfo info);
        public abstract void CMS_Files_UpdateSortOrder(BaseUpdateInfo info);
        public abstract void CMS_Users_Log_Update(CMS_Users_Log info);
        public abstract IDataReader CMS_Users_Log_Get(int UserId, int limit);
        public abstract IDataReader QN_CMS_Count(int PortalID);
        public abstract IDataReader QN_Contact_Count(int PortalID);
        public abstract IDataReader QN_CMS_GetTopView(int PortalID, int Top);
        // user property
        public abstract IDataReader CMS_Users_Profile_Get(int UserID);
        public abstract void CMS_Users_Profile_Update(CMS_Users_Profile info);
        public abstract void CMS_Users_Profile_UpdateImg(CMS_Users_Profile info);
        // video
        public abstract IDataReader CMS_Video_Update(CMS_Video info);
        public abstract IDataReader CMS_Video_Get(int PortalID);
        public abstract void CMS_Video_Update_Public(BaseUpdateInfo info);
        public abstract void CMS_Video_Update_View(BaseUpdateInfo info);
        public abstract void CMS_Video_Delete(BaseUpdateInfo info);
        // van ban
        public abstract IDataReader CMS_VanBan_Update(CMS_Vanban info);
        public abstract IDataReader CMS_VanBan_Get(int PortalID);
        public abstract void CMS_VanBan_UpdatePublic(BaseUpdateInfo info);
        public abstract void CMS_VanBan_UpdateView(BaseUpdateInfo info);
        public abstract void CMS_VanBan_Delete(BaseUpdateInfo info);
        public abstract IDataReader CMS_VanBan_File_GetByVB(int IDVB);
        public abstract IDataReader CMS_VanBan_File_Update(CMS_VanBan_File info);
        public abstract void CMS_VanBan_File_Delete(BaseUpdateInfo info);
        public abstract void CMS_VanBan_UpdateSortOrder(BaseUpdateInfo info);
        // danh ba dien thoai
        public abstract IDataReader CMS_DanhBaDienThoai_Update(CMS_DanhBaDienThoai info);
        public abstract IDataReader CMS_DanhBaDienThoai_Get(int PortalID);
        public abstract void CMS_DanhBaDienThoai_UpdateSortOrder(BaseUpdateInfo info);
        public abstract void CMS_DanhBaDienThoai_UpdateSuDung(BaseUpdateInfo info);
        public abstract void CMS_DanhBaDienThoai_Delete(BaseUpdateInfo info);
        // link lien ket
        public abstract IDataReader CMS_LinkLienKet_Update(CMS_LinkLienKet info);
        public abstract IDataReader CMS_LinkLienKet_Get(int PortalID);
        public abstract void CMS_LinkLienKet_UpdateSortOrder(BaseUpdateInfo info);
        public abstract void CMS_LinkLienKet_UpdateSuDung(BaseUpdateInfo info);
        public abstract void CMS_LinkLienKet_Delete(BaseUpdateInfo info);
        #endregion

    }
}
