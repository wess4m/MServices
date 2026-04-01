using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
                
        }
        public DbSet<Models.Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Product>().HasData(new Models.Product()
            {
                ProductId = 1,
                Name = "Headphones",
                Price = 100,
                Description = "This is headphones",
                ImageUrl = "1.jpg"
            });
            modelBuilder.Entity<Models.Product>().HasData(new Models.Product()
            {
                ProductId = 2,
                Name = "Mobile",
                Price = 900,
                Description = "This is mobile",
                ImageUrl = "2.jpg"
            });
            modelBuilder.Entity<Models.Product>().HasData(new Models.Product()
            {
                ProductId = 3,
                Name = "Laptop",
                Price = 2500,
                Description = "This is laptop",
                ImageUrl = "3.jpg"
            });
        }
    }
}
