using ECommerce.Application.Abstractions.Hubs;
using ECommerce.Application.RepositoryAbstractions;
using MediatR;

namespace ECommerce.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductHubService _productHubService;
        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService)
        {
            _productWriteRepository = productWriteRepository;
            _productHubService = productHubService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            });

            var result = await _productWriteRepository.SaveAsync();

            if (result > 0)
            {
                await _productHubService.ProductAddedMessageAsync($"{request.Name} product added...");
            }

            return new();
        }
    }
}
