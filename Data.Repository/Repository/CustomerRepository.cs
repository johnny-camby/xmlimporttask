using Data.Repository.Entities;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repository.Repository
{
    public class CustomerRepository : IXmlImporterRepository<Customer>
    {
        private readonly XmlImporterDbContext _ctx;
        public CustomerRepository(XmlImporterDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task AddEntityCollectionAsync(IEnumerable<Customer> entityCollection)
        {
            if (entityCollection == null)
            {
                throw new ArgumentNullException(nameof(entityCollection));
            }
            await _ctx.Customers.AddRangeAsync(entityCollection);
            await SaveAsync();
        }

        public async Task AddAsync(Customer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _ctx.Customers.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(Customer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.FromResult(_ctx.FullAddress.Remove(entity.FullAddress));
            await Task.FromResult(_ctx.Customers.Remove(entity));
            await SaveAsync();
        }

        public async Task<IEnumerable<Customer>> GetAsync()
        {
            return await _ctx.Customers.Include(f => f.FullAddress).ToListAsync();
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            return await _ctx.Customers.Include(f => f.FullAddress).Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistingAsync(Guid id)
        {
            return await _ctx.Customers.Include(f => f.FullAddress).AnyAsync(u => u.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _ctx.SaveChangesAsync() > 0);
        }

        public async Task UpdateAsync(Customer entity)
        {
            var address = new FullAddress
            {
                FullAddressId = entity.FullAddressId,
                Address = entity.FullAddress.Address,
                City = entity.FullAddress.City,
                Country = entity.FullAddress.Country,
                PostalCode = entity.FullAddress.PostalCode,
                Region = entity.FullAddress.Region
            };
            _ctx.FullAddress.Update(address);
            _ctx.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }
    }
}
