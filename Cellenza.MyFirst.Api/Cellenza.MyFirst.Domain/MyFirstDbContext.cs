using Microsoft.EntityFrameworkCore;

namespace Cellenza.MyFirst.Domain
{
    public class MyFirstDbContext : DbContext
    {
        public MyFirstDbContext(DbContextOptions<MyFirstDbContext> contextOptions)
            : base(contextOptions)
        {

        }

        public DbSet<Client> Clients { get; set; }
    }
}