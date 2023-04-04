using Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class XmlImporterDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FullAddress> FullAddress { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShipInfo> ShipInfo { get; set; }

        public XmlImporterDbContext(DbContextOptions<XmlImporterDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
