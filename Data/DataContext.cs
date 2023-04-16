using eCommerceAPI.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Data
{
    public class CommerceDbContext : DbContext
    {
        public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
