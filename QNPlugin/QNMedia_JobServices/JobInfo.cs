
using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;

namespace QNMedia.Job.JobServices
{
    public class StatusInfo
    {
        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
    public class QN_Job
    {
        public int ID { get; set; }
        public int OgID { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string Required { get; set; }
        public string Benefits { get; set; }
        public string ContactTo { get; set; }
        public int Sex { get; set; }
        public int Old_From { get; set; }
        public int Old_To { get; set; }
        public int Salary_From { get; set; }
        public int Salary_To { get; set; }
        public int Job_Type { get; set; }
        public int Job_Level { get; set; }
        public int Job_Education { get; set; }
        public int Job_Language { get; set; }
        public int Job_KinhNghiem { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpirationOn { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public int LastedModifyBy { get; set; }
        public string OgName { get; set; }
        public string LevelName { get; set; }
        public string TypeName { get; set; }
        public string EducationName { get; set; }
        public string LanguageName { get; set; }
        public string KinhNghiem { get; set; }
        public string Job_NoiLamViec { get; set; }
        public string SourceID { get; set; }
        public string Job_Career { get; set; }
        public bool IsTit { get; set; }
        public bool IsHot { get; set; }
        public int TotalView { get; set; }
    }
    public class Job_View
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public string OgName { get; set; }
        public string Salary { get; set; }
        public string Level { get; set; }
        public string WorkPlace { get; set; }
        public string CreatedOn { get; set; }
        public string Career { get; set; }
    }
    public class QN_Job_Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }
        public string Img { get; set; }
        public string QuyMo { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public int LastedModifyBy { get; set; }
        public string SourceID { get; set; }
    }
    public class QN_Job_Category
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastedModifyOn { get; set; }
        public int LastedModifyBy { get; set; }
    }
    public class QN_Job_MoreInfo
    {
        public string type { get; set; }
        public int TypeID { get; set; }
        public int JobID { get; set; }
        public string Name { get; set; }
    }
    public class CheckExists
    {
        public bool IsExists { get; set; }
    }
}