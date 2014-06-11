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

        private GenericRepository<ApplicationUser> userRepository;
        private GenericRepository<Comment> commentRepository;
        private GenericRepository<Issue> issueRepository;
        private GenericRepository<Milestone> milestoneRepository;
        private GenericRepository<Project> projectRepository;
        private GenericRepository<Tag> tagRepository;

        public UnitOfWork(BugTrackerDBContext context)
        {
            this.context = context;
        }

        #region Repositories
        public GenericRepository<ApplicationUser> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<ApplicationUser>(context);
                }
                return userRepository;
            }
        }

        public GenericRepository<Comment> CommentRepository
        {
            get 
            {
                if (this.commentRepository == null)
                {
                    this.commentRepository = new GenericRepository<Comment>(context);
                }
                return commentRepository;
            }
        }

        public GenericRepository<Issue> IssueRepository
        {
            get
            {
                if (this.issueRepository == null)
                {
                    this.issueRepository = new GenericRepository<Issue>(context);
                }
                return issueRepository;
            }
        }

        public GenericRepository<Milestone> MilestoneRepository
        {
            get
            {
                if (this.milestoneRepository == null)
                {
                    this.milestoneRepository = new GenericRepository<Milestone>(context);
                }
                return milestoneRepository;
            }
        }

        public GenericRepository<Project> ProjectRepository
        {
            get
            {
                if (this.projectRepository == null)
                {
                    this.projectRepository = new GenericRepository<Project>(context);
                }
                return projectRepository;
            }
        }

        public GenericRepository<Tag> TagRepository
        {
            get
            {
                if (this.tagRepository == null)
                {
                    this.tagRepository = new GenericRepository<Tag>(context);
                }
                return tagRepository;
            }
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
