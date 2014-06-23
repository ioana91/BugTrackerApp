using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Domain.Interfaces;
using BugTracker.Domain.Entities;

namespace BugTracker.Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BugTrackerDBContext context;

        private IGenericRepository<ApplicationUser> userRepository;
        private IGenericRepository<Comment> commentRepository;
        private IGenericRepository<Issue> issueRepository;
        private IGenericRepository<Milestone> milestoneRepository;
        private IGenericRepository<Project> projectRepository;
        private IGenericRepository<Tag> tagRepository;

        public UnitOfWork(BugTrackerDBContext context, IGenericRepository<ApplicationUser> userRepository,
            IGenericRepository<Comment> commentRepository, IGenericRepository<Issue> issueRepository,
            IGenericRepository<Milestone> milestoneRepository, IGenericRepository<Project> projectRepository,
            IGenericRepository<Tag> tagRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
            this.issueRepository = issueRepository;
            this.milestoneRepository = milestoneRepository;
            this.projectRepository = projectRepository;
            this.tagRepository = tagRepository;
        }

        #region Repositories
        public IGenericRepository<ApplicationUser> UserRepository
        {
            get { return userRepository; }
        }

        public IGenericRepository<Comment> CommentRepository
        {
            get { return commentRepository; }
        }

        public IGenericRepository<Issue> IssueRepository
        {
            get { return issueRepository; }
        }

        public IGenericRepository<Milestone> MilestoneRepository
        {
            get { return milestoneRepository; }
        }

        public IGenericRepository<Project> ProjectRepository
        {
            get { return projectRepository; }
        }

        public IGenericRepository<Tag> TagRepository
        {
            get { return tagRepository; }
        }
        #endregion

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public BugTrackerDBContext GetContext
        {
            get { return context; }
        }

        #region IDisposable Implementation
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
