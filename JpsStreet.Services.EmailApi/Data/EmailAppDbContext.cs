using JpsStreet.Services.EmailApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JpsStreet.Services.EmailApi.Data
{
    public class EmailAppDbContext : DbContext
    {
        public EmailAppDbContext(DbContextOptions<EmailAppDbContext> options) : base(options)
        { 
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}
