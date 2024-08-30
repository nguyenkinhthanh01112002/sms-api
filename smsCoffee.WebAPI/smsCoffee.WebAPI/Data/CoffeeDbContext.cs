using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using smsCoffee.WebAPI.Models;
using System.Data;

namespace smsCoffee.WebAPI.Data
{
    public class CoffeeDbContext : IdentityDbContext<AppUser>
    {
        public CoffeeDbContext(DbContextOptions<CoffeeDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
