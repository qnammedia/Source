
using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;

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
        public int Total_Year { get; set; }
        public int Total_Month { get; set; }
        public int Total_Day { get; set; }
    }
    public class QN_Sys_Categorys
    {
        public int ID { get; set; }
        public string Type { get; set; }
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
    public class QN_CMS_UserPolicy
    {
        public int UserID { get; set; }
        public int CID { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowCheck { get; set; }
    }
    public class QN_CMS
    {
        public int ID { get; set; }
        public string UID { get; set; }
        public int CID { get; set; }
        public string Title { get; set; }
        public string Intro_Img { get; set; }
        public string Intro_Content { get; set; }
        public string From { get; set; }
        public string CreatedByName { get; set; }
        public string Content { get; set; }
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastedModify { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public string KeyWord { get; set; }
        public bool IsPublic { get; set; }
        public int PublicBy { get; set; }
        public DateTime PublicOn { get; set; }
        public bool IsTit { get; set; }
        public bool IsHot { get; set; }
        public int TotalView { get; set; }
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
    }
}