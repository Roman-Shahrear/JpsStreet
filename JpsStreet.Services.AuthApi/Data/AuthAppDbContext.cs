using JpsStreet.Services.AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JpsStreet.Services.AuthApi.Data
{
    public class AuthAppDbContext : IdentityDbContext<ApplicationUser> // Before initialize ApplicationUser here need to keept IdentityUser
    {
        public AuthAppDbContext(DbContextOptions<AuthAppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
