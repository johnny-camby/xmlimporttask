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
    public class FullAddressRepository : IXmlImporterRepository<FullAddress>
    {
        private readonly XmlImporterDbContext _ctx;
        public FullAddressRepository(XmlImporterDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task AddAsync(FullAddress entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _ctx.FullAddress.AddAsync(entity);
            await SaveAsync();
        }

        public async Task AddEntityCollectionAsync(IEnumerable<FullAddress> entityCollection)
        {
            if (entityCollection == null)
            {
                throw new ArgumentNullException(nameof(entityCollection));
            }
            await _ctx.FullAddress.AddRangeAsync(entityCollection);
            await SaveAsync();
        }

        public async Task DeleteAsync(FullAddress entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await Task.FromResult(_ctx.FullAddress.Remove(entity));
        }

        public async Task<IEnumerable<FullAddress>> GetAsync()
        {
            return await _ctx.FullAddress.ToListAsync();
        }

        public async Task<FullAddress> GetAsync(Guid id)
        {
            return await _ctx.FullAddress.Where(u => u.FullAddressId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistingAsync(Guid id)
        {
            return await _ctx.FullAddress.AnyAsync(u => u.FullAddressId == id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _ctx.SaveChangesAsync() > 0);
        }

#pragma warning disable 1998
        public async Task UpdateAsync(FullAddress entity)
        {
            //no implementation required because of the entity tracking by the dbcontext.
        }
#pragma warning restore 1998
    }
}
