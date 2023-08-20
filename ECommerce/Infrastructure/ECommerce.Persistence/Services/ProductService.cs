using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.RepositoryAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IQRCodeService _qrCodeService;

        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qrCodeService)
        {
            _productReadRepository = productReadRepository;
            _qrCodeService = qrCodeService;
        }

        public async Task<byte[]> GetQRCodeByProductAsync(Guid productId)
        {
            var product = await _productReadRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");

            var qrObject = new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedDate
            };

            var qrText = JsonSerializer.Serialize(qrObject);

            return _qrCodeService.GenerateQRCode(qrText);
        }
    }
}
