using ECommerce.Application.Features.Commands.Order.CreateOrder;
using ECommerce.Application.Features.Queries.Order.GetAllOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
        {
            var response = await _mediator.Send(getAllOrdersQueryRequest);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            var response = await _mediator.Send(createOrderCommandRequest);

            return Ok(response);
        }
    }
}
