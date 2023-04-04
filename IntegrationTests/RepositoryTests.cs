using Data.Repository;
using Data.Repository.Entities;
using Data.Repository.Interfaces;
using Data.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace IntegrationTests
{
    public class RepositoryTests
    {
        private readonly DbContextOptions<XmlImporterDbContext> _dbContextOptions;

        public RepositoryTests()
        {
            var databaseName = Guid.NewGuid().ToString();
            _dbContextOptions = new DbContextOptionsBuilder<XmlImporterDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private IXmlImporterRepository<Customer> GetCustomerRepository(XmlImporterDbContext context)
        {
            IXmlImporterRepository<Customer> customerRepo = new CustomerRepository(context);
            return customerRepo;
        }

        [Fact]
        public async Task AddCustomerAsync()
        {
            using(var ctx = new XmlImporterDbContext(_dbContextOptions))
            {
                var customerRepository = GetCustomerRepository(ctx);
                var generatedCustomer = MockData.GenerateRandomCustomer(Guid.NewGuid());
                await customerRepository.AddAsync(generatedCustomer);

                var newCustomer = await ctx.Customers.Where(c => c.Id == generatedCustomer.Id).SingleAsync();
                newCustomer.Should().BeEquivalentTo(generatedCustomer, options => options.Excluding(c => c.Id));
            }
        }

        [Fact]
        public async Task GetCustomerAsync()
        {
            using (var ctx = new XmlImporterDbContext(_dbContextOptions))
            {
                var customerRepository = GetCustomerRepository(ctx);
                var generatedCustomer = MockData.GenerateRandomCustomer(Guid.NewGuid());
                await customerRepository.AddAsync(generatedCustomer);
               
                var newCustomer = await customerRepository.GetAsync(generatedCustomer.Id);
                newCustomer.Should().BeEquivalentTo(generatedCustomer, options => options.Excluding(c => c.Id));
            }
        }

        [Fact]
        public async Task DeleteCustomerAsync()
        {
            using (var ctx = new XmlImporterDbContext(_dbContextOptions))
            {
                var customerRepository = GetCustomerRepository(ctx);
                var generatedCustomer = MockData.GenerateRandomCustomer(Guid.NewGuid());
               
                await customerRepository.AddAsync(generatedCustomer);
                
                var newCustomer = await ctx.Customers.Where(c => c.Id == generatedCustomer.Id).SingleAsync();               
                newCustomer.Should().BeEquivalentTo(generatedCustomer, options => options.Excluding(c => c.Id));

                await customerRepository.DeleteAsync(newCustomer);
               
                var deletedCustomer = await ctx.Customers.Where(c => c.Id == generatedCustomer.Id).SingleOrDefaultAsync();
                deletedCustomer.Should().BeNull();
            }
        }

        [Fact]
        public async Task UpdateCustomerAsync()
        {
            using (var ctx = new XmlImporterDbContext(_dbContextOptions))
            {
                var customerRepository = GetCustomerRepository(ctx);
                var generatedCustomer = MockData.GenerateRandomCustomer(Guid.NewGuid());

                await customerRepository.AddAsync(generatedCustomer);

                var newCustomer = await ctx.Customers.Where(c => c.Id == generatedCustomer.Id).SingleOrDefaultAsync();

                newCustomer.Should().BeEquivalentTo(generatedCustomer, options => options.Excluding(c => c.Id));

                ctx.Entry(newCustomer).State = EntityState.Detached;
                ctx.Entry(newCustomer.FullAddress).State = EntityState.Detached;     
                
                var updatedCustomer =  MockData.GenerateRandomFullAddress(newCustomer.Id, newCustomer.FullAddressId);
                await customerRepository.UpdateAsync(updatedCustomer);

                var updatedCustomerEntity = await ctx.Customers.Where(c => c.Id == updatedCustomer.Id).SingleAsync();
                updatedCustomer.Should().BeEquivalentTo(updatedCustomerEntity);
            }
        }
    }
}
