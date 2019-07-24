using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Data.EfCore
{
    public class BlogContext: IdentityDbContext<AppUser>
    {
        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {

        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Categories> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add Identity related model configuration
            base.OnModelCreating(modelBuilder);

            // Your fluent modeling here
        }
    }
}
