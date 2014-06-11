using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Domain.Concrete;
using BugTracker.Domain.Entities;

namespace BugTracker.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
        BugTrackerDBContext GetContext { get; }

        GenericRepository<ApplicationUser> UserRepository { get; }
        GenericRepository<Comment> CommentRepository { get; }
        GenericRepository<Issue> IssueRepository { get; }
        GenericRepository<Milestone> MilestoneRepository { get; }
        GenericRepository<Project> ProjectRepository { get; }
        GenericRepository<Tag> TagRepository { get; }
    }
}
