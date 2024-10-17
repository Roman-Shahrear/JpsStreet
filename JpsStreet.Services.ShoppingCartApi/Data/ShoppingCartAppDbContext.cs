using JpsStreet.Services.ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JpsStreet.Services.ShoppingCartApi.Data
{
    public class ShoppingCartAppDbContext : DbContext
    {
        public ShoppingCartAppDbContext(DbContextOptions<ShoppingCartAppDbContext> options) : base(options)
        {
        }

        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
    }
}
