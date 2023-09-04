
using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;

namespace QNMedia.CMS.SearchServices
{
    public class StatusInfo
    {
        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
    public class ZSearchInfo
    {
        public int ID { get; set; }
        public string SearchKey { get; set; }
        public string KeyQN { get; set; }
        public int CID { get; set; }
        public DateTime LastedSearch { get; set; }
    }
    public class CheckExists
    {
        public bool IsExists { get;set; }
    }
}