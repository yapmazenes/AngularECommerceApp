using ECommerce.Application.RepositoryAbstractions;
using MediatR;

namespace ECommerce.Application.Features.Commands.Product.UpdateProductStockWithQRCode
{
    public class UpdateProductStockWithQRCodeCommandHandler : IRequestHandler<UpdateProductStockWithQRCodeCommandRequest, UpdateProductStockWithQRCodeCommandResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        public UpdateProductStockWithQRCodeCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<UpdateProductStockWithQRCodeCommandResponse> Handle(UpdateProductStockWithQRCodeCommandRequest request, CancellationToken cancellationToken)
        {

            var product = await _productReadRepository.GetByIdAsync(request.ProductId, false) ?? throw new Exception("Product not found");
            product.Stock = request.Stock;

            await _productWriteRepository.SaveAsync();

            return new();
        }
    }
}
