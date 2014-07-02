using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.WebUI.Models
{
    public class MilestoneViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Date { get; set; }
    }
}