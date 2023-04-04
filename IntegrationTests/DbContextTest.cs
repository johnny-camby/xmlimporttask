using Data.Repository;
using Data.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace IntegrationTests
{
    public class DbContextTest
    {
        [Fact]
        public void test_getcustomers()
        {
            var builder = new DbContextOptionsBuilder<XmlImporterDbContext>();
            builder.UseInMemoryDatabase("GetCustomers");
            Guid id = SeedCustomer(builder.Options);

            using var ctx = new XmlImporterDbContext(builder.Options);
            var data = ctx.Customers.FirstOrDefaultAsync(c => c.Id == id);
            Assert.Equal(id, data.Result.Id);
        }

        private Guid SeedCustomer(DbContextOptions<XmlImporterDbContext> options)
        {
            using var ctx = new XmlImporterDbContext(options);
            var customer = new Customer();
            ctx.Customers.Add(customer);
            ctx.SaveChanges();
            return customer.Id;
        }
    }
}
