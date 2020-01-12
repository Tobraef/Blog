using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.Entity;

namespace Blog.Models
{
    public class BlogContext : DbContext
    {
        public DbSet<BlogEntry> Entries
        {
            get;
            set;
        }

        public DbSet<Account> Accounts
        {
            get;
            set;
        }

        public DbSet<Comment> Comments
        {
            get;
            set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BlogEntry>().HasOptional(x => x.Author)
                .WithOptionalDependent().WillCascadeOnDelete(false);

        }

        public BlogContext()
            : base("Context")
        {
        }
    }
}