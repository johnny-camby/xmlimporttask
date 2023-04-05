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
    public class ShipInfoRepository : IXmlImporterRepository<ShipInfo>
    {
        private readonly XmlImporterDbContext _ctx;
        public ShipInfoRepository(XmlImporterDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task AddAsync(ShipInfo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _ctx.ShipInfo.AddAsync(entity);
            await SaveAsync();
        }

        public async Task AddEntityCollectionAsync(IEnumerable<ShipInfo> entityCollection)
        {
            if (entityCollection == null)
            {
                throw new ArgumentNullException(nameof(entityCollection));
            }
            await _ctx.ShipInfo.AddRangeAsync(entityCollection);
            await SaveAsync();
        }

        public async Task DeleteAsync(ShipInfo entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await Task.FromResult(_ctx.ShipInfo.Remove(entity));
        }

        public async Task<IEnumerable<ShipInfo>> GetAsync()
        {
            return await _ctx.ShipInfo.ToListAsync();
        }

        public async Task<ShipInfo> GetAsync(Guid id)
        {
            return await _ctx.ShipInfo.Where(u => u.ShipInfoId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistingAsync(Guid id)
        {
            return await _ctx.ShipInfo.AnyAsync(u => u.ShipInfoId == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _ctx.SaveChangesAsync() > 0);
        }

        public async Task UpdateAsync(ShipInfo entity)
        {
            _ctx.ShipInfo.Update(entity);
            await SaveAsync();
        }
    }

}
