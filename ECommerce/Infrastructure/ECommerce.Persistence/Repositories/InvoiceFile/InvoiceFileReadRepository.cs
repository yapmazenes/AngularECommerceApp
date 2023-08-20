using ECommerce.Application.RepositoryAbstractions.Invoice;
using ECommerce.Persistence.Contexts;

namespace ECommerce.Persistence.Repositories
{
    public class InvoiceFileReadRepository : ReadRepository<Domain.Entities.InvoiceFile>, IInvoiceFileReadRepository
    {
        public InvoiceFileReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
