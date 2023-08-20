using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<byte[]> GetQRCodeByProductAsync(Guid productId);
    }
}
