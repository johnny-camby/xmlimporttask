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

        public async Task UpdateAsync(Order entity)
        {
            var shipInfo = new ShipInfo 
            {
                ShipAddress = entity.ShipInfo.ShipAddress,
                ShipCity = entity.ShipInfo.ShipCity,
                ShipCountry = entity.ShipInfo.ShipCountry,
                ShipInfoId = entity.ShipInfoId,
                ShipName = entity.ShipInfo.ShipName,
                ShippedDate  = entity.ShipInfo.ShippedDate,
                ShipPostalCode = entity.ShipInfo.ShipPostalCode,
                ShipRegion = entity.ShipInfo.ShipRegion,
                ShipVia = entity.ShipInfo.ShipVia,
                Freight = entity.ShipInfo.Freight                
            };
            _ctx.ShipInfo.Update(shipInfo);
            _ctx.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }
    }
}
