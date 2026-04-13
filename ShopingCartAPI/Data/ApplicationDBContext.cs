using Microsoft.EntityFrameworkCore;

namespace ShoppingCartAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
                
        }
        public DbSet<Models.CartHeader> CartHeaders { get; set; }
        public DbSet<Models.CartDetails> CartDetails { get; set; }
    }
}
