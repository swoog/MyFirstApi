using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cellenza.MyFirst.Domain
{
    public class MyFirstDbContext : DbContext
    {
        private readonly DatabaseConfig config;

        public MyFirstDbContext(DatabaseConfig config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                $"Server=(localdb)\\mssqllocaldb;Database={config.DataBaseName};Trusted_Connection=True;");

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Client> Clients { get; set; }
    }
}