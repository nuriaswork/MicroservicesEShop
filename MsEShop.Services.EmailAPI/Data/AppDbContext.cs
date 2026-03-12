using Microsoft.EntityFrameworkCore;
using MsEShop.Services.EmailAPI.Models;

namespace MsEShop.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }

     
    }
}
