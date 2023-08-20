using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerce.Application.RepositoryAbstractions
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool asNoTracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> query, bool asNoTracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> query, bool asNoTracking = true);
        Task<T> GetByIdAsync(Guid id, bool asNoTracking = true);
    }
}
