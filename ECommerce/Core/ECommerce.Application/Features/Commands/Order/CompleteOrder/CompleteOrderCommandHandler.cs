using ECommerce.Application.Exceptions;
using ECommerce.Application.RepositoryAbstractions;
using MediatR;

namespace ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;

        public CompleteOrderCommandHandler(IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository)
        {
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderReadRepository.GetByIdAsync(request.OrderId);

            var response = new CompleteOrderCommandResponse();

            if (order != null)
            {
                response.Status = await _completedOrderWriteRepository.AddAsync(new Domain.Entities.CompletedOrder
                {
                    OrderId = order.Id,
                });

                await _completedOrderWriteRepository.SaveAsync();
            }

            return response;
        }
    }
}
