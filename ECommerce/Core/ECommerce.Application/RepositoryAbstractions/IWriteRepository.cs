using ECommerce.Domain.Entities.Common;

namespace ECommerce.Application.RepositoryAbstractions
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        bool Update(T entity);
        bool Remove(T entity);
        bool RemoveRange(IEnumerable<T> entities);
        Task<bool> Remove(string id);

        Task<int> SaveAsync();
    }
}
