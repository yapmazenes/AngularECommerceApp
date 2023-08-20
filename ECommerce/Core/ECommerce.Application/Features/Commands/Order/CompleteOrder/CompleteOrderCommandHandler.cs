using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
        private readonly IMailService _mailService;

        public CompleteOrderCommandHandler(IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, IMailService mailService)
        {
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderReadRepository.Table
                                   .Include(o => o.Basket)
                                    .ThenInclude(b => b.User)
                                        .FirstOrDefaultAsync(x => x.Id == request.OrderId);

            var response = new CompleteOrderCommandResponse();

            if (order != null)
            {
                await _completedOrderWriteRepository.AddAsync(new Domain.Entities.CompletedOrder
                {
                    OrderId = order.Id,
                });

                response.Status = await _completedOrderWriteRepository.SaveAsync() > 0;

                if (response.Status)
                {
                    await _mailService.SendCompletedOrderMailAsync(order.Basket.User.Email, order.OrderCode, order.CreatedDate, order.Basket.User.UserName);
                }
            }

            return response;
        }
    }
}
