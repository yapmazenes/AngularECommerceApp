using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetAllOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var orderQuery = _orderReadRepository.GetAll()
                            .Include(x => x.Basket)
                                .ThenInclude(x => x.User)
                            .Include(x => x.Basket)
                                .ThenInclude(x => x.BasketItems)
                                    .ThenInclude(x => x.Product);

            var datasTask = await orderQuery
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(x => new
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    OrderCode = x.OrderCode,
                    TotalPrice = x.Basket.BasketItems.Sum(y => y.Product.Price * y.Quantity),
                    UserName = x.Basket.User.UserName ?? ""
                }).ToListAsync();

            var countTask = await orderQuery.CountAsync();

            //await Task.WhenAll(datasTask, countTask);

            return new()
            {
                Datas = datasTask,

                TotalCount = countTask,
            };
        }
    }
}
