using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class ProductImageFileReadRepository : ReadRepository<Domain.Entities.ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
