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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);        
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }    
    }
}
