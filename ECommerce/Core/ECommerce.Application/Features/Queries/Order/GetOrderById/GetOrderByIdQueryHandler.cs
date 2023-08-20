using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICompletedOrderReadRepository _completedOrderReadRepository;

        public GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository, ICompletedOrderReadRepository completedOrderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
            _completedOrderReadRepository = completedOrderReadRepository;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var dataQuery = _orderReadRepository.GetAll()
                .Include(o => o.Basket)
                    .ThenInclude(o => o.BasketItems)
                        .ThenInclude(bi => bi.Product);

            return await (from order in dataQuery
                          join completedOrder in _completedOrderReadRepository.Table
                          on order.Id equals completedOrder.OrderId into completedOrderQuery
                          from completedOrder in completedOrderQuery.DefaultIfEmpty()
                          select new GetOrderByIdQueryResponse
                          {
                              Id = order.Id,
                              Address = order.Address,
                              CreatedDate = order.CreatedDate,
                              Description = order.Description,
                              OrderCode = order.OrderCode,
                              BasketItems = order.Basket.BasketItems.Select(y => new
                              {
                                  y.Product.Name,
                                  y.Product.Price,
                                  y.Quantity
                              }),
                              Completed = completedOrder != null
                          }).FirstOrDefaultAsync(x => x.Id == request.Id) ?? new GetOrderByIdQueryResponse();
        }
    }
}
