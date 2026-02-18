using Microsoft.EntityFrameworkCore;
using MsEShop.Services.CouponAPI.Models;

namespace MsEShop.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
