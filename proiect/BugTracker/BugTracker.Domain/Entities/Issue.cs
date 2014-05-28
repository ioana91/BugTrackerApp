using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Domain.Entities
{
    public enum IssueStatus
    {
        Open,
        InProgress, 
        InTesting,
        Closed
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
        public int? ResponsibleId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("MilestoneId")]
        public virtual Milestone Milestone { get; set; }
        public virtual Person Responsible { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Person> PersonsInvolved { get; set; }
    }
}
