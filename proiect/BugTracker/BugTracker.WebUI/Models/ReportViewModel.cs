using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.WebUI.Models
{
    public class ReportViewModel
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }

        public string OpenName { get; set; }
        public int OpenIssues { get; set; }
        public string ClosedName { get; set; }
        public int ClosedIssues { get; set; }
        public string InProgressName { get; set; }
        public int InProgressIssues { get; set; }
        public string InTestingName { get; set; }
        public int InTestingIssues { get; set; }
        public string UnsolvableName { get; set; }
        public int UnsolvableIssues { get; set; }

        public string Low { get; set; }
        public int LowIssues { get; set; }
        public string Medium { get; set; }
        public int MediumIssues { get; set; }
        public string Critical { get; set; }
        public int CriticalIssues { get; set; }
        public string High { get; set; }
        public int HighIssues { get; set; }

        public string Bug { get; set; }
        public int BugIssues { get; set; }
        public string Feature { get; set; }
        public int FeatureIssues { get; set; }
    }

    public class UserDetails
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}