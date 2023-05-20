using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public ICollection<Order> Orders { get; set; }
    }
}
