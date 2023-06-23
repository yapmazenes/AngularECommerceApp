using ECommerce.Application.Abstractions.Hubs;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Exceptions;
using ECommerce.Application.RepositoryAbstractions;
using MediatR;

namespace ECommerce.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IBasketService _basketService;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderHubService _orderHubService;
        public CreateOrderCommandHandler(IOrderWriteRepository orderWriteRepository, IBasketService basketService, IOrderHubService orderHubService)
        {
            _orderWriteRepository = orderWriteRepository;
            _basketService = basketService;
            _orderHubService = orderHubService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var userActiveBasket = await _basketService.GetUserActiveBasket() ?? throw new NotFoundBasketException();

            await _orderWriteRepository.AddAsync(new Domain.Entities.Order
            {
                Address = request.Address,
                BasketId = userActiveBasket.Id,
                Description = request.Description,
            });

            await _orderWriteRepository.SaveAsync();

            await _orderHubService.OrderAddedMessageAsync("We have got a new order. please check it!");
            return new();
        }
    }
}
