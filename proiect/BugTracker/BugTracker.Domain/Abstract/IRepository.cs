using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Domain.Entities;

namespace BugTracker.Domain.Abstract
{
    public interface IRepository
    {
        IQueryable<Comment> Comments { get; }
        IQueryable<Issue> Issues { get; }
        IQueryable<Milestone> Milestones { get; }
        IQueryable<Person> Persons { get; }
        IQueryable<Project> Projects { get; }
        IQueryable<Tag> Tags { get; }

        void Add();
    }
}
