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
using QNMedia.Office;

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
        public void QN_LTC_Update(int PortalId)
        {
            DataProvider.Instance().QN_LTC_Update(PortalId);
            var cache = MemoryCacheHelper.GetValue(PortalId + "_LTC");
            if (cache != null)
            {
                LTCInfo info = new LTCInfo();
                info = cache as LTCInfo;
                info.Total_Day++;
                info.Total_Month++;
                info.Total_Year++;
                MemoryCacheHelper.Add(PortalId + "_LTC", info, DateTimeOffset.UtcNow.AddMonths(1));
            }
        }
        public LTCInfo QN_LTC_Get(int PortalId)
        {
            LTCInfo info = new LTCInfo();
            var cache = MemoryCacheHelper.GetValue(PortalId + "_LTC");
            if (cache != null)
            {
                info = cache as LTCInfo;
            }
            else
            {
                info = CBO.FillObject<LTCInfo>(DataProvider.Instance().QN_LTC_Get(PortalId));
                MemoryCacheHelper.Add(PortalId + "_LTC", info, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return CBO.FillObject<LTCInfo>(DataProvider.Instance().QN_LTC_Get(PortalId));
        }
        public List<QN_Sys_Policy> QN_Sys_Policy_Get(int UserId, int PortalID)
        {
            return CBO.FillCollection<QN_Sys_Policy>(DataProvider.Instance().QN_Sys_Policy_Get(UserId, PortalID));
        }
        public List<QN_Sys_Categorys> QN_Sys_Categorys_Get(int portalid)
        {
            var lst = new List<QN_Sys_Categorys>();
            var cache = MemoryCacheHelper.GetValue(portalid + "_Categorys");
            if (cache != null)
            {
                lst = cache as List<QN_Sys_Categorys>;
            }
            else
            {
                lst = CBO.FillCollection<QN_Sys_Categorys>(DataProvider.Instance().QN_Sys_Categorys_Get( portalid));
                MemoryCacheHelper.Add(portalid + "_Categorys", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public BaseUpdateInfo QN_Sys_Categorys_CheckUpdate(QN_Sys_Categorys info)
        {
            return CBO.FillObject<BaseUpdateInfo>(DataProvider.Instance().QN_Sys_Categorys_CheckUpdate(info));
        }
        public QN_Sys_Categorys QN_Sys_Categorys_Update(QN_Sys_Categorys info)
        {
            var ob= CBO.FillObject<QN_Sys_Categorys>(DataProvider.Instance().QN_Sys_Categorys_Update(info));
            if (ob != null)
            {
                var lst = QN_Sys_Categorys_Get( info.PortalID);
                if (info.ID >0 )
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    lst[index] = ob;
                }
                else
                {
                    lst.Add(ob);
                }
                MemoryCacheHelper.Add(info.PortalID + "_Categorys", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return ob;
        }
        public void QN_Sys_Categorys_UpdateSort(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_Sys_Categorys_UpdateSort(info);
            var lst = QN_Sys_Categorys_Get( info.PortalID);
             var index = lst.FindLastIndex(x => x.ID == info.ID);
            lst[index].SortOrder = int.Parse(info.Value.ToString());
            MemoryCacheHelper.Add(info.PortalID + "_Categorys", lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public void QN_Sys_Categorys_UpdatePublic(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_Sys_Categorys_UpdatePublic(info);
            var lst = QN_Sys_Categorys_Get( info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            lst[index].IsPublic = bool.Parse(info.Value.ToString());
            MemoryCacheHelper.Add(info.PortalID + "_Categorys", lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public List<QN_UserInfo> QN_Sys_GetUserPortal(int PortalID)
        {
            return CBO.FillCollection<QN_UserInfo>(DataProvider.Instance().QN_Sys_GetUserPortal(PortalID));
        }
        public void QN_CMS_Policy_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_CMS_Policy_Delete(info);
        }
        public List<QN_CMS_Policy> QN_CMS_Policy_Get(int PortalID, string type)
        {
            return CBO.FillCollection<QN_CMS_Policy>(DataProvider.Instance().QN_CMS_Policy_Get(PortalID, type));
        }
        public QN_CMS_Policy QN_CMS_Policy_Update(QN_CMS_Policy info)
        {
            return CBO.FillObject<QN_CMS_Policy>(DataProvider.Instance().QN_CMS_Policy_Update(info));
        }
        public BaseUpdateInfo QN_CMS_UserPolicy_Update(QN_CMS_UserPolicy info)
        {
            return CBO.FillObject<BaseUpdateInfo>(DataProvider.Instance().QN_CMS_UserPolicy_Update(info));
        }
        public void QN_CMS_UserPolicy_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_CMS_UserPolicy_Delete(info);
        }
        public List<QN_CMS_UserPolicy> QN_CMS_UserPolicy_Gets(int PID)
        {
            return CBO.FillCollection<QN_CMS_UserPolicy>(DataProvider.Instance().QN_CMS_UserPolicy_Gets(PID));
        }
        public List<QN_CMS_UserPolicy> QN_CMS_UserPolicy_GetByUser(int UserID, string Type, int PortalID)
        {
            return CBO.FillCollection<QN_CMS_UserPolicy>(DataProvider.Instance().QN_CMS_UserPolicy_GetByUser(UserID, Type, PortalID));
        }
        public QN_CMS QN_CMS_Update(QN_CMS info)
        {
            var ob = CBO.FillObject<QN_CMS>(DataProvider.Instance().QN_CMS_Update(info));
            if (ob != null)
            {
                List<QN_CMS> lst = QN_CMS_Gets(info.PortalID);
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    lst[index] = ob;
                }
                else
                {
                    lst.ForEach(a => { a.SortOrder++; });
                    lst.Add(ob);
                }
                MemoryCacheHelper.Add(info.PortalID + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return ob;
        }
        public QN_CMS QN_CMS_Get(string UID)
        {
            return CBO.FillObject<QN_CMS>(DataProvider.Instance().QN_CMS_Get(UID));
        }
        public void QN_CMS_ResetCache(int PortalID)
        {
            List<QN_CMS> lst = CBO.FillCollection<QN_CMS>(DataProvider.Instance().QN_CMS_Gets(PortalID));
            MemoryCacheHelper.Add(PortalID + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public List<QN_CMS> QN_CMS_Gets(int PortalID)
        {
            List<QN_CMS> lst = new List<QN_CMS>();
            var cache = MemoryCacheHelper.GetValue(PortalID + "_CMSTinTuc");
            if (cache != null)
            {
                lst = cache as List<QN_CMS>;
            }
            else
            {
                lst = CBO.FillCollection<QN_CMS>(DataProvider.Instance().QN_CMS_Gets(PortalID));
                MemoryCacheHelper.Add(PortalID + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public List<QN_CMS_ListViewInfo> QN_CMS_GetByUser(int UserId, int PortalID, int CID, int Public, int LoaiTin, int CreateBy, DateTime TuNgay, DateTime DenNgay, string TuKhoa, int Size, int Page)
        {
            return CBO.FillCollection<QN_CMS_ListViewInfo>(DataProvider.Instance().QN_CMS_GetByUser(UserId, PortalID, CID, Public, LoaiTin, CreateBy, TuNgay, DenNgay, TuKhoa, Size, Page));
        }
        public void QN_CMS_UpdateSort(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_CMS_UpdateSort(info);
        }
        public void QN_CMS_UpdateCheck(CMS_UpdateCheckInfo info)
        {
            DataProvider.Instance().QN_CMS_UpdateCheck(info);
        }
        public QN_CMS_Policy QN_CMS_Policy_GetByUser(int UserID, string Type, int PortalID)
        {
            return CBO.FillObject<QN_CMS_Policy>(DataProvider.Instance().QN_CMS_Policy_GetByUser(UserID, Type, PortalID));
        }
        public void QN_CMS_UpdateView(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_CMS_UpdateView(info);
        }
        public void QN_CMS_UpdateViewPortal(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_CMS_UpdateView(info);
            List<QN_CMS> lst = new List<QN_CMS>();
            var cache = MemoryCacheHelper.GetValue(info.Value + "_CMSTinTuc");
            if (cache != null)
            {
                lst = cache as List<QN_CMS>;
                var index = lst.FindLastIndex(x => x.UID == info.UID);
                lst[index].TotalView = lst[index].TotalView + 1;
                MemoryCacheHelper.Add(info.Value + "_CMSTinTuc", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
        }
        public void QN_CMS_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().QN_CMS_Delete(info);
        }
        public List<QN_UserInfo> QN_CMS_GetUserCreate(int PortalID, string type)
        {
            return CBO.FillCollection<QN_UserInfo>(DataProvider.Instance().QN_CMS_Policy_Get(PortalID, type));
        }
        public List<QN_Sys_Policy> QN_Sys_Policy_GetByUserPortal(int UserID, int PortalID)
        {
            return CBO.FillCollection<QN_Sys_Policy>(DataProvider.Instance().QN_Sys_Policy_GetByUserPortal(UserID, PortalID));
        }
        public void QN_Sys_UserPolicy_Update(QN_Sys_UserPolicy info)
        {
            DataProvider.Instance().QN_Sys_UserPolicy_Update(info);
        }
        public void QN_Sys_UserPolicy_Delete(QN_Sys_UserPolicy info)
        {
            DataProvider.Instance().QN_Sys_UserPolicy_Delete(info);
        }
        public BaseUpdateInfo CMS_Contact_Insert(CMS_Contact info)
        {
            return CBO.FillObject<BaseUpdateInfo>(DataProvider.Instance().CMS_Contact_Insert(info));
        }
        public void CMS_Contact_Update(CMS_Contact info)
        {
            DataProvider.Instance().CMS_Contact_Update(info);
        }
        public void CMS_Contact_UpdatePublic(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_Contact_UpdatePublic(info);
        }
        public void CMS_Contact_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_Contact_Delete(info);
        }
        public List<CMS_Contact> CMS_Contact_Gets(int PortalID, int Public, int Deleted)
        {
            return CBO.FillCollection<CMS_Contact>(DataProvider.Instance().CMS_Contact_Gets(PortalID, Public, Deleted));
        }
        public void CMS_UpdateTabModuleSetting(TabModuleSettingInfo info)
        {
            DataProvider.Instance().CMS_UpdateTabModuleSetting(info);
        }
        public List<TabModuleSettingInfo> CMS_GetTabModuleSettings(int TabModuleId)
        {
            return CBO.FillCollection<TabModuleSettingInfo>(DataProvider.Instance().CMS_GetTabModuleSettings(TabModuleId));
        }
        public CMS_PortalSettings CMS_PortalSettings_Get(int PortalID)
        {
            return CBO.FillObject<CMS_PortalSettings>(DataProvider.Instance().CMS_PortalSettings_Get(PortalID));
        }
        public CMS_DetailInfo CMS_GetDetails(int PortalID, int ID)
        {
            try
            {
                List<QN_CMS> lst = QN_CMS_Gets(PortalID);
                CMS_DetailInfo info = new CMS_DetailInfo();
                info.Detail = lst.Where(x => x.ID == ID && x.IsPublic && !x.IsDeleted).FirstOrDefault();
                if (info.Detail != null)
                {
                    info.TinLienQuan = lst.Where(x => x.ID != info.Detail.ID && x.CID == info.Detail.CID && x.IsPublic && !x.IsDeleted && x.SortOrder >= info.Detail.SortOrder).OrderBy(x => x.SortOrder).Take(10).ToList();
                    var count = info.TinLienQuan.Count;
                    if (count < 10)
                    {
                        info.TinLienQuan.AddRange(lst.Where(x => x.ID != info.Detail.ID && x.CID != info.Detail.CID && x.ParentID == info.Detail.ParentID && x.IsPublic && !x.IsDeleted).OrderBy(x => x.SortOrder).Take(10 - count).ToList());
                    }
                }
                return info;
            }
            catch (Exception ex)
            {
                return new CMS_DetailInfo();
            }
        }
        public List<CMS_TinTuc_PublicInfo> CMS_GetByChuyenMuc(int PortalID, int tabmid, int CID)
        {
            try
            {
                List<QN_CMS> lst = QN_CMS_Gets(PortalID);
                List<TabModuleSettingInfo> lstsettings = CMS_GetTabModuleSettings(tabmid);
                QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();
                CMS_PublicInfo info = new CMS_PublicInfo { total = 10, countview = 5, height = 400 };
                var tt = hp.GetSettingValue(lstsettings, "Total");
                if (tt != null)
                {
                    info.total = int.Parse(tt);
                }
                return lst.Where(x => x.CID == CID && !x.IsDeleted && x.IsPublic).OrderBy(x => x.SortOrder).Take(info.total).Select(item => new CMS_TinTuc_PublicInfo()
                {
                    ID = item.ID,
                    Title = item.Title,
                    Intro_Content = item.Intro_Content,
                    Intro_Img = item.Intro_Img,
                    Intro_Img_Lg = item.Intro_Img_Lg,
                    SortOrder = item.SortOrder,
                    CreatedOn = item.LastedModifyOnHT,
                    CName = item.CName,
                    CID = item.CID,
                    TotalView = item.TotalView,
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<CMS_TinTuc_PublicInfo>();
            }
        }
        public List<CMS_TinTuc_PublicInfo> CMS_GetByModulePortal(int portalid, int tabmid)
        {
            try
            {
                List<TabModuleSettingInfo> lstsettings = CMS_GetTabModuleSettings(tabmid);
                if (lstsettings.Count == 0) return new List<CMS_TinTuc_PublicInfo>();
                List<QN_CMS> lst = QN_CMS_Gets(portalid);
                CMS_PublicInfo info = new CMS_PublicInfo { total = 10, countview = 5, height = 400 };
                var result = new List<CMS_TinTuc_PublicInfo>();
                var newlst = lst.Where(x => !x.IsDeleted && x.IsPublic).ToList();
                if (lst.Count == 0) return new List<CMS_TinTuc_PublicInfo>();
                QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();
                var all = hp.GetSettingValue(lstsettings, "All");
                var cm = hp.GetSettingValue(lstsettings, "Cat");
                var type = hp.GetSettingValue(lstsettings, "CMSType");
                var tt = hp.GetSettingValue(lstsettings, "Total");
                if (all == null || type == null || tt == null) return new List<CMS_TinTuc_PublicInfo>();
                if (!bool.Parse(all))
                {
                    if (String.IsNullOrEmpty(cm)) return new List<CMS_TinTuc_PublicInfo>();
                    if (String.IsNullOrEmpty(type)) return new List<CMS_TinTuc_PublicInfo>();
                    var lstcm = cm.Split(',');
                    newlst = newlst.Where(r => lstcm.Contains(r.CID.ToString())).ToList();
                }
                switch (type)
                {
                    case "TieuDiem": newlst = newlst.Where(r => r.IsTit).OrderBy(x => x.SortOrder).ToList(); break;
                    case "TinHot": newlst = newlst.Where(r => r.IsHot).OrderBy(x => x.SortOrder).ToList(); break;
                    case "TinMoi": newlst = newlst.OrderBy(x => x.SortOrder).ToList(); break;
                    case "TinDocNhieu": newlst = newlst.Where(x => (DateTime.Now - x.CreatedOn).TotalDays <= 7).OrderByDescending(x => x.TotalView).ToList(); break;
                    default: newlst = newlst.OrderBy(x => x.SortOrder).ToList(); break;
                }
                if (tt != null)
                {
                    info.total = int.Parse(tt);
                }
                result = newlst.Take(info.total).Select(item => new CMS_TinTuc_PublicInfo()
                {
                    ID = item.ID,
                    Title = item.Title,
                    Intro_Content = item.Intro_Content,
                    Intro_Img = item.Intro_Img,
                    Intro_Img_Lg = item.Intro_Img_Lg,
                    SortOrder = item.SortOrder,
                    CreatedOn = item.LastedModifyOnHT,
                    CName = item.CName,
                    CID = item.CID,
                    TotalView = item.TotalView,
                    Source_Name = item.Source_Name,
                    Source_ID = item.Source_ID
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<CMS_TinTuc_PublicInfo>();
            }
        }
        public CMS_User_Folders CMS_User_Folders_Update(CMS_User_Folders info)
        {
            var rs = CBO.FillObject<CMS_User_Folders>(DataProvider.Instance().CMS_User_Folders_Update(info));
            if (rs.ID > 0)
            {
                List<CMS_User_Folders> lst = new List<CMS_User_Folders>();
                var cache = MemoryCacheHelper.GetValue("CMSFolders");
                if (cache != null)
                {
                    lst = cache as List<CMS_User_Folders>;
                }
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0)
                    {
                        lst[index] = rs;
                    }
                }
                else
                {
                    lst.Add(rs);
                }
                MemoryCacheHelper.Add("CMSFolders", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return rs;
        }
        public void CMS_User_Folders_UpdateSort(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_User_Folders_UpdateSort(info);
            List<CMS_User_Folders> lst = new List<CMS_User_Folders>();
            var cache = MemoryCacheHelper.GetValue("CMSFolders");
            if (cache != null)
            {
                lst = cache as List<CMS_User_Folders>;
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0)
                    {
                        lst[index].SortOrder = int.Parse(info.Value.ToString());
                    }
                    MemoryCacheHelper.Add("CMSFolders", lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
            }
        }
        public void CMS_User_Folders_UpdateActive(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_User_Folders_UpdateActive(info);
            List<CMS_User_Folders> lst = new List<CMS_User_Folders>();
            var cache = MemoryCacheHelper.GetValue("CMSFolders");
            if (cache != null)
            {
                lst = cache as List<CMS_User_Folders>;
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0)
                    {
                        lst[index].IsActive = bool.Parse(info.Value.ToString());
                    }
                    MemoryCacheHelper.Add("CMSFolders", lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
            }
        }
        public List<CMS_User_Folders> CMS_User_Folders_Gets()
        {
            List<CMS_User_Folders> lst = new List<CMS_User_Folders>();
            var cache = MemoryCacheHelper.GetValue("CMSFolders");
            if (cache != null)
            {
                lst = cache as List<CMS_User_Folders>;
            }
            else
            {
                lst = CBO.FillCollection<CMS_User_Folders>(DataProvider.Instance().CMS_User_Folders_Gets());
                MemoryCacheHelper.Add("CMSFolders", lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public CMS_Files CMS_Files_Add(CMS_Files info)
        {
            var rs = CBO.FillObject<CMS_Files>(DataProvider.Instance().CMS_Files_Add(info));
            if (rs.ID > 0)
            {
                List<CMS_Files> lst = new List<CMS_Files>();
                var cache = MemoryCacheHelper.GetValue("CMS_Files_" + info.CreatedBy.ToString());
                if (cache != null)
                {
                    lst = cache as List<CMS_Files>;
                }
                lst.Add(rs);
                MemoryCacheHelper.Add("CMS_Files_" + info.CreatedBy.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return rs;
        }
        public List<CMS_Files> CMS_Files_Get(int CreatedBy, int FID)
        {
            List<CMS_Files> lst = new List<CMS_Files>();
            var cache = MemoryCacheHelper.GetValue("CMS_Files_" + CreatedBy.ToString());
            if (cache != null)
            {
                lst = cache as List<CMS_Files>;
            }
            else
            {
                lst = CBO.FillCollection<CMS_Files>(DataProvider.Instance().CMS_Files_Get(CreatedBy));
                MemoryCacheHelper.Add("CMS_Files_" + CreatedBy.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst.Where(x => x.FID == FID).ToList();
        }
        public string CMS_Files_Delete(BaseUpdateInfo info)
        {
            try
            {
                DataProvider.Instance().CMS_Files_Delete(info);
                var cache = MemoryCacheHelper.GetValue("CMS_Files_" + info.UserID.ToString());
                var lst = (cache as List<CMS_Files>).FindAll(x => x.ID != info.ID).ToList();
                MemoryCacheHelper.Add("CMS_Files_" + info.UserID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public void CMS_Files_UpdateSortOrder(BaseUpdateInfo info)
        {
             DataProvider.Instance().CMS_Files_UpdateSortOrder(info);
        }
        public void CMS_Users_Log_Update(CMS_Users_Log info)
        {
            DataProvider.Instance().CMS_Users_Log_Update(info);
        }
        public List<CMS_Users_Log> CMS_Users_Log_Get(int UserId, int limit)
        {
            return CBO.FillCollection<CMS_Users_Log>(DataProvider.Instance().CMS_Users_Log_Get(UserId, limit));
        }
        public LTCInfo QN_CMS_Count(int PortalID)
        {
            return CBO.FillObject<LTCInfo>(DataProvider.Instance().QN_CMS_Count(PortalID));
        }
        public Contact_Count QN_Contact_Count(int PortalID)
        {
            return CBO.FillObject<Contact_Count>(DataProvider.Instance().QN_Contact_Count(PortalID));
        }
        public List<QN_CMS> QN_CMS_GetTopView(int PortalID, int Top)
        {
            return CBO.FillCollection<QN_CMS>(DataProvider.Instance().QN_CMS_GetTopView(PortalID, Top));
        }
        public CMS_Users_Profile CMS_Users_Profile_Get(int UserID)
        {
            return CBO.FillObject<CMS_Users_Profile>(DataProvider.Instance().CMS_Users_Profile_Get(UserID));
        }
        public void CMS_Users_Profile_Update(CMS_Users_Profile info)
        {
            DataProvider.Instance().CMS_Users_Profile_Update(info);
        }
        public void CMS_Users_Profile_UpdateImg(CMS_Users_Profile info)
        {
            DataProvider.Instance().CMS_Users_Profile_UpdateImg(info);
        }
        public CMS_Video CMS_Video_Update(CMS_Video info)
        {
            var video = CBO.FillObject<CMS_Video>(DataProvider.Instance().CMS_Video_Update(info));
            if (video.ID > 0)
            {
                List<CMS_Video> lst = CMS_Video_Get(info.PortalID);
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0) lst[index] = video;
                }
                else
                {
                    lst.Add(video);
                    MemoryCacheHelper.Add("Video_" + video.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
            }
            return video;

        }
        public List<CMS_Video> CMS_Video_Get(int PortalID)
        {
            List<CMS_Video> lst = new List<CMS_Video>();
            var cache = MemoryCacheHelper.GetValue("Video_" + PortalID.ToString());
            if (cache != null)
            {
                lst = cache as List<CMS_Video>;
            }
            else
            {
                lst = CBO.FillCollection<CMS_Video>(DataProvider.Instance().CMS_Video_Get(PortalID));
                MemoryCacheHelper.Add("Video_" + PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public void CMS_Video_Update_Public(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_Video_Update_Public(info);
            List<CMS_Video> lst = CMS_Video_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].IsPublic = bool.Parse(info.Value.ToString());
            MemoryCacheHelper.Add("Video_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public void CMS_Video_Update_View(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_Video_Update_Public(info);
            List<CMS_Video> lst = CMS_Video_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].TotalView = lst[index].TotalView + 1; ;
            MemoryCacheHelper.Add("Video_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public void CMS_Video_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_Video_Delete(info);
            List<CMS_Video> lst = CMS_Video_Get(info.PortalID);
            lst = lst.Where(x => x.ID != info.ID).ToList();
            MemoryCacheHelper.Add("Video_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public List<CMS_Video> CMS_Video_GetByModule(int portalid, int tabmid)
        {
            try
            {
                List<TabModuleSettingInfo> lstsettings = CMS_GetTabModuleSettings(tabmid);
                if (lstsettings.Count == 0) return new List<CMS_Video>();
                List<CMS_Video> lst = CMS_Video_Get(portalid);
                var result = new List<CMS_Video>();
                var newlst = lst.Where(x => x.IsPublic).ToList();
                if (lst.Count == 0) return new List<CMS_Video>();
                QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();
                var all = hp.GetSettingValue(lstsettings, "All");
                var cm = hp.GetSettingValue(lstsettings, "Cat");
                var tt = hp.GetSettingValue(lstsettings, "Total");
                if (all == null ||  tt == null) return new List<CMS_Video>();
                if (!bool.Parse(all))
                {
                    if (String.IsNullOrEmpty(cm)) return new List<CMS_Video>();
                    var lstcm = cm.Split(',');
                    newlst = newlst.Where(r => lstcm.Contains(r.CID.ToString())).ToList();
                }
                result = newlst.OrderBy(x => x.SortOrder).Take(int.Parse(tt)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new List<CMS_Video>();
            }
        }
        // van ban
        public CMS_Vanban CMS_VanBan_Update(CMS_Vanban info)
        {
            var ob = CBO.FillObject<CMS_Vanban>(DataProvider.Instance().CMS_VanBan_Update(info));
            if (ob.ID > 0)
            {
                List<CMS_Vanban> lst = CMS_VanBan_Get(info.PortalID);
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0) lst[index] = ob;
                }
                else
                {
                    lst.ForEach(a => { a.SortOrder++; });
                    lst.Add(ob);
                    MemoryCacheHelper.Add("VanBan_" + ob.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
            }
            return ob;
        }
        public List<CMS_Vanban> CMS_VanBan_Get(int PortalID)
        {
            List<CMS_Vanban> lst = new List<CMS_Vanban>();
            var cache = MemoryCacheHelper.GetValue("VanBan_" + PortalID.ToString());
            if (cache != null)
            {
                lst = cache as List<CMS_Vanban>;
            }
            else
            {
                lst = CBO.FillCollection<CMS_Vanban>(DataProvider.Instance().CMS_VanBan_Get(PortalID));
                MemoryCacheHelper.Add("VanBan_" + PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public  void CMS_VanBan_UpdatePublic(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_VanBan_UpdatePublic(info);
            List<CMS_Vanban> lst = CMS_VanBan_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].IsPublic = bool.Parse(info.Value.ToString());
            MemoryCacheHelper.Add("VanBan_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }

        public void CMS_VanBan_UpdateView(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_VanBan_UpdateView(info);
            List<CMS_Vanban> lst = CMS_VanBan_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].TotalView = lst[index].TotalView + 1; ;
            MemoryCacheHelper.Add("VanBan_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public  void CMS_VanBan_UpdateSortOrder(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_VanBan_UpdateView(info);
            List<CMS_Vanban> lst = CMS_VanBan_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].SortOrder = int.Parse( info.Value.ToString()) ;
            MemoryCacheHelper.Add("VanBan_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }

        public void CMS_VanBan_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_VanBan_Delete(info);
            List<CMS_Vanban> lst = CMS_VanBan_Get(info.PortalID);
            lst = lst.Where(x => x.ID != info.ID).ToList();
            MemoryCacheHelper.Add("VanBan_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public List<CMS_VanBan_File> CMS_VanBan_File_GetByVB(int IDVB)
        {
            return CBO.FillCollection<CMS_VanBan_File>(DataProvider.Instance().CMS_VanBan_File_GetByVB(IDVB));
        }
        public CMS_VanBan_File CMS_VanBan_File_Update(CMS_VanBan_File info)
        {
            return CBO.FillObject<CMS_VanBan_File>(DataProvider.Instance().CMS_VanBan_File_Update(info));
        }
        public void CMS_VanBan_File_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_VanBan_File_Delete(info);
        }
        public List<CMS_Vanban> CMS_VanBan_GetByModule(int portalid, int tabmid)
        {
            try
            {
                List<TabModuleSettingInfo> lstsettings = CMS_GetTabModuleSettings(tabmid);
                if (lstsettings.Count == 0) return new List<CMS_Vanban>();
                List<CMS_Vanban> lst = CMS_VanBan_Get(portalid);
                var result = new List<CMS_Vanban>();
                var newlst = lst.Where(x => x.IsPublic).ToList();
                if (lst.Count == 0) return new List<CMS_Vanban>();
                QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();
                var tt = hp.GetSettingValue(lstsettings, "Total");
                var all_loai = hp.GetSettingValue(lstsettings, "AllL");
                var all_coquan = hp.GetSettingValue(lstsettings, "AllCQ");
                var all_linhvuc = hp.GetSettingValue(lstsettings, "AllLV");
                if (all_loai == null || all_coquan ==null || all_linhvuc == null || tt == null) return new List<CMS_Vanban>();
                if (!bool.Parse(all_loai))
                {
                    var loai = hp.GetSettingValue(lstsettings, "LoaiVB");

                    if (String.IsNullOrEmpty(loai)) return new List<CMS_Vanban>();
                    var lstcm = loai.Split(',');
                    newlst = newlst.Where(r => lstcm.Contains(r.LID.ToString())).ToList();
                }
                if (!bool.Parse(all_coquan))
                {
                    var data = hp.GetSettingValue(lstsettings, "CoQuan");
                    if (String.IsNullOrEmpty(data)) return new List<CMS_Vanban>();
                    var lstcm = data.Split(',');
                    newlst = newlst.Where(r => lstcm.Contains(r.CQID.ToString())).ToList();
                }
                if (!bool.Parse(all_linhvuc))
                {
                    var data = hp.GetSettingValue(lstsettings, "LinhVuc");
                    if (String.IsNullOrEmpty(data)) return new List<CMS_Vanban>();
                    var lstcm = data.Split(',');
                    newlst = newlst.Where(r => lstcm.Contains(r.LVID.ToString())).ToList();
                }
                if (int.Parse(tt) != 0)
                    result = newlst.OrderBy(x => x.SortOrder).Take(int.Parse(tt)).ToList();
                else
                    result = newlst.OrderBy(x => x.SortOrder).ToList();
                return result;
            }
            catch (Exception ex)
            {
                var lst = new List<CMS_Vanban>();
                lst.Add(new CMS_Vanban { TrichYeu = ex.Message });
                return lst;
            }
        }
        public List<CMS_Vanban> CMS_VanBan_GetVBKhac(int ID,int take,int Pid)
        {
            List<CMS_Vanban> lst = CMS_VanBan_Get(Pid).Where(x => x.IsPublic).ToList();
            var current = lst.Where(x => x.ID == ID).FirstOrDefault();
            if (current != null)
            {
                var rs = lst.Where(x => x.ID != current.ID && x.SortOrder >= current.SortOrder && x.PortalID == current.PortalID && x.LID == current.LID).OrderBy(x => x.SortOrder).Take(take).ToList();
                return rs;
            }
            return new List<CMS_Vanban>();
        }
        // danh ba dien thoai
        public CMS_DanhBaDienThoai CMS_DanhBaDienThoai_Update(CMS_DanhBaDienThoai info)
        {
            var ob = CBO.FillObject<CMS_DanhBaDienThoai>(DataProvider.Instance().CMS_DanhBaDienThoai_Update(info));
            if (ob.ID > 0)
            {
                List<CMS_DanhBaDienThoai> lst = CMS_DanhBaDienThoai_Get(info.PortalID);
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0) lst[index] = ob;
                }
                else
                {
                    lst.ForEach(a => { a.SortOrder++; });
                    lst.Add(ob);
                    MemoryCacheHelper.Add("VanBan_" + ob.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
            }
            return ob;
        }
        public List<CMS_DanhBaDienThoai> CMS_DanhBaDienThoai_Get(int PortalID)
        {
            List<CMS_DanhBaDienThoai> lst = new List<CMS_DanhBaDienThoai>();
            var cache = MemoryCacheHelper.GetValue("DanhBa_" + PortalID.ToString());
            if (cache != null)
            {
                lst = cache as List<CMS_DanhBaDienThoai>;
            }
            else
            {
                lst = CBO.FillCollection<CMS_DanhBaDienThoai>(DataProvider.Instance().CMS_DanhBaDienThoai_Get(PortalID));
                MemoryCacheHelper.Add("DanhBa_" + PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public  void CMS_DanhBaDienThoai_UpdateSortOrder(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_DanhBaDienThoai_UpdateSortOrder(info);
            List<CMS_DanhBaDienThoai> lst = CMS_DanhBaDienThoai_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].SortOrder = int.Parse(info.Value.ToString());
            MemoryCacheHelper.Add("DanhBa_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public  void CMS_DanhBaDienThoai_UpdateSuDung(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_DanhBaDienThoai_UpdateSuDung(info);
            List<CMS_DanhBaDienThoai> lst = CMS_DanhBaDienThoai_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].IsPublic = bool.Parse(info.Value.ToString());
            MemoryCacheHelper.Add("DanhBa_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public  void CMS_DanhBaDienThoai_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_DanhBaDienThoai_Delete(info);
            List<CMS_DanhBaDienThoai> lst = CMS_DanhBaDienThoai_Get(info.PortalID).Where(x=>x.ID!=info.ID).ToList();
            MemoryCacheHelper.Add("DanhBa_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        // link lien ket
        public CMS_LinkLienKet CMS_LinkLienKet_Update(CMS_LinkLienKet info)
        {
            var ob = CBO.FillObject<CMS_LinkLienKet>(DataProvider.Instance().CMS_LinkLienKet_Update(info));
            if (ob.ID > 0)
            {
                List<CMS_LinkLienKet> lst = CMS_LinkLienKet_Get(info.PortalID);
                if (info.ID > 0)
                {
                    var index = lst.FindLastIndex(x => x.ID == info.ID);
                    if (index >= 0) lst[index] = ob;
                }
                else
                {
                    lst.ForEach(a => { a.SortOrder++; });
                    lst.Add(ob);
                    MemoryCacheHelper.Add("VanBan_" + ob.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
                }
            }
            return ob;
        }
        public List<CMS_LinkLienKet> CMS_LinkLienKet_Get(int PortalID)
        {
            List<CMS_LinkLienKet> lst = new List<CMS_LinkLienKet>();
            var cache = MemoryCacheHelper.GetValue("Link_" + PortalID.ToString());
            if (cache != null)
            {
                lst = cache as List<CMS_LinkLienKet>;
            }
            else
            {
                lst = CBO.FillCollection<CMS_LinkLienKet>(DataProvider.Instance().CMS_LinkLienKet_Get(PortalID));
                MemoryCacheHelper.Add("Link_" + PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
            }
            return lst;
        }
        public  void CMS_LinkLienKet_UpdateSortOrder(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_LinkLienKet_UpdateSortOrder(info);
            List<CMS_LinkLienKet> lst = CMS_LinkLienKet_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].SortOrder = int.Parse(info.Value.ToString());
            MemoryCacheHelper.Add("Link_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public  void CMS_LinkLienKet_UpdateSuDung(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_LinkLienKet_UpdateSuDung(info);
            List<CMS_LinkLienKet> lst = CMS_LinkLienKet_Get(info.PortalID);
            var index = lst.FindLastIndex(x => x.ID == info.ID);
            if (index >= 0) lst[index].IsPublic = bool.Parse(info.Value.ToString());
            MemoryCacheHelper.Add("Link_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public  void CMS_LinkLienKet_Delete(BaseUpdateInfo info)
        {
            DataProvider.Instance().CMS_LinkLienKet_Delete(info);
            List<CMS_LinkLienKet> lst = CMS_LinkLienKet_Get(info.PortalID).Where(x => x.ID != info.ID).ToList();
            MemoryCacheHelper.Add("DanhBa_" + info.PortalID.ToString(), lst, DateTimeOffset.UtcNow.AddMonths(1));
        }
        public List<CMS_LinkLienKet> CMS_LinkLienKet_GetByModule(int portalid, int tabmid)
        {
            try
            {
                List<TabModuleSettingInfo> lstsettings = CMS_GetTabModuleSettings(tabmid);
                if (lstsettings.Count == 0) return new List<CMS_LinkLienKet>();
                List<CMS_LinkLienKet> lst = CMS_LinkLienKet_Get(portalid);
                var result = new List<CMS_LinkLienKet>();
                var newlst = lst.Where(x => x.IsPublic).ToList();
                if (lst.Count == 0) return new List<CMS_LinkLienKet>();
                QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();
                var all = hp.GetSettingValue(lstsettings, "All");
                if (!bool.Parse(all))
                {
                    var dm = hp.GetSettingValue(lstsettings, "CID");

                    if (String.IsNullOrEmpty(dm)) return new List<CMS_LinkLienKet>();
                    var lstcm = dm.Split(',');
                    newlst = newlst.Where(r => lstcm.Contains(r.CID.ToString())).ToList();
                }
                result = newlst.OrderBy(x => x.SortOrder).ToList();
                return result;
            }
            catch (Exception ex)
            {
                var lst = new List<CMS_LinkLienKet>();
                return lst;
            }
        }
        #endregion

    }
}

