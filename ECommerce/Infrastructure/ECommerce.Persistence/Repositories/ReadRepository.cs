using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Domain.Entities.Common;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ECommerceDbContext _context;

        public ReadRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool asNoTracking = true) => asNoTracking ? Table.AsNoTracking() : Table.AsQueryable();

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> query, bool asNoTracking = true) => asNoTracking ? Table.AsNoTracking().Where(query) : Table.Where(query);

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> query, bool asNoTracking = true)
        {
            var queriable = Table.AsQueryable();

            if (asNoTracking)
            {
                queriable = Table.AsNoTracking();
            }

            return await queriable.FirstOrDefaultAsync(query);
        }

        public async Task<T> GetByIdAsync(string id, bool asNoTracking = true)
        {
            var queriable = Table.AsQueryable();

            if (asNoTracking)
            {
                queriable = Table.AsNoTracking();
            }

            return await queriable.FirstOrDefaultAsync(q => q.Id == Guid.Parse(id));
        }
    }
}
