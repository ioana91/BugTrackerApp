using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BugTracker.Domain.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BugTracker.Domain.Concrete
{
    public class BugTrackerDBContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public BugTrackerDBContext() : base("BugTrackerDBContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Person>().
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
               HasMany(i => i.PersonsInvolved).
               WithMany().
               Map(
                   m =>
                   {
                       m.MapLeftKey("IssueId");
                       m.MapRightKey("PersonId");
                       m.ToTable("IssuePersons");
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
                HasMany(p => p.Persons).
                WithMany(p => p.Projects).
                Map(
                    m =>
                    {
                        m.MapLeftKey("ProjectId");
                        m.MapRightKey("PersonId");
                        m.ToTable("ProjectPersons");
                    });
        }
    }
}
