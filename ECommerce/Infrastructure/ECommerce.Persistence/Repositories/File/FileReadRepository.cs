using ECommerce.Application.RepositoryAbstractions.File;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
