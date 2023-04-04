using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IBusinessMainService { }
    public interface IBusinessMainService<T, TEntityCreateUpdateDto> : IBusinessMainService
        where T : class, new()
        where TEntityCreateUpdateDto : class, new()
    {
        Task AddAsync(TEntityCreateUpdateDto entity);
        Task<List<T>> GetAsync();
        Task<T> GetAsync(Guid id);
        Task UpdateAsync(TEntityCreateUpdateDto entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}
