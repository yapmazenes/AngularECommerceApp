using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class BasketWriteRepository : WriteRepository<Basket>, IBasketWriteRepository
    {
        public BasketWriteRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
