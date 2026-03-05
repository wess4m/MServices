using Microsoft.EntityFrameworkCore;

namespace CouponAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
                
        }
        public DbSet<Models.Coupon> Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Coupon>().HasData(new Models.Coupon()
            {
                CouponId = 1,
                CouponCode = "Tr3cF",
                DiscountAmount = 5,
                MinAmount = 8
            });
            modelBuilder.Entity<Models.Coupon>().HasData(new Models.Coupon()
            {
                CouponId = 2,
                CouponCode = "Cr4cD",
                DiscountAmount = 15,
                MinAmount = 18
            });
            modelBuilder.Entity<Models.Coupon>().HasData(new Models.Coupon()
            {
                CouponId = 3,
                CouponCode = "Yr3sW",
                DiscountAmount = 10,
                MinAmount = 15
            });
        }
    }
}
