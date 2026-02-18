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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed table with initial values

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 1,
                Code = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 2,
                Code = "20OFF",
                DiscountAmount = 20,
                MinAmount = 40,
                CreatedOn = DateTime.Now,
                LastUpdatedOn = DateTime.Now
            });
        }
    }
}
