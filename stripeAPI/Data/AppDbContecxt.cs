using Microsoft.EntityFrameworkCore;
using stripeAPI.Models;

namespace stripeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<CustomerOrder> CustomerOrders { get; set; }
    }
}
