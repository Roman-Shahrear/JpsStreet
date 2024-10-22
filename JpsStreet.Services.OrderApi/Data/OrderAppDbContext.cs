using JpsStreet.Services.OrderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JpsStreet.Services.OrderApi.Data
{
    public class OrderAppDbContext : DbContext
    {
        public OrderAppDbContext(DbContextOptions<OrderAppDbContext> options) : base(options)
        { 
        }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
