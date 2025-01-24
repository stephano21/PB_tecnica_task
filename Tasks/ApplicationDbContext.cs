using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using Tasks.Entities;

namespace Tasks
{
    public class ApplicationDbContext : IdentityDbContext<CustomUserIdentity>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<TaskNotes> TaskNotes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomUserIdentity>()
            .ToTable("Users", "AUTH");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsersToken", "AUTH");
            modelBuilder.Entity<IdentityRole>().ToTable("Role", "AUTH");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "AUTH");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "AUTH");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "AUTH");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "AUTH");
        }
    }
}
