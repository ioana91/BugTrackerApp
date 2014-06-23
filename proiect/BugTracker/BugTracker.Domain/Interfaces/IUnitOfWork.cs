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

        IGenericRepository<ApplicationUser> UserRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Issue> IssueRepository { get; }
        IGenericRepository<Milestone> MilestoneRepository { get; }
        IGenericRepository<Project> ProjectRepository { get; }
        IGenericRepository<Tag> TagRepository { get; }
    }
}
