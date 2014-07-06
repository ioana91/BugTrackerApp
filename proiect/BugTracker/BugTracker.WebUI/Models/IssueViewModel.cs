using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.WebUI.Models
{
    public class IssueViewModel
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public int ProjectSelectId { get; set; }
        public int MilestoneSelectId { get; set; }
        public List<string> Tags { get; set; }
    }
}