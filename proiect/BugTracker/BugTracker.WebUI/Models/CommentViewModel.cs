using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Domain.Entities;

namespace BugTracker.WebUI.Models
{
    public class CommentViewModel
    {
        public List<Comment> Comments { get; set; }
        public string IssueTitle { get; set; }
        public int IssueId { get; set; }
        public string NewComment { get; set; }
    }
}