using Data.Repository.Entities;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Repository
{
    public class OrderRepository : IXmlImporterRepository<Order>
    {
        private readonly XmlImporterDbContext _ctx;
        public OrderRepository(XmlImporterDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task AddAsync(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _ctx.Orders.AddAsync(entity);
            await SaveAsync();
        }

        public async Task AddEntityCollectionAsync(IEnumerable<Order> entityCollection)
        {
            if (entityCollection == null)
            {
                throw new ArgumentNullException(nameof(entityCollection));
            }
            await _ctx.Orders.AddRangeAsync(entityCollection);
        }

        public async Task DeleteAsync(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await Task.FromResult(_ctx.Orders.Remove(entity));
            await SaveAsync();
        }

        public async Task<IEnumerable<Order>> GetAsync()
        {
            return await _ctx.Orders.Include(f => f.ShipInfo).ToListAsync();
        }

        public async Task<Order> GetAsync(Guid id)
        {
            return await _ctx.Orders.Include(f => f.ShipInfo).Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistingAsync(Guid id)
        {
            return await _ctx.Orders.Include(f => f.ShipInfo).AnyAsync(u => u.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _ctx.SaveChangesAsync() > 0);
        }

#pragma warning disable 1998
        public async Task UpdateAsync(Order entity)
        {
            //no implementation required because of the entity tracking by the dbcontext.
        }
#pragma warning restore 1998
    }

}
