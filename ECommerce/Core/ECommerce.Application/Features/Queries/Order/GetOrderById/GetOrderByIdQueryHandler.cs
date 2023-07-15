using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderReadRepository.GetAll()
                .Include(o => o.Basket)
                    .ThenInclude(o => o.BasketItems)
                        .ThenInclude(bi => bi.Product)
                        .Select(x => new GetOrderByIdQueryResponse
                        {
                            Id = x.Id,
                            Address = x.Address,
                            CreatedDate = x.CreatedDate,
                            Description = x.Description,
                            OrderCode = x.OrderCode,
                            BasketItems = x.Basket.BasketItems.Select(y => new
                            {
                                y.Product.Name,
                                y.Product.Price,
                                y.Quantity
                            })
                        }).FirstOrDefaultAsync(x => x.Id == request.Id);

            return data;
        }
    }
}
