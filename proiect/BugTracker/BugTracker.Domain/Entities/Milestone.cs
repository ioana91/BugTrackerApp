using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Domain.Entities
{
    [Table("Milestone")]
    public class Milestone
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MilestoneId { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }

        public virtual ICollection<Issue> Issues { get; set; }
    }
}
