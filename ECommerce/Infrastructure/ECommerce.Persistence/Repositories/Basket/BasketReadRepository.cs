using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class BasketReadRepository : ReadRepository<Basket>, IBasketReadRepository
    {
        public BasketReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
