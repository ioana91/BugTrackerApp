using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Domain.Entities;

namespace BugTracker.WebUI.Models
{
    public class MilestoneIndexViewModel
    {
        public IEnumerable<MilestoneDetails> MilestoneDetails { get; set; }
        public List<Project> Projects { get; set; }
    }

    public class MilestoneDetails
    {
        public int MilestoneId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }

        public float OpenIssues { get; set; }
        public float ClosedIssues { get; set; }
        public float InProgressIssues { get; set; }
        public float InTestingIssues { get; set; }
        public float UnsolvableIssues { get; set; }
    }
}