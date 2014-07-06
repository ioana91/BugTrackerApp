using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.WebUI.Models
{
    public class EditIssueViewModel
    {
        public int IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public int MilestoneSelectId { get; set; }
        public string ResponsibleId { get; set; }
        public List<string> EditTags { get; set; }
    }
}