using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, IEnumerable<GetBasketItemsQueryResponse>>
    {

        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IEnumerable<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)

            => (await _basketService.GetBasketItemsAsync()).Select(ba => new GetBasketItemsQueryResponse
            {
                BasketItemId = ba.Id,
                Name = ba.Product.Name,
                Price = ba.Product.Price,
                Quantity = ba.Quantity
            });
    }
}
