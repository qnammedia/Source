using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Services.Search.Controllers;
using HtmlAgilityPack;
using IISTask;
using QNMedia.CMS.Services;
using QNMedia.Office;

namespace QNMedia.Scheduler
{
    public class RefreshCacheScheduler : SchedulerClient
    {
        QNMedia.CMS.SearchServices.SearchController ctrl = new QNMedia.CMS.SearchServices.SearchController();
        CMSController cms = new CMSController();
        public RefreshCacheScheduler(ScheduleHistoryItem oItem) : base()
        {
            this.ScheduleHistoryItem = oItem;
        }
        public override void DoWork()
        {
            try
            {
                this.Progressing();
                {
                    cms.QN_CMS_ResetCache(0);
                    this.ScheduleHistoryItem.Succeeded = true;
                }
            }
            catch (Exception ex)
            {
                this.ScheduleHistoryItem.AddLogNote(ex.Message);
                this.ScheduleHistoryItem.Succeeded = false;
                this.Errored(ref ex);
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }

        }
    }
}