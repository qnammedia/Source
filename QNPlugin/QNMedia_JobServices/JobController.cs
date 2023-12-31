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
using QNMedia.CMS.Services;

namespace QNMedia.Job.JobServices
{
    public class JobController
    {
        QNMedia.Office.QNMediaHelper hp = new Office.QNMediaHelper();

        #region Constructors

        public JobController()
        {
        }

        #endregion

        #region Public Methods
        public QN_Job_Company QN_Job_Company_Update(QN_Job_Company info)
        {
            if (!QN_Job_Company_CheckUpdate(info.Name))
            {
                var ob = CBO.FillObject<QN_Job_Company>(DataProvider.Instance().QN_Job_Company_Update(info));
                var lst = QN_Job_Company_Gets();
                lst.Add(ob);
                lst = lst.OrderBy(x => x.SortOrder).ToList();
                MemoryCacheHelper.Add("Job_Company", lst, DateTime.Now.AddMonths(1));
                return ob;
            }
            else
            {
                return new QN_Job_Company();
            }
        }
        public QN_Job_Company QN_Job_Company_AutoUpdate(QN_Job_Company info)
        {
            var ob = CBO.FillObject<QN_Job_Company>(DataProvider.Instance().QN_Job_Company_Update(info));
            var lst = QN_Job_Company_Gets();
            lst.Add(ob);
            lst = lst.OrderBy(x => x.SortOrder).ToList();
            MemoryCacheHelper.Add("Job_Company", lst, DateTime.Now.AddMonths(1));
            return ob;
        }
        public List<QN_Job_Company> QN_Job_Company_Gets()
        {
            List<QN_Job_Company> lst = new List<QN_Job_Company>();
            var cache = MemoryCacheHelper.GetValue("Job_Company");
            if (cache != null)
            {
                lst = cache as List<QN_Job_Company>;
            }
            else
            {
                lst = CBO.FillCollection<QN_Job_Company>(DataProvider.Instance().QN_Job_Company_Gets());
                MemoryCacheHelper.Add("Job_Company", lst, DateTime.Now.AddMonths(1));
            }
            return lst;
        }
        public bool QN_Job_Company_CheckUpdate(string Name)
        {
            return CBO.FillObject<CheckExists>(DataProvider.Instance().QN_Job_Company_CheckUpdate(Name)).IsExists;
        }
        public bool QN_Job_CheckUpdate(string SourceID)
        {
            return CBO.FillObject<CheckExists>(DataProvider.Instance().QN_Job_CheckUpdate(SourceID)).IsExists;
        }
        public QN_Job_Company QN_Job_Company_AutoCheckUpdate(string SourceID)
        {
            return CBO.FillObject<QN_Job_Company>(DataProvider.Instance().QN_Job_Company_AutoCheckUpdate(SourceID));
        }
        public void QN_Job_AutoUpdate(QN_Job info)
        {

            var job = CBO.FillObject<QN_Job>(DataProvider.Instance().QN_Job_AutoUpdate(info));
            if (job != null)
            {
                List<QN_Job> lst = QN_Job_Gets();
                lst.Add(job);
                MemoryCacheHelper.Add("Jobs", lst, DateTime.Now.AddMonths(1));
            }
        }
        public List<QN_Job> QN_Job_Gets()
        {
            List<QN_Job> lst = new List<QN_Job>();
            var cache = MemoryCacheHelper.GetValue("Jobs");
            if (cache != null)
            {
                lst = cache as List<QN_Job>;
            }
            else
            {
                lst = CBO.FillCollection<QN_Job>(DataProvider.Instance().QN_Job_Gets());
                MemoryCacheHelper.Add("Jobs", lst, DateTime.Now.AddMonths(1));
            }
            return lst;
        }
        public List<QN_Job_Category> QN_Job_Category_Gets()
        {
            List<QN_Job_Category> lst = new List<QN_Job_Category>();
            var cache = MemoryCacheHelper.GetValue("Job_Cats");
            if (cache != null)
            {
                lst = cache as List<QN_Job_Category>;
            }
            else
            {
                lst = CBO.FillCollection<QN_Job_Category>(DataProvider.Instance().QN_Job_Category_Gets());
                MemoryCacheHelper.Add("Job_Cats", lst, DateTime.Now.AddMonths(1));
            }
            return lst;
        }
        public QN_Job QN_Job_Get(int ID)
        {
            return QN_Job_Gets().Where(x => x.ID == ID).FirstOrDefault();
        }
        public List<Job_View> QN_Job_GetListView(string type, int total,int curentid=0)
        {
            var lst = QN_Job_Gets();
            switch (type)
            {
                case "New": lst = lst.Where(x => !x.IsDeleted&&(x.ExpirationOn - DateTime.Now).TotalDays>-1).OrderByDescending(x => x.ID).Take(total).ToList(); break;
                case "Hot": lst = lst.Where(x => !x.IsDeleted && x.IsHot && (x.ExpirationOn - DateTime.Now).TotalDays > -1).OrderByDescending(x => x.ID).Take(total).ToList(); break;
                case "Tit": lst = lst.Where(x => !x.IsDeleted && x.IsTit && (x.ExpirationOn - DateTime.Now).TotalDays > -1).OrderByDescending(x => x.ID).Take(total).ToList(); break;
                case "Khac": lst = lst.Where(x =>  !x.IsDeleted &&x.ID<= curentid&&(x.ExpirationOn - DateTime.Now).TotalDays>-1).OrderByDescending(x => x.ID).Take(total).ToList(); break;
            }
            var cps = QN_Job_Company_Gets();
            List<Job_View> data = new List<Job_View>(); 
            lst.ForEach(item =>
            {
                var cp = cps.Where(x => x.ID == item.OgID).First();
                var salary = "Thương lượng";
                if(item.Salary_From != 0 && item.Salary_To != 0)
                {
                    salary = "Từ " + item.Salary_From + " đến " + item.Salary_To + " triệu";
                }
                else 
                {
                    if (item.Salary_From != 0 && item.Salary_To == 0)
                    {
                        salary = "Trên " + item.Salary_From + " triệu";
                    }
                }
                data.Add(new Job_View
                {
                    ID = item.ID,
                    Title = item.Title,
                    Salary = salary,
                    Level  = item.LevelName,
                    CreatedOn = (DateTime.Now -  item.CreatedOn).TotalDays<=1?"Vừa mới đăng": (int)Math.Floor((DateTime.Now - item.CreatedOn).TotalDays) + " ngày trước",
                    Img = cp.Img,
                    OgName = cp.Name,
                    WorkPlace = item.Job_NoiLamViec,
                });
            });
            return data;
        }
        public void QN_Job_UpdateView(int ID)
        {
            DataProvider.Instance().QN_Job_UpdateView(ID);
            var lst = QN_Job_Gets();
            var index = lst.FindLastIndex(x => x.ID == ID);
            if (index >= 0)
            {
                lst[index].TotalView = lst[index].TotalView + 1;
                MemoryCacheHelper.Add("Jobs", lst, DateTime.Now.AddMonths(1));
            }
        }
        public  void QN_Job_UpdateBool(int ID, int Type, bool Value)
        {
            DataProvider.Instance().QN_Job_UpdateBool(ID, Type,Value);
            var lst = QN_Job_Gets();
            var index = lst.FindLastIndex(x => x.ID == ID);
            if (index >= 0)
            {
                switch (Type)
                {
                    case 0: lst[index].IsDeleted = Value;break;
                    case 1: lst[index].IsHot = Value;break;
                    case 2: lst[index].IsTit = Value;break;
                }
                MemoryCacheHelper.Add("Jobs", lst, DateTime.Now.AddMonths(1));
            }
        }
        public List<QN_Job_MoreInfo> QN_Job_MoreInfo_Get(int ID)
        {
            return CBO.FillCollection<QN_Job_MoreInfo>(DataProvider.Instance().QN_Job_MoreInfo_Get(ID));
        }

        #endregion

    }
}

