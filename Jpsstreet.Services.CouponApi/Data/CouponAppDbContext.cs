using Jpsstreet.Services.CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Jpsstreet.Services.CouponApi.Data
{
    public class CouponAppDbContext : DbContext
    {
        public CouponAppDbContext(DbContextOptions<CouponAppDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(
            new Coupon {
                CouponId = 1,
                CouponCode = "100ff",
                DiscountAmount = 10,
                MinAmount = 20,
            },
            new Coupon {
                 CouponId = 2,
                 CouponCode = "200ff",
                 DiscountAmount = 20,
                 MinAmount = 40
            });
        }
    }
}
