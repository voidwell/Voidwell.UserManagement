using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Voidwell.UserManagement.Data.Models;
using System;

namespace Voidwell.UserManagement.Data
{
    public class UserDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(a =>
            {
                a.Property(u => u.Id).HasDefaultValue(Guid.NewGuid());
            });

            builder.Entity<ApplicationRole>(a =>
            {
                a.Property(u => u.Id).HasDefaultValue(Guid.NewGuid());
            });

            builder.Entity<SecurityQuestion>()
                .HasKey(a => new { a.UserId, a.Question });
        }
    }
}
