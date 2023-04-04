using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IXmlImporterRepository
    { }

    public interface IXmlImporterRepository<T> : IXmlImporterRepository
        where T : class, new()
    {
        Task AddAsync(T entity);
        Task AddEntityCollectionAsync(IEnumerable<T> entity);
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(Guid id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> IsExistingAsync(Guid id);
        Task<bool> SaveAsync();
    }
}
