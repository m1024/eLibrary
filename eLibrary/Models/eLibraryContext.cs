using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Security;

namespace eLibrary.Models
{
    public class eLibraryContext : DbContext
    {
        public DbSet<Role> role { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Author> author { get; set; }
        public DbSet<Book> book { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Genre> genre { get; set; }
        public DbSet<Series> serie { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasMany(c => c.Books)
                .WithMany(s => s.Authors)
                .Map(t => t.MapLeftKey("AuthorId")
                .MapRightKey("BookId")
                .ToTable("AuthorBook"));
        }
    }
}