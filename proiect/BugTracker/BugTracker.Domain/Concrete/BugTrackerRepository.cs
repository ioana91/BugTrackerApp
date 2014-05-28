using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Domain.Abstract;
using BugTracker.Domain.Entities;

namespace BugTracker.Domain.Concrete
{
    public class BugTrackerRepository : IRepository
    {
        private BugTrackerDBContext context = new BugTrackerDBContext();

        public IQueryable<Entities.Comment> Comments
        {
            get { return context.Comments; }
        }

        public IQueryable<Entities.Issue> Issues
        {
            get { return context.Issues; }
        }

        public IQueryable<Entities.Milestone> Milestones
        {
            get { return context.Milestones; }
        }

        public IQueryable<Entities.Person> Persons
        {
            get { return context.Persons; }
        }

        public IQueryable<Entities.Project> Projects
        {
            get { return context.Projects; }
        }

        public IQueryable<Entities.Tag> Tags
        {
            get { return context.Tags; }
        }


        public void Add()
        {
            var milestone = new Milestone { MilestoneId = 1, Name = "the first", DueDate = System.DateTime.Now };
            context.Milestones.Add(milestone);
            context.SaveChanges();            
        }
    }
}
