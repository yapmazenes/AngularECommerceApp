using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICompletedOrderReadRepository _completedOrderReadRepository;

        public GetAllOrdersQueryHandler(IOrderReadRepository orderReadRepository, ICompletedOrderReadRepository completedOrderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
            _completedOrderReadRepository = completedOrderReadRepository;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var orderQuery = _orderReadRepository.GetAll()
                            .Include(x => x.Basket)
                                .ThenInclude(x => x.User)
                            .Include(x => x.Basket)
                                .ThenInclude(x => x.BasketItems)
                                    .ThenInclude(x => x.Product);

            var skippedOrdersQuery = orderQuery
                .Skip(request.Page * request.Size)
                .Take(request.Size);


            var unCompletedOrderQuery = from order in skippedOrdersQuery
                                        join completedOrders in _completedOrderReadRepository.Table
                                        on order.Id equals completedOrders.OrderId into unCompletedOrdersQuery
                                        from unCompletedOrders in unCompletedOrdersQuery.DefaultIfEmpty()
                                        select new
                                        {
                                            Id = order.Id,
                                            CreatedDate = order.CreatedDate,
                                            OrderCode = order.OrderCode,
                                            TotalPrice = order.Basket.BasketItems.Sum(y => y.Product.Price * y.Quantity),
                                            UserName = order.Basket.User.UserName ?? "",
                                            Completed = unCompletedOrders != null
                                        };

            var datas = await unCompletedOrderQuery.ToListAsync();

            var count = await orderQuery.CountAsync();

            //await Task.WhenAll(datasTask, countTask);

            return new()
            {
                Datas = datas,

                TotalCount = count,
            };
        }
    }
}
