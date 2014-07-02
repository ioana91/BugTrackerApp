using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Domain.Entities
{
    [Table("Issue")]
    public class Issue
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IssuePriority Priority { get; set; }
        public IssueStatus Status { get; set; }
        public IssueType Type { get; set; }
        public int ProjectId { get; set; }
        public int? MilestoneId { get; set; }
        [ForeignKey("Responsible")]
        public string ResponsibleId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("MilestoneId")]
        public virtual Milestone Milestone { get; set; }
        public virtual ApplicationUser Responsible { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<ApplicationUser> UsersInvolved { get; set; }

        public string Color { get; set; }
    }

    public enum IssueStatus
    {
        Open,
        InProgress,
        InTesting,
        Closed,
        Unsolvable
    }

    public enum IssuePriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum IssueType
    {
        Bug,
        Feature
    }
}
