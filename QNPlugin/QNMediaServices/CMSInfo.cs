
using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;

namespace QNMedia.CMS.Services
{
    public class StatusInfo
    {
        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
    public class LTCInfo
    {
        public int Total { get; set; }
        public int Total_Year { get; set; }
        public int Total_Month { get; set; }
        public int Total_Day { get; set; }
    }
    public class Contact_Count
    {
        public int Total { get; set; }
        public int ChuaXuLy { get; set; }
    }
    public class QN_Sys_Categorys
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public int PortalID { get; set; }
        public int ParentID { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
        public bool IsPublic { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastedModify { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string KeyWord { get; set; }
        public int CountChild { get; set; }
    }
    public class QN_CMS
    {
        public int ID { get; set; }
        public int PortalID { get; set; }
        public int ParentID { get; set; }
        public string UID { get; set; }
        public int CID { get; set; }
        public string CName { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Intro_Img { get; set; }
        public string Intro_Img_Lg { get; set; }
        public string Intro_Content { get; set; }
        public string Source { get; set; }
        public string Source_Name { get; set; }
        public string Source_ID { get; set; }
        public string CreatedByName { get; set; }
        public string Content { get; set; }
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastedModify { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string KeyWord { get; set; }
        public bool IsCheckContent { get; set; }
        public int CheckContentBy { get; set; }
        public DateTime CheckContentOn { get; set; }
        public bool IsPublic { get; set; }
        public int PublicBy { get; set; }
        public DateTime PublicOn { get; set; }
        public bool IsTit { get; set; }
        public bool IsHot { get; set; }
        public int TotalView { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public string StartDayHT
        {
            get { return StartDay.ToString("yyyy") != "0001" ? StartDay.ToString("HH:mm dd/MM/yyyy") : ""; }
        }
        public string EndDayHT
        {
            get { return EndDay.ToString("yyyy") != "0001" ? EndDay.ToString("HH:mm dd/MM/yyyy") : ""; }
        }
        public int Total { get; set; }
        public String LastedModifyOnHT
        {
            get { return LastedModifyOn.ToString("HH:mm dd/MM/yyyy"); }
        }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool IsCheck { get; set; }
    }
    public class BaseUpdateInfo
    {
        public int ID { get; set; }
        public string UID { get; set; }
        public object Value { get; set; }
        public object Note { get; set; }
        public int UserID { get; set; }
        public int PortalID { get; set; }
    }
    public class CMS_Files
    {
        public int ID { get; set; }
        public int FID { get; set; }
        public int PortalID { get; set; }
        public string FileURL { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnHT { get { return CreatedOn.ToString("HH:mm dd/MM/yyyy"); } }
        public int SortOrder { get; set; }
        public string FileNameWithoutExtension { get { return Path.GetFileNameWithoutExtension(FileURL); } }
        public string FileExtension { get { return Path.GetExtension(FileURL); } }
        public double FileSizeMB { get { return Math.Round(FileSize * 1.0 / (1024 * 1024), 2); } }

        public string IconClass
        {
            get
            {
                var iconclass = "bi bi-file-earmark";
                if (".jpg,jpeg,.bmp,.gif,.png".Contains(FileExtension)) return "bi bi-file-image";
                if (".mp3".Contains(FileExtension)) return "bi bi-file-music";
                if (".doc,.docx".Contains(FileExtension)) return "bi bi-file-earmark-word";
                if (".xls,.xlsx".Contains(FileExtension)) return "bi bi-file-excel";
                if (".ppt,.pptx".Contains(FileExtension)) return "bi bi-file-earmark-ppt";
                if (".txt".Contains(FileExtension)) return "bi bi-file-earmark-text";
                if (".zip,.7zip,.7z,.gzip,.gz,.rar,.tar,.tz,.iso".Contains(FileExtension)) return "bi bi-file-zip";
                if (".pdf".Contains(FileExtension)) return "bi bi-file-earmark-pdf";
                if (".avi,.flv,.mp4,.mp3,.mkv,.mpg,.3gp,.mov,.wmv,.mpeg,.webm".Contains(FileExtension)) return "bi bi-camera-video";
                return iconclass;
            }
        }
    }
    public class ResponseStatus
    {
        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
        public int length { get; set; }
    }
    public class ResponsePagingServerStatus
    {
        public string status { get; set; }
        public object result { get; set; }
        public int length { get; set; }
        public int totalpage { get; set; }
        public string message { get; set; }
    }
    public class QN_CMS_Policy
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PortalId { get; set; }
        public string Type { get; set; }
        public int Role { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string RoleName
        {
            get { return Role == 0 ? "Toàn quyền" : "Tùy chọn"; }
        }
        public virtual List<QN_CMS_UserPolicy> lstUserPolicy { get; set; }
    }
    public class QN_Sys_Policy
    {
        public string ID { get; set; }
        public string Parent { get; set; }
        public string Href { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string Class { get; set; }
        public string IconClass { get; set; }
        public string InitFucntion { get; set; }
        public int SID { get; set; }
        public int PID { get; set; }
        public int UserID { get; set; }
        public int PortalId { get; set; }
        public bool IsSuDung { get; set; }
    }
    public class QN_CMS_UserPolicy
    {
        public int CID { get; set; }
        public int ParentID { get; set; }
        public int UserID { get; set; }
        public int ID { get; set; }
        public int PID { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowCheckContent { get; set; }
        public bool AllowCheckPublic { get; set; }
        public string Title { get; set; }
    }
    public class QN_UserInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
    }
    public class CMS_UpdateCheckInfo
    {
        public string UID { get; set; }
        public int UserID { get; set; }
        public int Type { get; set; }
        public bool Value { get; set; }

    }
    public class QN_CMS_ListViewInfo
    {
        public int PortalID { get; set; }
        public int ParentID { get; set; }
        public int CID { get; set; }
        public string CName { get; set; }
        public string UID { get; set; }
        public string Title { get; set; }
        public string Intro_Img { get; set; }
        public string CreatedByName { get; set; }
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }
        public int LastedModify { get; set; }
        public string LastedModifyBy { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string KeyWord { get; set; }
        public bool IsCheckContent { get; set; }
        public int CheckContentBy { get; set; }
        public DateTime CheckContentOn { get; set; }
        public bool IsPublic { get; set; }
        public bool IsTit { get; set; }
        public bool IsHot { get; set; }
        public int TotalView { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public int Total { get; set; }
        public String LastedModifyOnHT
        {
            get { return LastedModifyOn.ToString("HH:mm dd/MM/yyyy"); }
        }
        public bool IsConHan
        {
            get { return EndDay.ToString("yyyy") == "0001" || DateTime.Now > EndDay; }
        }
    }
    public class QN_Sys_UserPolicy
    {
        public int UserID { get; set; }
        public string PID { get; set; }
        public int PortalID { get; set; }
        public bool IsSuDung { get; set; }
    }
    public class CMS_Contact
    {
        public int ID { get; set; }
        public string HoVaTen { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string DonVi { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnHT { get { return CreatedOn.ToString("HH:mm dd/MM/yyyy"); } }
        public string GhiChu { get; set; }
        public int Status { get; set; }
        public DateTime LastedUpdateOn { get; set; }
        public int LastedBy { get; set; }
        public string LastedUpdateOnHT { get { return LastedUpdateOn.ToString("HH:mm dd/MM/yyyy"); } }
        public string DisplayName { get; set; }
        public int Type { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; }
        public int PortalID { get; set; }
    }
    public class TabModuleSettingInfo
    {
        public int TabModuleId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public int UserID { get; set; }
        public int TabId { get; set; }
    }
    public class CMS_PublicInfo
    {
        public List<QN_CMS> data { get; set; }
        public int height { get; set; }
        public int total { get; set; }
        public int countview { get; set; }
    }
    public class CMS_PortalSettings
    {
        public int PortalID { get; set; }
        public int ParentID { get; set; }
        public bool IsMain { get; set; }
        public string OgName { get; set; }
        public string CMS_Detail_URL { get; set; }
        public string CMS_ListTag_URL { get; set; }
        public string CMS_ListCat_URL { get; set; }
        public string CMS_VideoPage { get; set; }
        public string CMS_VanBanPage { get; set; }
        public string CMS_VanBan_Detail { get; set; }
    }
    public class CMS_DetailInfo
    {
        public QN_CMS Detail { get; set; }
        public List<QN_CMS> TinLienQuan { get; set; }
    }
    public class CMS_TinTuc_PublicInfo
    {
        public int ID { get; set; }
        public int CID { get; set; }
        public string Title { get; set; }
        public string Intro_Content { get; set; }
        public string Intro_Img { get; set; }
        public string Intro_Img_Lg { get; set; }
        public int SortOrder { get; set; }
        public string CreatedOn { get; set; }
        public string CName { get; set; }
        public int TotalView { get; set; }
        public string Source_Name { get; set; }
        public string Source_ID { get; set; }
    }
    public class CMS_User_Folders
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastedModifyOn { get; set; }
    }
    public class CMS_Users_Log
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public DateTime LoginOn { get; set; }
        public string LoginOnHT
        {
            get
            {
                var t = (DateTime.Now - LoginOn).TotalDays;
                if (t < 1)
                {
                    var t2 = (DateTime.Now - LoginOn).TotalHours;
                    if (t2 < 1)
                    {
                        var t3 = (int)Math.Ceiling((DateTime.Now - LoginOn).TotalMinutes);
                        return  t3 + " phút";
                    }
                    return (int)Math.Ceiling(t2) + " giờ";
                }
               return (int)Math.Ceiling(t) + " ngày";
            }
        }
        public string LoginIn { get; set; }
        public string LoginIP { get; set; }
    }
    public class CMS_Users_Profile
    {
        public int UserID { get; set; }
        public string Img { get; set; }
        public string LastedMobileToken { get; set; }
        public DateTime LoginMobileOn { get; set; }
        public string LastedWebToken { get; set; }
        public DateTime LoginWebOn { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
    public class User_ChangePWInfo
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string RePassWord { get; set; }
    }
    public class CMS_Video
    {
        public int ID { get; set; }
        public int PortalID { get; set; }
        public int CID { get; set; }
        public string CName { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Img { get; set; }
        public string Img_Lg { get; set; }
        public string URL { get; set; }
        public int SortOrder { get; set; }
        public int TotalView { get; set; }
        public int LastedModifyBy { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string LastedModifyOnHT { get { return LastedModifyOn.ToString("HH:mm dd/MM/yyyy"); } }
        public string LastedModifyByName { get; set; }
        public bool IsPublic { get; set; }
    }
    public class CMS_Vanban
    {
        public int ID { get; set; }
        public string SoKyHieu { get; set; }
        public string TrichYeu { get; set; }
        public int CQID { get; set; }
        public int LID { get; set; }
        public int LVID { get; set; }
        public DateTime NgayBanHanh { get; set; }
        public string NgayBanHanhHT { get { return NgayBanHanh.ToString("dd/MM/yyyy"); } }
        public DateTime NgayHieuLuc { get; set; }
        public string NgayHieuLucHT { get { return NgayHieuLuc.ToString("dd/MM/yyyy"); } }
        public DateTime NgayHetHieuLuc { get; set; }
        public string NgayHetHieuLucHT { get { return NgayHetHieuLuc.ToString("yyyy")!="0001"? NgayHetHieuLuc.ToString("dd/MM/yyyy"):""; } }
        public bool IsConHieuLuc
        {
            get
            {
                if (NgayHieuLuc.ToString("yyyy") != "0001") return true;
                return Math.Round((DateTime.Now - NgayHetHieuLuc).TotalDays) >= 0;
            }
        }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastedModifyBy { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string LastedModifyOnHT { get { return LastedModifyOn.ToString("HH:mm dd/MM/yyyy"); } }
        public bool IsPublic { get; set; }
        public int TotalView { get; set; }
        public int PortalID { get; set; }
        public int FileID { get; set; }
        public string FileURL { get; set; }
        public string TenCoQuan { get; set; }
        public string TenLoai { get; set; }
        public string TenLinhVuc { get; set; }
        public string LastedModifyByName { get; set; }
        public virtual CMS_Files FileChinh { get; set; }
        public List<CMS_VanBan_File> LstFileKhac { get; set; }
        public int SortOrder { get; set; }
    }
    public class CMS_VanBan_File : CMS_Files 
    {
        public int KID { get; set; }
        public int VBID { get; set; }
        public int FileID { get; set; }
    }
    public class CMS_DanhBaDienThoai
    {
        public int ID { get; set; }
        public int CID { get; set; }
        public int CVID { get; set; }
        public int PortalID { get; set; }
        public string HoLot { get; set; }
        public string Ten { get; set; }
        public string Img { get; set; }
        public int GioiTinh { get; set; }
        public string DiDong { get; set; }
        public string CoDinh { get; set; }
        public string Email { get; set; }
        public int SortOrder { get; set; }
        public int CSortOrder { get; set; }
        public bool IsPublic { get; set; }
        public int LastedModifyBy { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string TenPhongBan { get; set; }
        public string TenChucVu { get; set; }
    }
    public class CMS_LinkLienKet
    {
        public int ID { get; set; }
        public int CID { get; set; }
        public int PortalID { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string Target { get; set; }
        public int SortOrder { get; set; }
        public bool IsPublic { get; set; }
        public int LastedModifyBy { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string CName { get; set; }
    }
}