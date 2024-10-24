using JpsStreet.Services.ShoppingCartApi.Models;
using JpsStreet.Services.ShoppingCartApi.Models.DTo;
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
        //public DbSet<CouponDTo> Coupons { get; set; }
    }
}
