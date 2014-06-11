using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using BugTracker.Domain.Entities;

namespace BugTracker.Domain.Concrete
{
    public class BugTrackerDBContext : IdentityDbContext<ApplicationUser>
    {
        public BugTrackerDBContext() : base("BugTrackerDBContext")
        {
        }

        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ApplicationUser>().
                HasMany(p => p.Comments).
                WithRequired(c => c.Author).
                HasForeignKey(c => c.AuthorId).
                WillCascadeOnDelete(value: false);

            modelBuilder.Entity<Issue>().
                HasOptional(i => i.Responsible).
                WithMany(p => p.Issues).
                HasForeignKey(i => i.ResponsibleId).
                WillCascadeOnDelete(false);

            modelBuilder.Entity<Issue>().
               HasMany(i => i.UsersInvolved).
               WithMany().
               Map(
                   m =>
                   {
                       m.MapLeftKey("IssueId");
                       m.MapRightKey("UserId");
                       m.ToTable("IssueUsers");
                   });

            modelBuilder.Entity<Tag>().
                HasMany(t => t.Issues).
                WithMany(i => i.Tags).
                Map(
                    m =>
                    {
                        m.MapLeftKey("TagId");
                        m.MapRightKey("IssueId");
                        m.ToTable("IssueTags");
                    });

            modelBuilder.Entity<Project>().
                HasMany(p => p.Users).
                WithMany(p => p.Projects).
                Map(
                    m =>
                    {
                        m.MapLeftKey("ProjectId");
                        m.MapRightKey("UserId");
                        m.ToTable("ProjectUsers");
                    });
        }
    }
}
